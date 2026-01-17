namespace CoreServer.Api.Dto
{
    public class ApiResponse
    {
        public bool success { get; init; }
        public string? error { get; init; }
        public string? message { get; init; }

        public static ApiResponse Ok(string? message = null)
            => new() { success = true, message = message };

        public static ApiResponse Fail(string error, string message)
            => new() { success = false, error = error, message = message };
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T? data { get; init; }

        public static ApiResponse<T> Ok(T data, string? message = null)
            => new() { success = true, data = data, message = message };

        public new static ApiResponse<T> Fail(string error, string message)
            => new() { success = false, error = error, message = message };
    }
}

