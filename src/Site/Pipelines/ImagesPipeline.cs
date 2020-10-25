using System.Linq;
using System.Threading.Tasks;
using Site.Modules;
using Site.Operations;
using Statiq.Common;
using Statiq.Core;
using Statiq.Images;

namespace Site.Pipelines
{
    public class ImagesPipeline : Pipeline
    {
        public ImagesPipeline()
        {
            var exts = new[] { ".JPG", ".JPEG", ".PNG", ".TIFF" };
            InputModules = new ModuleList
            {
                new ReadFiles("*")
                    .Where(x => Task.FromResult(exts.Contains(x.Path.Extension.ToUpper())))
            };

            ProcessModules = new ModuleList
            {
                new ReadImageData(),
                new MutateImage()
                    .Operation(WatermarkOperation.Apply)
                    .Operation(CustomResizeOperation.Apply),
                new OrderDocuments(Config.FromDocument(d => d.GetDateTime(ImageDataKeys.TakenAt)))
                    .Descending()
            };

            OutputModules = new ModuleList
            {
                new WriteFiles()
            };
        }
    }
}