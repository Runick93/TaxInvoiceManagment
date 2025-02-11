namespace TaxInvoiceManagment.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        ITaxableItemRepository TaxableItems { get; }
        ITaxOrServiceRepository TaxesOrServices { get; }
        IInvoiceRepository Invoices { get; }
        Task<int> SaveChangesAsync();
    }
}
