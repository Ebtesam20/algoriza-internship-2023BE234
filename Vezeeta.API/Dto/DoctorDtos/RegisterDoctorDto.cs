using System.ComponentModel.DataAnnotations;

namespace Vezeeta.API.Dto.DoctorDtos
{
    public class RegisterDoctorDto
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
        [Required]
        public IFormFile ImageUrl { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Specialization { get; set; }




    }
}
