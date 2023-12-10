﻿using System.ComponentModel.DataAnnotations;

namespace Vezeeta.API.Dto.DoctorDtos
{
    public class DoctorUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public IFormFile ImageUrl { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Specialization { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}
