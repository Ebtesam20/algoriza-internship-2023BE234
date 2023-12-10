namespace Vezeeta.API.Dto.DiscountCodeDtos
{
    public class DiscountCodeInputDto
    {
        public string DiscountCode { get; set; }
        public int NumOfCompletedRequest { get; set; }
        public string DiscountType { get; set; }
        public decimal value { get; set; }

    }
}
