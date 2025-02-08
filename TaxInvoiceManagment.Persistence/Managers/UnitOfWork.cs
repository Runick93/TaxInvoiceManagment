using Microsoft.EntityFrameworkCore.Storage;
using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Persistence.Repositories;

namespace TaxInvoiceManagment.Persistence.Managers
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TaxInvoiceManagmentDbContext _context;
        private IDbContextTransaction? _currentTransaction;

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

        /// <summary>
        /// Inicia una transacción.
        /// </summary>
        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }

            _currentTransaction = await _context.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// Confirma la transacción actual.
        /// </summary>
        public async Task CommitTransactionAsync()
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("No transaction in progress to commit.");
            }

            try
            {
                await _currentTransaction.CommitAsync();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        /// <summary>
        /// Realiza un rollback de la transacción actual.
        /// </summary>
        public async Task RollbackTransactionAsync()
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("No transaction in progress to rollback.");
            }

            await _currentTransaction.RollbackAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();

            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}
