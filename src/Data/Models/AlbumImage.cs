namespace Data.Models
{
    public class AlbumImage
    {
        public int AlbumId { get; set; }
        public Album Album { get; set; }
        public int ImageId { get; set; }
        public Image Image { get; set; }
    }
}