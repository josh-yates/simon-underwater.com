using System;
using System.IO;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Hosting;

namespace Web.Services
{
    public class ImageService
    {
        private readonly IWebHostEnvironment _env;
        public ImageService(
            IWebHostEnvironment env
        )
        {
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }
        public Task<FileStream> GenerateFileForImage(Image image, bool isThumbnail = false)
        {
            return Task.FromResult(new FileStream(_env.WebRootFileProvider.GetFileInfo("raw/image.jpg").PhysicalPath, FileMode.Open));
        }
    }
}