using System.Collections.Generic;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.MetaData.Profiles.Exif;
using SixLabors.ImageSharp.PixelFormats;
using Statiq.Common;

namespace Site.Modules
{
    public class ReadImageData : ParallelSyncModule
    {
        protected override IEnumerable<IDocument> ExecuteInput(IDocument input, IExecutionContext context)
        {
            Image<Rgba32> image;
            IImageFormat imageFormat;
            using (Stream stream = input.GetContentStream())
            {
                image = SixLabors.ImageSharp.Image.Load(stream, out imageFormat);
            }

            var takenAtMetadata = new KeyValuePair<string, object>(
                ImageDataKeys.TakenAt,
                image.MetaData.ExifProfile.GetValue(ExifTag.DateTime));
            var newMetadata = new KeyValuePair<string, object>[]
            {
                takenAtMetadata
            };

            return new IDocument[] { input.Clone(null, null, newMetadata)};
        }
    }

    public static class ImageDataKeys
    {
        public static readonly string TakenAt = nameof(TakenAt);
        public static readonly string Destination = nameof(Destination);
    }
}