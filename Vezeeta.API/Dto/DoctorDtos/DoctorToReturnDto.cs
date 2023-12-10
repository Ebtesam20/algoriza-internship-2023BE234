namespace Vezeeta.API.Dto.DoctorDtos
{
    public class DoctorToReturnDto
    {
        public string FullName { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int SpecializationId { get; set; }
        public string Specialization { get; set; }


    }
}
