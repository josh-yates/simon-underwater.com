using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Auth;
using Web.Pages.Shared;
using Web.Services;

namespace Web.Pages.Photos
{
    public class UploadModel : BasePageModel
    {
        private readonly ImageService _imageService;
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _dbContext;
        
        public UploadModel(
            ImageService imageService,
            UserManager<User> userManager,
            AppDbContext dbContext
        )
        {
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [BindProperty]
        public ICollection<IFormFile> Files { get; set; }

        public List<string> ErrorMessages = new List<string>();

        public List<string> SuccessMessages = new List<string>();

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirst(Constants.EmailClaim).Value);

            if (user == null)
            {
                return Unauthorized();
            }
            
            // TODO: find user and save files. Then return success messages & redirect to current page

            var imageModels = await Task.WhenAll(Files
                .Where(f => 
                {
                    var errors = _imageService.ValidateImageUpload(f);
                    ErrorMessages.AddRange(errors.Select(e => $"{f.FileName} - {e}"));
                    return !errors.Any();
                })
                .Select(f => _imageService.SaveImageUploadAsync(f, user)));
            
            if (imageModels.Any())
            {
                 _dbContext.Images.AddRange(imageModels);
                  await _dbContext.SaveChangesAsync();
            }

            SuccessMessages.AddRange(imageModels.Select(i => $"{i.OriginalName} - Uploaded successfully"));

            return Page();
        }
    }
}