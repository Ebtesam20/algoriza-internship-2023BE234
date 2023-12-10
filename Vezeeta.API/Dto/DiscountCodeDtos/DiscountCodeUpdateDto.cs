namespace Vezeeta.API.Dto.DiscountCodeDtos
{
    public class DiscountCodeUpdateDto
    {
        public int Id { get; set; }
        public string DiscountCode { get; set; }
        public int NumOfCompletedRequest { get; set; }
        public string DiscountType { get; set; }
        public decimal value { get; set; }
       
    }
}
