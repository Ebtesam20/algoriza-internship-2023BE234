namespace Vezeeta.API.Dto
{
    public class NumberOfRequestsDto
    {
        public int TotalNumberOfRequests { get; set; }
        public int NumberOfPendingRequests { get; set; }
        public int NumberOfCompletedRequests { get; set; }
        public int NumberOfCanceledRequests { get;set; }
    }
}
