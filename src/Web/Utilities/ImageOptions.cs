namespace Web.Utilities
{
    public class ImageOptions
    {
        public string UploadsBaseDirectory { get; set; }
        public string WebImagesBaseDirectory { get; set; }
        public string ThumbnailsBaseDirectory { get; set; }
        public double WebImageResizeFactor { get; set; }
        public double ThumbnailResizeFactor { get; set; }
    }
}