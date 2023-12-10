namespace Vezeeta.API.Dto.DoctorDtos
{
    public class DoctorBookingDto
    {
        public string PatientName { get; set; }
        public string Image { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Day { get; set; }
        public TimeSpan Time { get; set; }
    }
}
