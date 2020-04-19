using System;
using Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Web.Auth;
using Web.Auth.MagicLink;
using Web.Services;
using Web.Utilities;
using Web.Utilities.Startup;

namespace Web
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                // Global - in source control
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                // Environment - in source control
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                // Environment secrets - out of source control
                .AddJsonFile($"appsettings.secrets.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAppDbContext(Configuration.GetSection("Database"));

            services.AddIdentityCore<User>(options => 
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            })
            .AddUserStore<UserStore>()
            .AddSignInManager<SignInManager<User>>()
            .AddDefaultTokenProviders()
            .AddMagicLinkTokenProvider();

            services.AddScoped<IUserClaimsPrincipalFactory<User>, CustomUserClaimsPrincipalFactory>();

            services.AddAuthentication(IdentityConstants.ApplicationScheme)
            .AddCookie(IdentityConstants.ApplicationScheme, options =>
            {
                options.LoginPath = "/Login";
                options.AccessDeniedPath = "/Error";
                options.SlidingExpiration = true;
            });

            services.AddRouting(options =>
            {
                options.LowercaseQueryStrings = true;
                options.LowercaseUrls = true;
            });

            services.AddMvc();
            services.AddRazorPages()
            .AddRazorPagesOptions(options =>
            {
                // TODO consider whitelisting anonymous routes
                options.Conventions.AuthorizeFolder("/dashboard");
                options.Conventions.AuthorizePage("/photos/upload");
                options.Conventions.AuthorizePage("/albums/add");
                options.Conventions.AuthorizePage("/contact/requests");
                options.Conventions.AuthorizePage("/contact/request");
                options.Conventions.AuthorizePage("/about/edit");
                options.Conventions.AuthorizeFolder("/photo");
                options.Conventions.AllowAnonymousToPage("/photo/index");
                options.Conventions.AuthorizeFolder("/album");
                options.Conventions.AllowAnonymousToPage("/album/index");
            });

            services.AddHttpContextAccessor();
            services.Configure<ImageOptions>(Configuration.GetSection("Images"));
            services.Configure<FileSystemOptions>(Configuration.GetSection("FileSystem"));
            services.AddSingleton<ImageService>();
            services.AddScoped<MagicLinkService>();
            services.AddAppFilesystemHub();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.MigrateDatabase();
            app.ConfigureFileSystemHub(env);
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
