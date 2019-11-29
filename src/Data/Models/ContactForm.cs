using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class ContactForm
    {
        public int Id { get; set; }
        public DateTimeOffset SentAt { get; set; }
        [Required]
        [StringLength(100)]
        public string EmailAddress { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(5000)]
        public string Content { get; set; }
        public bool IsSpam { get; set; }
    }
}