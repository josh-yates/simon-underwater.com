using System;
using System.Collections.Generic;
using System.Globalization;
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
        public DateTime F { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime T { get; set; }

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

            if (F != default(DateTime))
            {
                query = query
                    .Where(i => i.TakenAt >= F);
            }

            if (T != default(DateTime))
            {
                query = query
                    .Where(i => i.TakenAt <= T);
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