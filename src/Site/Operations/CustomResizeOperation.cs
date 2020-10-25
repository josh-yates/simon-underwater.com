using System;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Transforms;
using SixLabors.Primitives;

namespace Site.Operations
{
    public static class CustomResizeOperation
    {
        public static IImageProcessingContext<Rgba32> Apply(IImageProcessingContext<Rgba32> image)
        {
            Size imgSize = image.GetCurrentSize();
            var scalingFactor = Math.Max(imgSize.Height, imgSize.Width) / 1000F;

            if (scalingFactor <= 1)
            {
                return image;
            }

            return image.Resize(new ResizeOptions
            {
                Size = new Size((int)Math.Round(imgSize.Width / scalingFactor), (int)Math.Round(imgSize.Height / scalingFactor)),
                Position = AnchorPositionMode.Center,
                Mode = ResizeMode.Max
            });
        }
    }
}