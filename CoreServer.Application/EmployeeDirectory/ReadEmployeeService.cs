using CoreServer.Domain.Employees;

namespace CoreServer.Application.EmployeeDirectory
{
    public class ReadEmployeeService
    {
        private readonly IEmployeeRepository _repo;
        public ReadEmployeeService(IEmployeeRepository repo) => _repo = repo;

        public Task<List<Employee>> GetPagedAsync(int page, int pageSize, CancellationToken ct)
            => _repo.GetPagedAsync(page, pageSize, ct);

        public Task<Employee?> GetByNameAsync(string name, CancellationToken ct)
            => _repo.GetByNameAsync(name, ct);
    }
}
