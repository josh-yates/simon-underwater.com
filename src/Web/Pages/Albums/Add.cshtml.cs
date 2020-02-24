using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Web.Pages.Shared;

namespace Web.Pages.Albums
{
    public class AddModel : BasePageModel
    {
        private readonly AppDbContext _dbContext;

        public AddModel(
            AppDbContext dbContext
        )
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [BindProperty]
        public string Title { get; set; }

        [BindProperty]
        public string Description { get; set; }

        [BindProperty]
        public string Ids { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var album = new Data.Models.Album
            {
                Description = Description,
                Title = Title,
                AlbumImages = Ids
                    .Split(',')
                    .Select(s => s.Trim())
                    .Where(s => int.TryParse(s, out var _))
                    .Select(s => int.Parse(s))
                    .Select(i => new AlbumImage
                    {
                        ImageId = i
                    })
                    .ToArray()
            };

            _dbContext
                .Albums
                .Add(album);

            await _dbContext.SaveChangesAsync();

            return RedirectToPage("../album/index", new { id = album.Id });
        }
    }
}