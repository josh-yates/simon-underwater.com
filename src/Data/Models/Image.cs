using System;
using System.Collections.Generic;
using Data.Enums;

namespace Data.Models
{
    public class Image
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string OnDiskName { get; set; }
        public string OriginalName { get; set; }
        public int Size { get; set; }
        public DateTimeOffset TakenAt { get; set; }
        public TakenAtSourceType TakenAtSource { get; set; }
        public bool IsDeleted { get; set; }
        public string Description { get; set; }
        public DateTimeOffset UploadedAt { get; set; }
        public ICollection<AlbumImage> ImageAlbums { get; set; }
    }
}