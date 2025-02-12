using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TaxInvoiceManagment.Persistence;
using TaxInvoiceManagment.Application.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TaxInvoiceManagmentDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TaxableItemValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TaxOrServiceValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<InvoiceValidator>();


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
        // Aqu� puedes registrar el error usando Serilog o alg�n logger configurado
        Console.WriteLine($"Error applying migrations: {ex.Message}");
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
