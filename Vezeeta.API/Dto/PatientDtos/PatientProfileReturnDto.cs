using Vezeeta.API.Dto.BookingDtos;

namespace Vezeeta.API.Dto.PatientDtos
{
    public class PatientProfileReturnDto
    {
        public PatientToReturnDto Details { get; set; }
        public List<BookingReturnDto> Requests { get; set; }
    }
}
