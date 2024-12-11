namespace McDermott.TelemedicianApi.ViewModel
{
    public class ApiResponse<T>(int statusCode = StatusCodes.Status200OK, string message = "Ok", T data = default)
    {
        public int StatusCode { get; set; } = statusCode;
        public string Message { get; set; } = message;
        public T Data { get; set; } = data;
    }
}