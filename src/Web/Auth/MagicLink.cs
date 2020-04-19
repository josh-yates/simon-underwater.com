using System;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Web.Auth.MagicLink
{
    public class MagicLinkTokenProviderOptions : DataProtectionTokenProviderOptions
    {
        public MagicLinkTokenProviderOptions()
        {
            // TODO tie the lifespan to config
            Name = Constants.MagicLinkTokenProvider;
            TokenLifespan = TimeSpan.FromMinutes(15);
        }
    }

    public class MagicLinkTokenProvider<TUser> : DataProtectorTokenProvider<TUser>
        where TUser: class
    {
        public MagicLinkTokenProvider(
            IDataProtectionProvider dataProtectionProvider,
            IOptions<MagicLinkTokenProviderOptions> options,
            ILogger<MagicLinkTokenProvider<TUser>> logger) 
            : base(dataProtectionProvider, options, logger)
        {
        }
    }

    public static class CustomIdentityBuilderExtensions
    {
        public static IdentityBuilder AddMagicLinkTokenProvider(this IdentityBuilder builder)
        {
            var userType = builder.UserType;
            var provider= typeof(MagicLinkTokenProvider<>).MakeGenericType(userType);
            return builder.AddTokenProvider(Constants.MagicLinkTokenProvider, provider);
        }
    }
}