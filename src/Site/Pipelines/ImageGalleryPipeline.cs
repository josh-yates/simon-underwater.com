using System.Linq;
using Site.Keys;
using Site.Models;
using Statiq.Common;
using Statiq.Core;
using Statiq.Razor;

namespace Site.Pipelines
{
    public class ImageGalleryPipeline : Pipeline
    {
        public ImageGalleryPipeline()
        {
            Dependencies.Add(nameof(ImagesPipeline));

            ProcessModules = new ModuleList
            {
                new ConcatDocuments(nameof(ImagesPipeline)),
                new SetContent(Config.FromValue(string.Empty)),
                new OrderDocuments(Config.FromDocument(d => d.GetDateTime(ImageDataKeys.TakenAt)))
                    .Descending(),
                new PaginateDocuments(10),
                new SetContent(Config.FromContext(async ctx =>
                    await ctx.FileSystem.GetInputFile("razor/gallery.cshtml").ReadAllTextAsync())),
                new RenderRazor()
                    .WithLayout("/razor/_Layout.cshtml")
                    .WithModel(Config.FromDocument(d =>
                    {
                        var index = d.GetInt(Statiq.Common.Keys.Index);
                        var totalPages = d.GetInt(Statiq.Common.Keys.TotalPages);

                        return new GalleryPage
                        {
                            Images = d.GetDocuments(Statiq.Common.Keys.Children)
                                .Select(d => new GalleryPage.Image
                                {
                                    Href = $"/{d.Source.FileNameWithoutExtension}",
                                    Src = $"/images/{d.Source.FileNameWithoutExtension}{d.Source.Extension}"
                                })
                                .ToList(),
                            Index = index,
                            NextHref = index >= totalPages ? null : $"/photos/{index + 1}",
                            PrevHref = index <= 1 ? null : $"/photos/{index - 1}"
                        };
                    })),
                new SetDestination(Config.FromDocument(d => new NormalizedPath($"photos/{d.GetInt(Statiq.Common.Keys.Index)}/index.html")))
            };

            OutputModules = new ModuleList
            {
                new WriteFiles()
            };
        }
    }
}