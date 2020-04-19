using System;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Auth;
using Web.Auth.MagicLink;
using Web.Pages.Shared;

namespace Web.Pages
{
    public class LoginModel : BasePageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly MagicLinkService _magicLinkService;

        public LoginModel(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            ILogger<LoginModel> logger,
            MagicLinkService magicLinkService
        )
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _magicLinkService = magicLinkService ?? throw new ArgumentNullException(nameof(magicLinkService));
        }

        // Page model
        [BindProperty]
        public string EmailAddress { get; set; }

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
                var magicLink = await _magicLinkService.GetMagicLinkForUser(user);
                _logger.LogInformation(magicLink);
            }

            return Page();
        }
    }
}