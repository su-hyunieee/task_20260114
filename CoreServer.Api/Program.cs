using CoreServer.Application.EmployeeDirectory;
using CoreServer.Domain.Employees;
using CoreServer.Infrastructure.Db;
using CoreServer.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;

var bootstrapLogger = LogManager.GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();


    builder.Services.AddControllers();

    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseSqlite(
            builder.Configuration.GetConnectionString("Sqlite")
            ?? "Data Source=employee.db"));

    builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

    builder.Services.AddScoped<ReadEmployeeService>();
    builder.Services.AddScoped<UpdateEmployeesService>();

    builder.Services.AddOpenApi();
    
    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();
    }

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();
    app.MapControllers();

    app.Logger.LogInformation("********** CoreServer start !! **********");
    app.Run();
}
catch (Exception ex)
{
    bootstrapLogger.Error(ex, "CoreServer stopped unexpectedly.");
    throw;
}
finally
{
    LogManager.Shutdown();
}