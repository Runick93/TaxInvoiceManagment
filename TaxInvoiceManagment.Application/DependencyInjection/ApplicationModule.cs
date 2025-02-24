using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TaxInvoiceManagment.Application.Interfaces;
using TaxInvoiceManagment.Application.Services;

namespace TaxInvoiceManagment.Application.DependencyInjection
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Services
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<ITaxService, TaxService>();
            services.AddScoped<ITaxableItemService, TaxableItemService>();
            services.AddScoped<IUserService, UserService>();

            // Validators
            //services.AddFluentValidationAutoValidation();
            //services.AddValidatorsFromAssemblyContaining<UserDtoValidator>();
            //services.AddValidatorsFromAssemblyContaining<TaxableItemDtoValidator>();
            //services.AddValidatorsFromAssemblyContaining<TaxDtoValidator>();
            //services.AddValidatorsFromAssemblyContaining<InvoiceDtoValidator>();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Automapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}
