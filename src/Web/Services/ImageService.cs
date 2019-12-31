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
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

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
                     .AutoOrient()
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

        public string GetWebVersionUrl(Data.Models.Image image, bool isThumbnail = false)
        {
            var url = Path.Combine(isThumbnail ? _imageOptions.ThumbnailsBaseDirectory : _imageOptions.WebImagesBaseDirectory, image.OnDiskName);

            if (!url.StartsWith("/"))
            {
                return "/" + url;
            }

            return url;
        }

        public List<string> ValidateImageUpload(IFormFile upload)
        {
            var errorMessages = new List<string>();
            var extension = Path.GetExtension(upload.FileName).ToLower();

            if (!_imageOptions.PermittedExtensions.Contains(extension))
            {
                errorMessages.Add($"File extension {extension} is not permitted for photo uploads.");
            }

            return errorMessages;
        }

        public async Task<Data.Models.Image> SaveImageUploadAsync(IFormFile upload, User author)
        {
            var extension = Path.GetExtension(upload.FileName).ToLower();
            var image = new Data.Models.Image
            {
                OnDiskName = Guid.NewGuid().ToString() + extension,
                OriginalName = upload.FileName,
                User = author,
                UploadedAt = DateTimeOffset.Now
            };

            var stream = upload.OpenReadStream();
            var timestampString = (string)SixLabors.ImageSharp.Image.Identify(stream).Metadata.ExifProfile.GetValue(SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag.DateTime).Value;
            if (DateTimeOffset.TryParse(timestampString, out var parsedTimestamp))
            {
                image.TakenAt = parsedTimestamp;
            }

            var savePath = Path.Combine(_env.ContentRootPath, image.OnDiskName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await upload.CopyToAsync(fileStream);
            }
        }
    }
}