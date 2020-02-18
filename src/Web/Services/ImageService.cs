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
using Core.FileSystem;

namespace Web.Services
{
    public class ImageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ImageOptions _imageOptions;
        private readonly IFileSystemHub _fileSystemHub;
        private readonly FileSystemOptions _fileSystemOptions;

        public ImageService(
            IWebHostEnvironment env,
            IOptions<ImageOptions> imageOptions,
            IFileSystemHub fileSystemHub,
            IOptions<FileSystemOptions> fileSystemOptions
        )
        {
            _env = env ?? throw new ArgumentNullException(nameof(env));
            _imageOptions = imageOptions.Value ?? throw new ArgumentNullException(nameof(imageOptions.Value));
            _fileSystemHub = fileSystemHub ?? throw new ArgumentNullException(nameof(fileSystemHub));
            _fileSystemOptions = fileSystemOptions.Value ?? throw new ArgumentNullException(nameof(fileSystemOptions.Value));
        }
        public void GenerateWebVersionForImage(Data.Models.Image image, bool isThumbnail = false)
        {
            // TODO save the image to a stream, then should be able to save it via the hub
            var inputPath = _env.ContentRootFileProvider.GetFileInfo(Path.Combine(_fileSystemOptions.UploadsBaseDirectory, image.OnDiskName)).PhysicalPath;
            var outputBaseDirectory = isThumbnail ? _fileSystemOptions.ThumbnailsBaseDirectory : _fileSystemOptions.WebImagesBaseDirectory;
            var outputPath = _env.WebRootFileProvider.GetFileInfo(Path.Combine(outputBaseDirectory, image.OnDiskName)).PhysicalPath;

            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

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

        public async Task<FileStream> GetOriginalImage(Data.Models.Image image)
        {
            return (FileStream)(await _fileSystemHub.Get(FileSystemKeys.Uploads).GetFileStreamAsync(image.OnDiskName));
        }

        public async Task<FileStream> GetWebVersionForImage(Data.Models.Image image, bool isThumbnail = false)
        {
            return (FileStream)(await _fileSystemHub
                .Get(isThumbnail ? FileSystemKeys.Thumbnails : FileSystemKeys.WebImages)
                .GetFileStreamAsync(image.OnDiskName));
        }

        public async Task RemoveGeneratedImages(Data.Models.Image image)
        {
            await _fileSystemHub.Get(FileSystemKeys.WebImages).DeleteFileAsync(image.OnDiskName);
            await _fileSystemHub.Get(FileSystemKeys.Thumbnails).DeleteFileAsync(image.OnDiskName);
        }

        public string GetImageUrl(Data.Models.Image image, bool isThumbnail = false)
        {
            return Url.Combine(isThumbnail ? _imageOptions.ThumbnailsBaseDirectory : _imageOptions.WebImagesBaseDirectory, image.OnDiskName);
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

            image.TakenAt = DateTimeOffset.Now;
            image.TakenAtSource = Data.Enums.TakenAtSourceType.CurrentTime;

            var stream = upload.OpenReadStream();
            var exifData = SixLabors.ImageSharp.Image.Identify(stream).Metadata.ExifProfile;

            // PNGs don't have exif data
            if (exifData != null)
            {
                var timestampData = exifData.GetValue(SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag.DateTime);

                if (timestampData != null && DateTimeOffset.TryParse(timestampData.ToString(), out var parsedTimestamp))
                {
                    image.TakenAt = parsedTimestamp;
                    image.TakenAtSource = Data.Enums.TakenAtSourceType.FromExif;
                }
            }

            var savePath = Path.Combine(_env.ContentRootPath, _imageOptions.UploadsBaseDirectory, image.OnDiskName);

            Directory.CreateDirectory(Path.GetDirectoryName(savePath));

            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                stream.Seek(0, SeekOrigin.Begin);
                await stream.CopyToAsync(fileStream);
            }

            return image;
        }
    }
}