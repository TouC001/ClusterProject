namespace ClusterAPILibrary
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }

        public ApiResponse(T data, string message = "OK")
        {
            Success = true;
            Data = data;
            Message = message;
        }

        public ApiResponse(string errorMessage)
        {
            Success = false;
            Data = default;
            Message = errorMessage;
        }
    }
}
