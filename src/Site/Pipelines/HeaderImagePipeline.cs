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
            InputModules = new ModuleList
            {
                new ReadFiles("assets/header-background.JPG")
            };

            ProcessModules = new ModuleList
            {
                new MutateImage()
                    .Operation(BlurOperation.Apply)
            };

            OutputModules = new ModuleList
            {
                new WriteFiles()
            };
        }
    }
}