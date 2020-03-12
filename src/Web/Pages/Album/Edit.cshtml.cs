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
using Web.Utilities.UI;

namespace Web.Pages.Album
{
    public class EditModel : BasePageModel
    {
        private readonly AppDbContext _dbContext;
        public EditModel(
            AppDbContext dbContext
        )
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        // Image filters
        [BindProperty(SupportsGet = true)]
        public int P { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public string S { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime F { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime T { get; set; }

        // Album data
        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public string Description { get; set; }

        public PaginatedResult<Checkbox<Image>> ImageCheckboxes { get; set; }
        public int Id { get; set; }
        public DateTime? DatePickerMin { get; set; }
        public DateTime? DatePickerMax { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            var album = await _dbContext
                .Albums
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();

            if (album == null)
            {
                return NotFound();
            }

            await SetFormData();

            Id = album.Id;
            Description = album.Description;
            Title = album.Title;

            // TODO see if this can be a left join for single execution
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
                var startDate = F.StartOfDay();
                query = query
                    .Where(i => i.TakenAt >= startDate);
            }

            if (T != default(DateTime))
            {
                var endDate = T.EndOfDay();
                query = query
                    .Where(i => i.TakenAt <= endDate);
            }

            var images = await query
                .ToPaginatedResultAsync(index, 10);
            
            if (images.Count <= 0 && index > 1 && index > images.TotalPages)
            {
                return RedirectToPage(new { p = images.TotalPages, s = S, f = F.ToString("yyyy-MM-dd"), t = T.ToString("yyyy-MM-dd") });
            }

            var imageIds = images.Select(i => i.Id);

            var imagesInAlbum = await _dbContext
                .AlbumImages
                .Where(ai => ai.AlbumId == album.Id &&
                    imageIds.Contains(ai.ImageId))
                .Select(ai => ai.ImageId)
                .ToArrayAsync();
            
            var imageCheckboxList = images
                .Select(i => new Checkbox<Image>
                {
                    Item = i,
                    Checked = imagesInAlbum.Contains(i.Id)
                })
                .ToList();
            
            ImageCheckboxes = new PaginatedResult<Checkbox<Image>>(imageCheckboxList, images.TotalCount, index, 10);

            return Page();
        }

        public async Task<IActionResult> OnPostUpdate(int id)
        {
            var album = await _dbContext
                .Albums
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();

            if (album == null)
            {
                return NotFound();
            }

            album.Title = Title;
            album.Description = Description;

            var viewImageIds = ImageCheckboxes.Select(ic => ic.Item.Id);

            var existingImageIds = await _dbContext
                .AlbumImages
                .Where(ai => ai.AlbumId == album.Id)
                .Where(ai => viewImageIds.Contains(ai.ImageId))
                .Select(ai => ai.ImageId)
                .ToArrayAsync();
            
            // Find additions; checked images that aren't in the existing list
            var imagesAdded = ImageCheckboxes
                .Where(ic => ic.Checked)
                .Select(ic => ic.Item.Id)
                .Except(existingImageIds);


            // Find deletions; unchecked images that are in the existing list
            var imagesRemoved = ImageCheckboxes
                .Where(ic => !ic.Checked)
                .Select(ic => ic.Item.Id)
                .Intersect(existingImageIds);

            _dbContext
                .AlbumImages
                .AddRange(imagesAdded.Select(i => new AlbumImage
                {
                    ImageId = i,
                    AlbumId = album.Id
                }));
            
            _dbContext
                .RemoveRange(imagesRemoved.S)

            await _dbContext.SaveChangesAsync();

            return RedirectToPage(new { p = P, s = S, f = F.ToString("yyyy-MM-dd"), t = T.ToString("yyyy-MM-dd") });
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            var album = await _dbContext
                .Albums
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();

            if (album == null)
            {
                return NotFound();
            }

            _dbContext.Albums.Remove(album);

            await _dbContext.SaveChangesAsync();

            return RedirectToPage("../Albums/Index");
        }

        private async Task SetFormData()
        {
            var query = _dbContext
                .Images
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