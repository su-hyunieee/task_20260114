namespace CoreServer.Domain.Employees
{
    public interface IEmployeeRepository
    {
        // read
        Task<List<Employee>> GetPagedAsync(int page, int pageSize, CancellationToken ct);
        Task<Employee?> GetByNameAsync(string name, CancellationToken ct);

        // write
        Task<bool> ExistsByEmailAsync(string email, CancellationToken ct);
        Task AddRangeAsync(IEnumerable<Employee> employees, CancellationToken ct);
    }
}
