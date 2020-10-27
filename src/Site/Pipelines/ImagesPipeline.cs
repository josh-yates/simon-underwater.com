using System;
using System.Linq;
using System.Threading.Tasks;
using Site.Keys;
using Site.Operations;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.MetaData.Profiles.Exif;
using Statiq.Common;
using Statiq.Core;
using Statiq.Images;

namespace Site.Pipelines
{
    public class ImagesPipeline : Pipeline
    {
        public ImagesPipeline()
        {
            var exts = new[] { ".JPG", ".JPEG", ".PNG", ".TIFF" };
            InputModules = new ModuleList
            {
                new ReadFiles("*")
                    .Where(x => Task.FromResult(exts.Contains(x.Path.Extension.ToUpper())))
            };

            ProcessModules = new ModuleList
            {
                new SetMetadata(ImageDataKeys.TakenAt, Config.FromDocument(d =>
                {
                    using var stream = d.GetContentStream();
                    var image = Image.Load(stream, out var format);

                    var exifDateFormatted = string.Join(' ',
                        image
                            .MetaData
                            .ExifProfile
                            .GetValue(ExifTag.DateTime)
                            .ToString()
                            .Split(' ')
                            .Select((s, i) => i == 0 ?
                                s.Replace(':', '/') :
                                s));

                    return DateTime.TryParse(exifDateFormatted, out var parsed) ? parsed : default;
                })),
                new MutateImage()
                    .Operation(WatermarkOperation.Apply)
                    .Operation(CustomResizeOperation.Apply),
                new OrderDocuments(Config.FromDocument(d => d.GetDateTime(ImageDataKeys.TakenAt)))
                    .Descending()
            };

            OutputModules = new ModuleList
            {
                new WriteFiles()
            };
        }
    }
}