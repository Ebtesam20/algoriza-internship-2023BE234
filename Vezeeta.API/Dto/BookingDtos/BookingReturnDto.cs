namespace Vezeeta.API.Dto.BookingDtos
{
    public class BookingReturnDto
    {
        public string DoctorName { get; set; }
        public string Image { get; set; }
        public string Specialization { get; set; }
        public string Day { get; set; }
        public TimeSpan Time { get; set; }
        public decimal Price { get; set; }
        public string? DiscountCode { get; set; }
        public decimal FinalPrice { get; set; }
        public string Status { get; set; }


    }
}
