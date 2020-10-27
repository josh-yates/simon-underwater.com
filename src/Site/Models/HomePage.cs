using System.Collections.Generic;

namespace Site.Models
{
    public class HomePage
    {
        public List<Image> Images { get; set; }
        public class Image
        {
            public string Src { get; set; }
            public string Href { get; set; }
        }
    }
}