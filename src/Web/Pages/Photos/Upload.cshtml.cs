using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Pages.Shared;

namespace Web.Pages.Photos
{
    public class UploadModel : BasePageModel
    {
        [BindProperty]
        public ICollection<IFormFile> Files { get; set; }

        public IActionResult OnPostAsync()
        {
            return Page();
        }
    }
}