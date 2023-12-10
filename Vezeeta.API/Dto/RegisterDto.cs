using System.ComponentModel.DataAnnotations;

namespace Vezeeta.API.Dtos
{
    public class RegisterDto
    {
        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$",
         ErrorMessage = "Minimum eight characters, at least one uppercase letter, one lowercase letter and one number")]
        public string Password { get; set; }

        public string? Image { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public AccountType AccountType { get; set; }
    }
}
