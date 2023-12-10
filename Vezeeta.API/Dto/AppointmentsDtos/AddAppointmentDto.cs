namespace Vezeeta.API.Dto.AppointmentsDtos
{
    public class AddAppointmentDto
    {
        public decimal Price { get; set; }
        public List<DayWithTimesDto> Days { get; set; }

    }
}
