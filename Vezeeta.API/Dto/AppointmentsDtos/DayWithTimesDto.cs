using Vezeeta.API.Dto.TimesDtos;

namespace Vezeeta.API.Dto.AppointmentsDtos
{
    public class DayWithTimesDto
    {
        public string Day { get; set; }
        public List<TimeSlotDto> Times { get; set; }

    }
}
