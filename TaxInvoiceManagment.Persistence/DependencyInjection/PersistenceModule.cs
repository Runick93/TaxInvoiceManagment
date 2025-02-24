using Microsoft.Extensions.DependencyInjection;
using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Persistence.DbContexts;
using TaxInvoiceManagment.Persistence.Repositories;

namespace TaxInvoiceManagment.Persistence.DependencyInjection
{
    public static class PersistenceModule
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddDbContext<TaxInvoiceManagmentDbContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITaxableItemRepository, TaxableItemRepository>();
            services.AddScoped<ITaxRepository, TaxRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();

            return services;
        }
    }
}
