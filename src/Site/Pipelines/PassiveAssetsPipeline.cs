using Statiq.Common;
using Statiq.Core;

namespace Site.Pipelines
{
    public class PassiveAssetsPipeline : Pipeline
    {
        public PassiveAssetsPipeline()
        {
            InputModules = new ModuleList
            {
                new ReadFiles("assets/**/{*,!logo}.*")
            };

            ProcessModules = new ModuleList
            {
                new SetDestination(Config.FromDocument(d => new NormalizedPath($"{d.Destination.FileName}")))
            };

            OutputModules = new ModuleList
            {
                new WriteFiles()
            };
        }
    }
}