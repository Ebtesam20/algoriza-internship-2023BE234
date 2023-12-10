namespace Vezeeta.API.Dto.BookingDtos
{
    public class AppointmentTimeToreturnDto
    {

        public string Day { get; set; }

        public List<DoctorTimeToReturnDto> Times { get; set; }
    }
}
