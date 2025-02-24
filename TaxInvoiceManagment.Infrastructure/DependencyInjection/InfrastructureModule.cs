using Microsoft.Extensions.DependencyInjection;
using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Infrastructure.FileStorage;
using TaxInvoiceManagment.Infrastructure.Logging;
namespace TaxInvoiceManagment.Infrastructure.DependencyInjection
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IFileSystemService, FileSystemService>();
            services.AddScoped<ILoggingService, SerilogLoggingService>();
            return services;
        }
    }
}
