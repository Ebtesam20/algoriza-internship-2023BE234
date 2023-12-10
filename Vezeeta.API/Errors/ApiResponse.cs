namespace Vezeeta.API.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageByStatusCode(statusCode);
        }

        private string? GetDefaultMessageByStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request, you have made",
                401 => "Authorized, you are not",
                404 => "Resource is not found",
                500 => "Internal Server Error",
                _ => null,
            };
        }
    }
}
