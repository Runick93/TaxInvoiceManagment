using Microsoft.EntityFrameworkCore;
using TaxInvoiceManagment.Persistence.DbContexts;

namespace TaxInvoiceManagment.Application.Tests.Helpers
{
    public class DbContextHelper
    {
        public static TaxInvoiceManagmentDbContext CreateSQLiteInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<TaxInvoiceManagmentDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            var context = new TaxInvoiceManagmentDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            return context;
        }
    }
}
