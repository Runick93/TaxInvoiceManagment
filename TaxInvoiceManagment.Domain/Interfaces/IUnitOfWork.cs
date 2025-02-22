namespace TaxInvoiceManagment.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        ITaxableItemRepository TaxableItems { get; }
        ITaxRepository Taxes { get; }
        IInvoiceRepository Invoices { get; }
        Task<int> SaveChangesAsync();
    }
}
