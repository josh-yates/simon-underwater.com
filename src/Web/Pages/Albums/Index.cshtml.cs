using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Data.Models;
using DataAccess.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Pages.Shared;
using Web.Utilities;

namespace Web.Pages.Albums
{
    public class IndexModel : BasePageModel
    {
        private readonly AppDbContext _dbContext;

        public IndexModel(
            AppDbContext dbContext
        )
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        // TODO: can we unify the filters into a partial?
        [BindProperty(SupportsGet = true)]
        public int P { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public string S { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime F { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime T { get; set; }
        public PaginatedResult<Album> Albums { get; set; }
        public DateTime? DatePickerMin { get; set; }
        public DateTime? DatePickerMax { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await SetFormData();

            var index = P > 0 ? P : 1;

            var query = _dbContext
                .Albums
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(S))
            {
                var searchString = S
                    .Trim()
                    .ToLower();

                query = query
                    .Where(a => a.Title.Contains(searchString) ||
                        a.Description.Contains(searchString));
            }

            if (F != default(DateTime))
            {
                var startDate = F.StartOfDay();

                query = query
                    .Where(a => a
                        .AlbumImages
                        .Select(ai => ai.Image.TakenAt)
                        .Min() >= startDate);
            }

            if (T != default(DateTime))
            {
                var endDate = T.EndOfDay();

                query = query
                    .Where(a => a
                        .AlbumImages
                        .Select(ai => ai.Image.TakenAt)
                        .Max() <= endDate);
            }

            Albums = await query
                .ToPaginatedResultAsync(index, 10);

            if (Albums.Count <= 0 && index > 1 && index > Albums.TotalPages)
            {
                return RedirectToPage(new { p = Albums.TotalPages });
            }

            return Page();
        }

        private async Task SetFormData()
        {
            var query = _dbContext
                .Albums
                .SelectMany(a => a.AlbumImages)
                .Select(ai => ai.Image)
                .Where(i => !i.IsDeleted)
                .OrderBy(i => i.TakenAt)
                .Select(i => i.TakenAt);

            var min = await query.FirstOrDefaultAsync(); 
            if (min != default(DateTimeOffset))
            {
                DatePickerMin = min.DateTime;
            }

            var max = await query.LastOrDefaultAsync();
            if (max != default(DateTimeOffset))
            {
                DatePickerMax = max.DateTime;
            }
        }
    }
}