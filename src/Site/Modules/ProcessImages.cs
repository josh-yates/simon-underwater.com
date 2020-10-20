using System.Collections.Generic;
using System.IO;
using Site.Operations;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Statiq.Common;

namespace Site.Modules
{
    public class ProcessImages : ParallelSyncModule
    {
        protected override IEnumerable<IDocument> ExecuteInput(IDocument input, IExecutionContext context)
        {
            Image<Rgba32> image;
            IImageFormat imageFormat;
            long fileSizeInBytes;

            using (var stream = input.GetContentStream())
            {
                image = SixLabors.ImageSharp.Image.Load(stream, out imageFormat);
                fileSizeInBytes = stream.Length;
            }

            image.Mutate(ctx =>
            {
                var workingCtx = ctx;
                workingCtx = WatermarkOperation.Apply(workingCtx);
                workingCtx = CustomResizeOperation.Apply(workingCtx, fileSizeInBytes);
            });

            var destinationPath = input.Source.GetRelativeInputPath();
            var outputStream = new MemoryStream();
            image.Save(outputStream, imageFormat);
            outputStream.Seek(0, SeekOrigin.Begin);
            return new IDocument[] {input.Clone(destinationPath, context.GetContentProvider(outputStream, destinationPath.MediaType)) };
        }
    }
}