using System.Linq;
using Site.Keys;
using Site.Models;
using Statiq.Common;
using Statiq.Core;
using Statiq.Razor;

namespace Site.Pipelines
{
    public class HomePagePipeline : Pipeline
    {
        public HomePagePipeline()
        {
            Dependencies.Add(nameof(ImagesPipeline));

            InputModules = new ModuleList
            {
                new ReadFiles("razor/home.cshtml")
            };

            ProcessModules = new ModuleList
            {
                new RenderRazor()
                    .WithLayout("/razor/_Layout.cshtml")
                    .WithModel(Config.FromContext(ctx => new HomePage
                    {
                        Images = ctx
                            .Outputs
                            .FromPipeline(nameof(ImagesPipeline))
                            .Select(d => new
                            {
                                TakenAt = d.GetDateTime(ImageDataKeys.TakenAt),
                                FileName = d.Source.FileNameWithoutExtension,
                                ImgExtension = d.Source.Extension
                            })
                            .OrderByDescending(d => d.TakenAt)
                            .Take(10)
                            .Select(i => new HomePage.Image
                            {
                                Href = $"/{i.FileName}.html",
                                Src = $"/images/{i.FileName}{i.ImgExtension}"
                            })
                            .ToList()
                    })),
                new SetDestination("index.html")
            };

            OutputModules = new ModuleList
            {
                new WriteFiles()
            };
        }
    }
}