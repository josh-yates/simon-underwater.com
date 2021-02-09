using SixLabors.ImageSharp.Processing.Transforms;
using Statiq.Common;
using Statiq.Core;
using Statiq.Images;

namespace Site.Pipelines
{
    public class LogoImagePipeline : Pipeline
    {
        public LogoImagePipeline()
        {
            InputModules = new ModuleList
            {
                new ReadFiles("assets/logo.jpg")
            };

            ProcessModules = new ModuleList
            {
                new MutateImage()
                    .Resize(480, 480, mode: ResizeMode.Max)
                    .OutputAsPng(),
                new SetDestination("assets/logo.png")
            };

            OutputModules = new ModuleList
            {
                new WriteFiles()
            };
        }
    }
}