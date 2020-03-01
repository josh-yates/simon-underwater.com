using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Pages.Shared;

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

        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public string Description { get; set; }

        public int Id { get; set; }

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

            Id = album.Id;
            Description = album.Description;
            Title = album.Title;

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

            var albumChanged = false;

            if (album.Title != Title)
            {
                album.Title = Title;
                albumChanged = true;
            }

            if (album.Description != Description)
            {
                album.Description = Description;
                albumChanged = true;
            }

            if (albumChanged)
            {
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToPage();
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
    }
}