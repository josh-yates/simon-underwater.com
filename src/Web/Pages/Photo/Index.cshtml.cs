using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Pages.Shared;
using Web.Services;

namespace Web.Pages.Photo
{
    public class IndexModel : BasePageModel
    {
        private readonly ImageService _imageService;
        private readonly AppDbContext _dbContext;

        public IndexModel(
            ImageService imageService,
            AppDbContext dbContext
        ) {
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public string ImageUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var image = await _dbContext.Images
                .Where(i => !i.IsDeleted && i.Id == id)
                .FirstOrDefaultAsync();
            
            if (image == null)
            {
                return NotFound();
            }

            ImageUrl = _imageService.GetImageUrl(image);

            return Page();
        }
    }
}