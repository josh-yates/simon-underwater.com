using Site.Keys;
using Site.Models;
using Statiq.Common;
using Statiq.Core;
using Statiq.Razor;

namespace Site.Pipelines
{
    public class ImagePagesPipeline : Pipeline
    {
        public ImagePagesPipeline()
        {
            Dependencies.Add(nameof(ImagesPipeline));
            ProcessModules = new ModuleList
            {
                new ConcatDocuments(nameof(ImagesPipeline)),
                new SetContent(Config.FromContext(async ctx =>
                    await ctx.FileSystem.GetInputFile("image.cshtml").ReadAllTextAsync())),
                new RenderRazor()
                    .WithModel(Config.FromDocument(d => new ImagePage
                    {
                        Src = d.Destination.ToString(),
                        TakenAt = d.GetDateTime(ImageDataKeys.TakenAt)
                    })),
                new SetDestination(".html")
            };

            OutputModules = new ModuleList
            {
                new WriteFiles()
            };
        }
    }
}