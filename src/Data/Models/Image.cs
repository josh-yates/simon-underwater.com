using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Image
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        [Required]
        public string OnDiskName { get; set; }
        [Required]
        public string OriginalName { get; set; }
        public int Size { get; set; }
        public DateTimeOffset TakenAt { get; set; }
        public bool IsDeleted { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public DateTimeOffset UploadedAt { get; set; }
        public ICollection<AlbumImage> ImageAlbums { get; set; }
    }
}