using System;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Web.Auth.MagicLink
{
    public class PasswordlessLoginTokenProviderOptions : DataProtectionTokenProviderOptions
    {
        public PasswordlessLoginTokenProviderOptions()
        {
            Name = "PasswordlessLoginTokenProvider";
            TokenLifespan = TimeSpan.FromMinutes(15);
        }
    }

    public class PasswordlessLoginTokenProvider<TUser> : DataProtectorTokenProvider<TUser>
        where TUser: class
    {
        public PasswordlessLoginTokenProvider(
            IDataProtectionProvider dataProtectionProvider,
            IOptions<PasswordlessLoginTokenProviderOptions> options,
            ILogger<PasswordlessLoginTokenProvider<TUser>> logger) 
            : base(dataProtectionProvider, options, logger)
        {
        }
    }

    public static class CustomIdentityBuilderExtensions
    {
        public static IdentityBuilder AddPasswordlessLoginTokenProvider(this IdentityBuilder builder)
        {
            var userType = builder.UserType;
            var provider= typeof(PasswordlessLoginTokenProvider<>).MakeGenericType(userType);
            return builder.AddTokenProvider("PasswordlessLoginProvider", provider);
        }
    }


}