using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Web.Services;

namespace Web.Controllers
{
    public class ImagesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ImageService _imageService;
        public ImagesController(
            AppDbContext context,
            ImageService imageService
        )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
        }

        [HttpGet("/images/{filename}")]
        public async Task<IActionResult> GetImage(string filename)
        {
            var image = await FindImageByOnDiskName(filename);
            
            if (image == null)
            {
                return NotFound();
            }

            _imageService.GenerateWebVersionForImage(image);

            var contentType = GetContentType(filename);

            return File(_imageService.GetWebVersionForImage(image), contentType);
        }

        [HttpGet("/images/thumbnails/{filename}")]
        public async Task<IActionResult> GetThumbnail(string filename)
        {
            var image = await FindImageByOnDiskName(filename);
            
            if (image == null)
            {
                return NotFound();
            }

            _imageService.GenerateWebVersionForImage(image, true);

            var contentType = GetContentType(filename);

            return File(_imageService.GetWebVersionForImage(image, true), contentType);
        }

        private async Task<Image> FindImageByOnDiskName(string filename)
        {
            return await _context
                .Images
                .Where(i => !i.IsDeleted && i.OnDiskName == filename)
                .FirstOrDefaultAsync();
        }

        private string GetContentType(string filename)
        {
            string contentType;

            if (!new FileExtensionContentTypeProvider().TryGetContentType(filename, out contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }
    }
}