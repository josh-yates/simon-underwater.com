using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
    }
}