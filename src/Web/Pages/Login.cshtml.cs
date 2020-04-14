using System;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Pages.Shared;

namespace Web.Pages
{
    public class LoginModel : BasePageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public LoginModel(
            SignInManager<User> signInManager,
            UserManager<User> userManager
        )
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        // Page model
        [BindProperty]
        public string EmailAddress { get; set; }
        [BindProperty]
        public bool RememberMe { get; set; }

        public IActionResult OnGet()
        {
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                return RedirectToHome();
            }

            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.FindByEmailAsync(EmailAddress);

            // TODO finish this 😉

            return Page();
        }
    }
}