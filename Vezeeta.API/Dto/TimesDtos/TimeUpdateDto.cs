namespace Vezeeta.API.Dto.TimesDtos
{
    public class TimeUpdateDto
    {
        public int Id { get; set; }
        public string Day { get; set; }
        public TimeSpan Time { get; set; }
    }
}
