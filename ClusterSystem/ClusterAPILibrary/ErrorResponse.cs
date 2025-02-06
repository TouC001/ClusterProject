namespace ClusterAPILibrary
{
    public class ErrorResponse
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public ErrorResponse(string code, string message)
        {
            ErrorCode = code;
            ErrorMessage = message;
        }
    }
}
