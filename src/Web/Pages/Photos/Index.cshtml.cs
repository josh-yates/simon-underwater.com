using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Pages.Shared;
using Web.Services;

namespace Web.Pages.Photos
{
    public class IndexModel : BasePageModel
    {
        private readonly AppDbContext _dbContext;
        private readonly ImageService _imageService;
        public IndexModel(
            AppDbContext dbContext,
            ImageService imageService
        )
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
        }

        public IndividualPhotoModel IndividualPhoto { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (!string.IsNullOrEmpty(id) && int.TryParse(id, out var parsedId))
            {
                var image = await _dbContext
                    .Images
                    .Where(i => i.Id == parsedId && !i.IsDeleted)
                    .FirstOrDefaultAsync();
                
                if (image == null)
                {
                    return NotFound();
                }

                IndividualPhoto = new IndividualPhotoModel
                {
                    Description = image.Description,
                    TakenAt = image.TakenAt,
                    ImageUrl = _imageService.GetWebVersionUrl(image)
                };
            }
            else
            {

            }

            return Page();
        }
    }
}