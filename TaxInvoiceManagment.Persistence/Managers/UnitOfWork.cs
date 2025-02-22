using Microsoft.EntityFrameworkCore.Storage;
using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Persistence.Repositories;

namespace TaxInvoiceManagment.Persistence.Managers
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly TaxInvoiceManagmentDbContext _context;
        private IDbContextTransaction? _currentTransaction;
        private bool _disposed = false;

        public IUserRepository Users { get; }
        public ITaxableItemRepository TaxableItems { get; }
        public ITaxRepository Taxes { get; }
        public IInvoiceRepository Invoices { get; }

        public UnitOfWork(TaxInvoiceManagmentDbContext context)
        {
            _context = context;
            Users = new UserRepository(context);
            TaxableItems = new TaxableItemRepository(context);
            Taxes = new TaxRepository(context);
            Invoices = new InvoiceRepository(context);
        }

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }

            _currentTransaction = await _context.Database.BeginTransactionAsync();
        }

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

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();

                    if (_currentTransaction != null)
                    {
                        _currentTransaction.Dispose();
                        _currentTransaction = null;
                    }
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
