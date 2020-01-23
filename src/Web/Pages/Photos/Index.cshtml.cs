using System;
using System.Collections.Generic;
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

        [BindProperty(SupportsGet = true)]
        public string S { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime M { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Y { get; set; }
        public ICollection<int> Years { get; set; }

        public PaginatedResult<Image> Images { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var index = P > 0 ? P : 1;

            var query = _dbContext
                .Images
                .Where(i => !i.IsDeleted);
            
            if (!string.IsNullOrWhiteSpace(S))
            {
                var searchString = S.Trim().ToLower();

                query = query
                    .Where(i => i.Description.Contains(searchString));
            }

            if (M != default(DateTime))
            {
                var start = new DateTime(M.Year, M.Month, 1);
                var end = start.AddMonths(1);

                query = query
                    .Where(i => i.TakenAt >= start && i.TakenAt < end);
            }

            Images = await query
                .ToPaginatedResultAsync(index, 10);
            
            if (Images.Count <= 0 && index > 1 && index > Images.TotalPages)
            {
                return RedirectToPage(new { p = Images.TotalPages });
            }

            return Page();
        }
    }
}