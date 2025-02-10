namespace TaxInvoiceManagment.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IAssetRepository Assets { get; }
        IHomeRepository Homes { get; }
        IVehicleRepository Vehicles { get; }
        ITaxOrServiceRepository TaxesOrServices { get; }
        IInvoiceRepository Invoices { get; }
        Task<int> SaveChangesAsync();
    }
}
