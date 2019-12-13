using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }

        // TODO Remove these for now
        public string PasswordSalt { get; set; }
        public string PasswordResetToken { get; set; }
    }
}