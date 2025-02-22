using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TaxInvoiceManagment.Application.Mappers;
using TaxInvoiceManagment.Application.Interfaces;
using TaxInvoiceManagment.Application.Managers;
using TaxInvoiceManagment.Application.Validators;
using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Entities;
using TaxInvoiceManagment.Persistence;
using TaxInvoiceManagment.Persistence.Managers;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = new ConfigurationBuilder()
                                            .SetBasePath(Directory.GetCurrentDirectory())
                                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                            .Build();
Appsettings appSettings = new();
configuration.Bind("Appsettings", appSettings);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .WriteTo.File(
    path: configuration.GetValue<string>("Serilog:Path"),
    restrictedToMinimumLevel: Enum.Parse<Serilog.Events.LogEventLevel>(configuration.GetValue<string>("Serilog:Level")),
    rollingInterval: RollingInterval.Day,
    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}"
    )
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);

// Add services to the container.
builder.Services.AddDbContext<TaxInvoiceManagmentDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Automapper
builder.Services.AddAutoMapper(typeof(UserMappingProfile));
builder.Services.AddAutoMapper(typeof(TaxableItemMappingProfile));
builder.Services.AddAutoMapper(typeof(TaxMappingProfile));
builder.Services.AddAutoMapper(typeof(InvoiceMappingProfile));

// Managers
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaxableItemService, TaxableItemService>();
builder.Services.AddScoped<ITaxService, TaxService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();

// Validations
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<UserDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TaxableItemDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TaxDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<InvoiceDtoValidator>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<TaxInvoiceManagmentDbContext>();
        context.Database.Migrate(); // O EnsureCreated()
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error applying migrations: {ex.Message}");
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
