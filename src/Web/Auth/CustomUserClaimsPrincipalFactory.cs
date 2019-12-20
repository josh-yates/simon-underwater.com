using System.Security.Claims;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Web.Auth
{
    public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User>
    {
        public CustomUserClaimsPrincipalFactory(
            UserManager<User> userManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, optionsAccessor) {}

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            identity.AddClaims(new Claim[]
            {
                new Claim(Constants.FirstNameClaim, user.FirstName),
                new Claim(Constants.LastNameClaim, user.LastName),
                new Claim(Constants.EmailClaim, user.EmailAddress)
            });

            return identity;
        }
    }
}