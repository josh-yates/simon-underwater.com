using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Convolution;

namespace Site.Operations
{
    public static class BlurOperation
    {
        public static IImageProcessingContext<Rgba32> Apply(IImageProcessingContext<Rgba32> image)
        {
            return image.GaussianBlur(50);
        }
    }
}