using CoreServer.Domain.Employees;

namespace CoreServer.Application.EmployeeDirectory
{
    public sealed class UpdateEmployeesService
    {
        private readonly IEmployeeRepository _repo;

        public UpdateEmployeesService(IEmployeeRepository repo)
        {
            _repo = repo;
        }

        public async Task<UpdateEmployeesResult> UpdateAsync(
            string? format,
            string payload,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(payload))
                return UpdateEmployeesResult.Fail(400, "INVALID_INPUT", "request body is empty");

            if (string.IsNullOrWhiteSpace(format))
                return UpdateEmployeesResult.Fail(400, "INVALID_FORMAT", "format query is required (csv|json)");

            format = format.Trim().ToLowerInvariant();

            List<Employee> list = format switch
            {
                "csv"  => EmployeeParser.FromCsv(payload),
                "json" => EmployeeParser.FromJson(payload),
                _      => new List<Employee>()
            };

            if (list.Count == 0)
            {
                return UpdateEmployeesResult.Fail(400, "PARSE_ERROR", "no employees parsed from payload");
            }

            if (format != "csv" && format != "json")
            {
                return UpdateEmployeesResult.Fail(400, "INVALID_FORMAT", "format must be csv or json");
            }

            // 이미 있는 email이면 실패
            foreach (var e in list)
            {
                if (string.IsNullOrWhiteSpace(e.Email))
                {
                    return UpdateEmployeesResult.Fail(400, "PARSE_ERROR", "email is missing in payload");
                }
                    
                if (await _repo.ExistsByEmailAsync(e.Email, ct))
                {
                    return UpdateEmployeesResult.Fail(409, "DUPLICATE_EMAIL", $"duplicate email: {e.Email}");
                }
            }

            await _repo.AddRangeAsync(list, ct);
            return UpdateEmployeesResult.Ok();
        }
    }
}