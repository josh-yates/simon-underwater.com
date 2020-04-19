using System;
using System.Net;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Web.Auth.MagicLink
{
    public class MagicLinkService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<User> _userManager;
        public MagicLinkService(
            IHttpContextAccessor contextAccessor,
            UserManager<User> userManager
        )
        {
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<string> GetMagicLinkForUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var token = await _userManager.GenerateUserTokenAsync(user, Constants.MagicLinkTokenProvider, Constants.MagicLinkTokenPurpose);
            var request = _contextAccessor.HttpContext.Request;

            return $"{request.Scheme}://{request.Host.Value}/auth/login?token={WebUtility.UrlEncode(token)}&email={WebUtility.UrlEncode(user.EmailAddress)}";
        }
    }
}