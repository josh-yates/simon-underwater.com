using System.ComponentModel.DataAnnotations;
using Data.Enums;

namespace Data.Models
{
    public class CustomContent
    {
        public int Id { get; set; }
        public CustomContentType CustomContentType { get; set; }
        [Required]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}