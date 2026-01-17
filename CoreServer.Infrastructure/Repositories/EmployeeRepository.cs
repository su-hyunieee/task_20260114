using CoreServer.Domain.Employees;
using CoreServer.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace CoreServer.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _db;
        public EmployeeRepository(AppDbContext db) => _db = db;

        public Task<List<Employee>> GetPagedAsync(int page, int pageSize, CancellationToken ct)
            => _db.Employees.AsNoTracking()
                .OrderBy(e => e.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

        public Task<Employee?> GetByNameAsync(string name, CancellationToken ct)
            => _db.Employees.AsNoTracking()
                .FirstOrDefaultAsync(e => e.Name == name, ct);

        public Task<bool> ExistsByEmailAsync(string email, CancellationToken ct)
            => _db.Employees.AnyAsync(e => e.Email == email, ct);

        public async Task AddRangeAsync(IEnumerable<Employee> employees, CancellationToken ct)
        {
            await _db.Employees.AddRangeAsync(employees, ct);
            await _db.SaveChangesAsync(ct);
        }
    }
}

