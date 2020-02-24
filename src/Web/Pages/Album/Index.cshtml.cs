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

namespace Web.Pages.Album
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

        public int Id { get; set; }
        public PaginatedResult<Image> Images { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var index = P > 0 ? P : 1;

            var album = await _dbContext
                .Albums
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();

            if (album == null)
            {
                return NotFound();
            }

            Id = album.Id;
            Description = album.Description;
            Title = album.Title;

            var query = _dbContext
                .AlbumImages
                .Where(ai => ai.AlbumId == id &&
                    !ai.Image.IsDeleted)
                .Select(ai => ai.Image);

            From = await query
                .Select(i => i.TakenAt)
                .MinAsync();

            To = await query
                .Select(i => i.TakenAt)
                .MaxAsync();

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