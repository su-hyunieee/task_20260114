using CoreServer.Api.Dto;
using CoreServer.Application.EmployeeDirectory;
using CoreServer.Domain.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreServer.Api.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly ReadEmployeeService _read;
        private readonly UpdateEmployeesService _update;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(ReadEmployeeService read, UpdateEmployeesService update, ILogger<EmployeeController> logger)
        {
            _read = read;
            _update = update;
            _logger = logger;
        }

        // GET /api/employee?page=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken ct = default)
        {
            var list = await _read.GetPagedAsync(page, pageSize, ct);
            return Ok(ApiResponse<List<Employee>>.Ok(list));
        }

        // GET /api/employee/{name}
        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(
            [FromRoute] string name,
            CancellationToken ct = default)
        {
            var employee = await _read.GetByNameAsync(name, ct);

            if (employee == null)
            {
                _logger.LogDebug("EmployeeController failed : name={Name} not found.", name);
                return NotFound(ApiResponse.Fail("NOT_FOUND", "employee not found"));
            }

            return Ok(ApiResponse<Employee>.Ok(employee));
        }

        // POST /api/employee?format=csv|json
        // Content-Type: text/plain or application/json
        [HttpPost]
        [Consumes("text/plain", "application/json")]
        public async Task<IActionResult> Update(
            [FromQuery] string format,
            CancellationToken ct = default)
        {
            using var reader = new StreamReader(Request.Body);
            var payload = await reader.ReadToEndAsync(ct);

            if (string.IsNullOrWhiteSpace(payload))
            {
                _logger.LogDebug("EmployeeController failed : request body is empty");
                return BadRequest(ApiResponse.Fail("INVALID_INPUT", "Request body is empty"));
            }

            var result = await _update.UpdateAsync(format, payload, ct);
            if (result.IsSuccess == false)
            {
                _logger.LogDebug("EmployeeController failed : status={StatusCode}, error={Error}", result.StatusCode, result.Error);

                return StatusCode(
                    result.StatusCode,
                    ApiResponse.Fail(
                        result.Error ?? "ERROR",
                        result.Message ?? "update failed"));
            }

            return Ok(ApiResponse<object>.Ok());
        }

        // POST /api/employee/upload?format=csv|json
        // multipart/form-data file 업로드
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateByUpload(
            [FromQuery] string format,
            [FromForm] IFormFile file,
            CancellationToken ct = default)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogDebug("EmployeeController failed : file is empty");
                return BadRequest(ApiResponse.Fail("INVALID_INPUT", "file is empty"));
            }

            using var reader = new StreamReader(file.OpenReadStream());
            var payload = await reader.ReadToEndAsync(ct);

            var result = await _update.UpdateAsync(format, payload, ct);
            if (result.IsSuccess == false)
            {
                _logger.LogDebug("EmployeeController failed : status={StatusCode}, error={Error}", result.StatusCode, result.Error);

                return StatusCode(
                    result.StatusCode,
                    ApiResponse.Fail(
                        result.Error ?? "ERROR",
                        result.Message ?? "update failed"));
            }

            return Ok(ApiResponse<object>.Ok());
        }
    }
}