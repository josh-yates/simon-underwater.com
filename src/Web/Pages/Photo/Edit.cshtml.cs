using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Data.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Pages.Shared;
using Web.Services;
using Web.Utilities;
using static Web.Utilities.DateTimeExtensions;

namespace Web.Pages.Photo
{
    public class EditModel : BasePageModel
    {
        private readonly ImageService _imageService;
        private readonly AppDbContext _dbContext;

        public EditModel(
            ImageService imageService,
            AppDbContext dbContext
        ) {
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [BindProperty]
        public string Description { get; set; }
        [BindProperty]
        public DateTime TakenAt { get; set; }

        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string TakenAtSourceMessage { get; set; }
        
        public async Task<IActionResult> OnGet(int id)
        {
            var image = await _dbContext.Images
                .Where(i => !i.IsDeleted && i.Id == id)
                .FirstOrDefaultAsync();
            
            if (image == null)
            {
                return NotFound();
            }

            ImageUrl = _imageService.GetImageUrl(image);
            Description = image.Description;
            Id = image.Id;
            TakenAt = image.TakenAt.DateTime;

            switch (image.TakenAtSource)
            {
                case (TakenAtSourceType.CurrentTime):
                    TakenAtSourceMessage = "The system was not able to determine when this image has taken, so has defaulted to the time it was uploaded.";
                    break;
                case (TakenAtSourceType.FromExif):
                    TakenAtSourceMessage = "The system was able to read when this image was taken.";
                    break;
                case (TakenAtSourceType.Manual):
                    TakenAtSourceMessage = "The date and time this image was taken has been manually set.";
                    break;
                default:
                    break;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUpdate(int id)
        {
            var image = await _dbContext.Images
                .Where(i => !i.IsDeleted && i.Id == id)
                .FirstOrDefaultAsync();
            
            if (image == null)
            {
                return NotFound();
            }

            var imageChanged = false;

            if (image.Description != Description)
            {
                image.Description = Description;
                imageChanged = true;
            }

            if (image.TakenAt.DateTime.Trim(DateTimeComponent.Second) != TakenAt.Trim(DateTimeComponent.Second))
            {
                image.TakenAt = TakenAt;
                image.TakenAtSource = TakenAtSourceType.Manual;
                imageChanged = true;
            }

            if (imageChanged)
            {
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToPage("./index", new { id = id });
        }
    }
}