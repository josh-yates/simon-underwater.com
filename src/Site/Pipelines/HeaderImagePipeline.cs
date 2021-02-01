using Site.Keys;
using Site.Operations;
using Statiq.Common;
using Statiq.Core;
using Statiq.Images;

namespace Site.Pipelines
{
    public class HeaderImagePipeline : Pipeline
    {
        public HeaderImagePipeline()
        {
            Dependencies.Add(nameof(ImagesPipeline));
            ProcessModules = new ModuleList
            {
                new ConcatDocuments(nameof(ImagesPipeline)),
                new OrderDocuments(Config.FromDocument(d => d.GetDateTime(ImageDataKeys.TakenAt))),
                new TakeDocuments(1),
                new MutateImage()
                    .Operation(BlurOperation.Apply)
                    .OutputAsPng(),
                new SetDestination("assets/header-background.png")
            };

            OutputModules = new ModuleList
            {
                new WriteFiles()
            };
        }
    }
}