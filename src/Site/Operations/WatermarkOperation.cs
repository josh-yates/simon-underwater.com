using System;
using SixLabors.Fonts;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Text;
using SixLabors.Primitives;

namespace Site.Operations
{
    public static class WatermarkOperation
    {
        public static IImageProcessingContext<Rgba32> Apply(IImageProcessingContext<Rgba32> image)
        {
            var text = "© Simon Yates";
            var fontFamily = new FontCollection().Install("./fonts/watermark-font.ttf");
            var font = new Font(fontFamily, 10, FontStyle.Regular);

            Size imgSize = image.GetCurrentSize();

            float targetWidth = 800;
            float targetHeight = 200;

            // measure the text size
            SizeF size = TextMeasurer.Measure(text, new RendererOptions(font));

            // find out how much we need to scale the text to fill the space (up or down)
            float scalingFactor = Math.Min(targetWidth / size.Width, targetHeight / size.Height);

            // create a new font
            Font scaledFont = new Font(font, scalingFactor * font.Size);

            PointF anchor = new PointF(imgSize.Width - 50, imgSize.Height - 50);
            TextGraphicsOptions textGraphicOptions = new TextGraphicsOptions()
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            return image.DrawText(textGraphicOptions, text, scaledFont, new Rgba32(255, 255, 255, 0.7F), anchor);
        }
    }
}