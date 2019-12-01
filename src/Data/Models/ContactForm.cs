using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class ContactForm
    {
        public int Id { get; set; }
        public DateTimeOffset SentAt { get; set; }
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public bool IsSpam { get; set; }
    }
}