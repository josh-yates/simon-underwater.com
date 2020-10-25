using Site.Modules;
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

            InputModules = new ModuleList
            {
                new ReadFiles("image.cshtml")
            };

            ProcessModules = new ModuleList
            {
                new MergeMetadata(nameof(ImagesPipeline)),
                new SetDestination(".html"),
                new RenderRazor()
                    .WithModel(Config.FromDocument(d => new
                    {
                        src = d.GetString(ImageDataKeys.Destination),
                        takenAt = d.GetDateTime(ImageDataKeys.TakenAt)
                    }))
                    .WithLayout("layout.cshmtl")
            };

            OutputModules = new ModuleList
            {
                new WriteFiles()
            };
        }
    }
}