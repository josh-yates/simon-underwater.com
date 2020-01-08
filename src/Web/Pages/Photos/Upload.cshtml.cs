using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Pages.Shared;
using Web.Services;

namespace Web.Pages.Photos
{
    public class UploadModel : BasePageModel
    {
        private readonly ImageService _imageService;
        public UploadModel(
            ImageService imageService
        )
        {
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
        }

        [BindProperty]
        public ICollection<IFormFile> Files { get; set; }

        public List<string> ErrorMessages = new List<string>();

        public async Task<IActionResult> OnPostAsync()
        {
            foreach (var file in Files)
            {
                var errors = _imageService.ValidateImageUpload(file);

                if (errors.Any())
                {
                    ErrorMessages.AddRange(errors.Select(e => $"{file.FileName} - {e}"));
                    continue;
                }

                // TODO: find user and save files. Then return success messages & redirect to current page
            }

            return Page();
        }
    }
}