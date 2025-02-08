using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Persistence.Repositories;

namespace TaxInvoiceManagment.Persistence.Managers
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TaxInvoiceManagmentDbContext _context;

        public IUserRepository Users { get; }
        public IAssetRepository Assets { get; }
        public ITaxOrServiceRepository TaxesOrServices { get; }
        public IInvoiceRepository Invoices { get; }

        public UnitOfWork(TaxInvoiceManagmentDbContext context)
        {
            _context = context;
            Users = new UserRepository(context);
            Assets = new AssetRepository(context);
            TaxesOrServices = new TaxOrServiceRepository(context);
            Invoices = new InvoiceRepository(context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
