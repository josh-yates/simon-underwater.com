using SixLabors.ImageSharp;
using System;
using System.IO;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Web.Utilities;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace Web.Services
{
    public class ImageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ImageOptions _imageOptions;
        public ImageService(
            IWebHostEnvironment env,
            IOptions<ImageOptions> imageOptions
        )
        {
            _env = env ?? throw new ArgumentNullException(nameof(env));
            _imageOptions = imageOptions.Value ?? throw new ArgumentException(nameof(imageOptions.Value));
        }
        public void GenerateWebVersionForImage(Data.Models.Image image, bool isThumbnail = false)
        {
            var inputPath = _env.ContentRootFileProvider.GetFileInfo(Path.Combine(_imageOptions.UploadsBaseDirectory, image.OnDiskName)).PhysicalPath;
            var outputBaseDirectory = isThumbnail ? _imageOptions.ThumbnailsBaseDirectory : _imageOptions.WebImagesBaseDirectory;
            var outputPath = _env.WebRootFileProvider.GetFileInfo(Path.Combine(outputBaseDirectory, image.OnDiskName)).PhysicalPath;

            using (var inputImage = SixLabors.ImageSharp.Image.Load(inputPath))
            {
                var resizeFactor = isThumbnail ? _imageOptions.ThumbnailResizeFactor : _imageOptions.WebImageResizeFactor;
                inputImage.Mutate(i => 
                    i.Resize(Convert.ToInt32(inputImage.Width * resizeFactor), Convert.ToInt32(inputImage.Height * resizeFactor))
                );
                inputImage.Save(outputPath);
            }
        }

        public FileStream GetOriginalImage(Data.Models.Image image)
        {
            var path = _env.ContentRootFileProvider.GetFileInfo(Path.Combine(_imageOptions.UploadsBaseDirectory, image.OnDiskName)).PhysicalPath;

            return File.OpenRead(path);
        }

        public FileStream GetWebVersionForImage(Data.Models.Image image, bool isThumbnail = false)
        {
            var baseDirectory = isThumbnail ? _imageOptions.ThumbnailsBaseDirectory : _imageOptions.WebImagesBaseDirectory;
            var path = _env.WebRootFileProvider.GetFileInfo(Path.Combine(baseDirectory, image.OnDiskName)).PhysicalPath;

            return File.OpenRead(path);
        }
    }
}