using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
            var md5 = MD5.Create();
            md5.Initialize();

            InputModules = new ModuleList
            {
                new ReadFiles("images/*")
                    .Where(x => Task.FromResult(exts.Contains(x.Path.Extension.ToUpper())))
            };

            ProcessModules = new ModuleList
            {
                new SetMetadata(ImageDataKeys.TakenAt, Config.FromDocument(d =>
                {
                    using var stream = d.GetContentStream();

                    var image = Image.Load(stream, out var format);

                    if (image.MetaData == null || image.MetaData.ExifProfile == null)
                    {
                        return default;
                    }

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
                    .Operation(CustomResizeOperation.Apply)
            };

            OutputModules = new ModuleList
            {
                new WriteFiles()
            };
        }
    }

    public static class ImagesPipelineExtensions
    {
        public static string ToHex(this byte[] bytes, bool upperCase)
        {
            var result = new StringBuilder(bytes.Length * 2);

            for (int i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));

            return result.ToString();
        }
    }
}