using System.Collections.Generic;

namespace Data.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<AlbumImage> AlbumImages { get; set; }
    }
}