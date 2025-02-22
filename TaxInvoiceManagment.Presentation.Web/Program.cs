using AntDesign.ProLayout;
using Microsoft.AspNetCore.Components;
using TaxInvoiceManagment.Application.Interfaces;
using TaxInvoiceManagment.Application.Managers;
using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Persistence;
using TaxInvoiceManagment.Persistence.Managers;
using TaxInvoiceManagment.Presentation.Web.Services;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using FluentValidation;
using TaxInvoiceManagment.Application.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAntDesign();
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(sp.GetService<NavigationManager>()!.BaseUri)
});
builder.Services.Configure<ProSettings>(builder.Configuration.GetSection("ProSettings"));
builder.Services.AddInteractiveStringLocalizer();
builder.Services.AddLocalization();

builder.Services.AddScoped<IChartService, ChartService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<TaxInvoiceManagment.Presentation.Web.Services.IUserService, TaxInvoiceManagment.Presentation.Web.Services.UserService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IProfileService, ProfileService>();

builder.Services.AddDbContext<TaxInvoiceManagmentDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<TaxInvoiceManagment.Application.Interfaces.IUserService, TaxInvoiceManagment.Application.Managers.UserService>();
builder.Services.AddScoped<ITaxableItemService, TaxableItemService>();
builder.Services.AddScoped<ITaxService, TaxService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<UserDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TaxableItemDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TaxDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<InvoiceDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UserDtoValidator>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<TaxInvoiceManagmentDbContext>();
        context.Database.Migrate(); // O EnsureCreated()
    }
    catch (Exception ex)
    {
        // Aquí puedes registrar el error usando Serilog o algún logger configurado
        Console.WriteLine($"Error applying migrations: {ex.Message}");
    }
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();