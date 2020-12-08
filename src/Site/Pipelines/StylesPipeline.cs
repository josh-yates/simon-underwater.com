using System;
using Site.Keys;
using Statiq.Common;
using Statiq.Core;
using Statiq.Sass;

namespace Site.Pipelines
{
    public class StylesPipeline : Pipeline
    {
        public StylesPipeline()
        {
            InputModules = new ModuleList
            {
                new ReadFiles("styles/Main.scss")
            };

            ProcessModules = new ModuleList
            {
                new CompileSass()
                    .WithCompactOutputStyle()
                    .IncludeSourceComments(false),
                new SetDestination($"styles/main.{RazorKeys.CssBuildId}.css")
            };

            OutputModules = new ModuleList
            {
                new WriteFiles()
            };
        }
    }
}