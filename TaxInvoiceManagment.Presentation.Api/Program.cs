using Microsoft.EntityFrameworkCore;
using Serilog;
using TaxInvoiceManagment.Application.DependencyInjection;
using TaxInvoiceManagment.Infrastructure.DependencyInjection;
using TaxInvoiceManagment.Persistence.DbContexts;
using TaxInvoiceManagment.Persistence.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// ==============================
// 🔹 Configuration
// ==============================
IConfiguration configuration = builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// Ensure directories exist
void EnsureDirectoryExists(string? filePath)
{
    if (!string.IsNullOrWhiteSpace(filePath))
    {
        string? directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
}

string dbPath = configuration.GetConnectionString("DefaultConnection") ?? "";
string logPath = configuration.GetValue<string>("Serilog:Path") ?? "";
EnsureDirectoryExists(dbPath);
EnsureDirectoryExists(logPath);

// ==============================
// 🔹 Logger Configuration (Serilog)
// ==============================
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .WriteTo.File(
        path: logPath,
        restrictedToMinimumLevel: Enum.Parse<Serilog.Events.LogEventLevel>(
            configuration.GetValue<string>("Serilog:Level") ?? "Information"
        ),
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}"
    )
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);

// ==============================
// 🔹 Services Registration
// ==============================
builder.Services.AddDbContext<TaxInvoiceManagmentDbContext>(options =>
    options.UseSqlite(dbPath));

builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddPersistence();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ==============================
// 🔹 Database Migration
// ==============================
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<TaxInvoiceManagmentDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        Log.Error($"Error applying migrations: {ex.Message}");
    }
}

// ==============================
// 🔹 Middleware Configuration
// ==============================
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseSerilogRequestLogging();
app.MapControllers();

app.Run();
