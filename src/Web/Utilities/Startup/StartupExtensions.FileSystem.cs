using System.IO;
using Core.FileSystem;
using Enable.Extensions.FileSystem;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Web.Utilities.Startup
{
    public static partial class StartupExtensions
    {
        public static void AddAppFilesystemHub(this IServiceCollection services)
        {
            var filesystemHub = new BasicFileSystemHub();

            services.AddSingleton<IFileSystemHub>(filesystemHub);
        }

        public static void ConfigureFileSystemHub(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            var options = app
                .ApplicationServices
                .GetRequiredService<IOptions<FileSystemOptions>>()
                .Value;

            var uploadsDirectory = Path.Combine(env.ContentRootPath, options.UploadsBaseDirectory);
            var thumbnailsDirectory = Path.Combine(env.WebRootPath, options.ThumbnailsBaseDirectory);
            var webVersionDirectory = Path.Combine(env.WebRootPath, options.WebImagesBaseDirectory);

            var hub = app.ApplicationServices.GetRequiredService<IFileSystemHub>();

            hub.Add(FileSystemKeys.Uploads, new FileSystem(uploadsDirectory));
            hub.Add(FileSystemKeys.Thumbnails, new FileSystem(thumbnailsDirectory));
            hub.Add(FileSystemKeys.WebImages, new FileSystem(webVersionDirectory));
        }
    }
}