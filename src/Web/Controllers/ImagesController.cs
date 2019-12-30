using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
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
        public async Task<IActionResult> Get(string filename)
        {
            var image = await _context
                .Images
                .Where(i => !i.IsDeleted && i.OnDiskName == filename)
                .FirstOrDefaultAsync();
            
            // if (image == null)
            // {
            //     return NotFound();
            // }

            string contentType;

            if (!new FileExtensionContentTypeProvider().TryGetContentType(filename, out contentType))
            {
                contentType = "application/octet-stream";
            }

            return File(await _imageService.GenerateFileForImage(image), contentType);
        }
    }
}