using System;
using Data.Enums;

namespace Data.Models
{
    public class Log
    {
        public int Id { get; set; }
        public LogType Type { get; set; }
        public string Description { get; set; }
        public DateTimeOffset LoggedAt { get; set; }

        // Optional log relationships
        public User User { get; set; }
        public int? UserId { get; set; }
        public Image Image { get; set; }
        public int? ImageId { get; set; }
        public Album Album { get; set; }
        public int? AlbumId { get; set; }
        public ContactForm ContactForm { get; set; }
        public int? ContactFormId { get; set; }
        public CustomContent CustomContent { get; set; }
        public int? CustomContentId { get; set; }
    }
}