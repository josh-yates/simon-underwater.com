using System.Collections.Generic;

namespace Site.Models
{
    public class GalleryPage
    {
        public string PrevHref { get; set; }
        public string NextHref { get; set; }
        public int Index { get; set; }
        public List<Image> Images { get; set; }
        public class Image
        {
            public string Src { get; set; }
            public string Href { get; set; }
        }
    }
}