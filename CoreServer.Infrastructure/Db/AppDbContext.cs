using CoreServer.Domain.Employees;
using Microsoft.EntityFrameworkCore;

namespace CoreServer.Infrastructure.Db
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees => Set<Employee>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var e = modelBuilder.Entity<Employee>();

            //직원 디비 하나만 만듬
            e.ToTable("employee"); 
            e.HasKey(x => x.Id);

            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Name).HasColumnName("name").IsRequired();
            e.Property(x => x.Email).HasColumnName("email").IsRequired();
            e.Property(x => x.Tel).HasColumnName("tel").IsRequired();

            e.Property(x => x.Joined)
                .HasColumnName("joined")
                .HasConversion(
                    v => v.ToString("yyyy-MM-dd"),
                    v => DateOnly.Parse(v))
                .IsRequired();

            e.HasIndex(x => x.Email).IsUnique();
        }
    }
}

