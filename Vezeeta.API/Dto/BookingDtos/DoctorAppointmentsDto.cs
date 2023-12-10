namespace Vezeeta.API.Dto.BookingDtos
{
    public class DoctorAppointmentsDto
    {

        public string Image { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Specialize { get; set; }
        public decimal Price { get; set; }
        public string Gender { get; set; }
        public List<AppointmentTimeToreturnDto> Appointments { get; set; }
    }
}
