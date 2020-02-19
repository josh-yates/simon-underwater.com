using System.Collections.Generic;

namespace Web.Utilities
{
    public class ImageOptions
    {
        public double WebImageResizeFactor { get; set; }
        public double ThumbnailResizeFactor { get; set; }
        public ICollection<string> PermittedExtensions { get; set; }
        public string WebImagesBaseUrl { get; set; }
        public string ThumbnailsBaseUrl { get; set; }
    }
}