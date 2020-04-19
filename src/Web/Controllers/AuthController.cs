using System;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Auth;

namespace Web.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AuthController(
            SignInManager<User> signInManager,
            UserManager<User> userManager
        )
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpGet("/auth/login")]
        public async Task<IActionResult> Login([FromQuery] string token, [FromQuery] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (await _userManager.VerifyUserTokenAsync(user, Constants.MagicLinkTokenProvider, Constants.MagicLinkTokenPurpose, token))
            {
                await _signInManager.SignInAsync(user, true);

                return LocalRedirect("~/");
            }

            return LocalRedirect("~/login");
        }

        [HttpPost("/auth/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return LocalRedirect("~/login");
        }
    }
}