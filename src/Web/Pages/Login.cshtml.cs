using System;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Auth;
using Web.Pages.Shared;

namespace Web.Pages
{
    public class LoginModel : BasePageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            ILogger<LoginModel> logger
        )
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

            if (user != null) {
                var token = await _userManager.GenerateUserTokenAsync(user, Constants.MagicLinkTokenProvider, Constants.MagicLinkTokenPurpose);
                var url = Url.Action("Login", "Auth", new { token = token, email = user.EmailAddress, rememberMe = RememberMe });

                _logger.LogInformation(url);
            }

            return Page();
        }
    }
}