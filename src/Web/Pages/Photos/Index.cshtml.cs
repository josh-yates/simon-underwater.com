using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Data.Models;
using DataAccess.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Pages.Shared;
using Web.Services;

namespace Web.Pages.Photos
{
    public class IndexModel : BasePageModel
    {
        private readonly AppDbContext _dbContext;
        public readonly ImageService _imageService;

        public IndexModel(
            AppDbContext dbContext,
            ImageService imageService
        )
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
        }

        [BindProperty(SupportsGet = true)]
        public int P { get; set; } = 1;
        public PaginatedResult<Image> Images { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var index = P > 0 ? P : 1;

            Images = await _dbContext
                .Images
                .Where(i => !i.IsDeleted)
                .ToPaginatedResultAsync(index, 10);
            
            if (Images.Count <= 0 && index > 1 && index > Images.TotalPages)
            {
                return RedirectToPage(new { p = Images.TotalPages });
            }

            return Page();
        }
    }
}