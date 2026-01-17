namespace CoreServer.Application.EmployeeDirectory
{
    public sealed record UpdateEmployeesResult(
        bool IsSuccess,
        int StatusCode,
        string? Error,
        string? Message
    )
    {
        public static UpdateEmployeesResult Ok(string? message = null)
            => new(true, 200, null, message);

        public static UpdateEmployeesResult Fail(int statusCode, string error, string? message = null)
            => new(false, statusCode, error, message);
    }
}