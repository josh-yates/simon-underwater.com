using Statiq.Common;
using Statiq.Core;
using Statiq.Razor;

namespace Site.Pipelines
{
    public class PassivePagePipeline : Pipeline
    {
        public PassivePagePipeline()
        {
            InputModules = new ModuleList
            {
                new ReadFiles(
                    "razor/about.cshtml",
                    "razor/contact.cshtml",
                    "razor/privacy.cshtml")
            };

            ProcessModules = new ModuleList
            {
                new RenderRazor()
                    .WithLayout("/razor/_Layout.cshtml"),
                new SetDestination(Config.FromDocument(d => new NormalizedPath($"{d.Destination.FileNameWithoutExtension}/index.html")))
            };

            OutputModules = new ModuleList
            {
                new WriteFiles()
            };
        }
    }
}