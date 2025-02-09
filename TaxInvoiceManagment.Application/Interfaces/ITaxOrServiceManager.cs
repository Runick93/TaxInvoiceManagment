using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Application.Interfaces
{
    public interface ITaxOrServiceManager
    {
        Task<bool> CreateTaxOrService(TaxOrService taxOrService);
        Task<bool> DeleteTaxOrService(int id);
        Task<ICollection<TaxOrService>> GetAllTaxesOrServices();
        Task<TaxOrService> GetTaxOrServiceById(int id);
        Task<bool> UpdateTaxOrService(TaxOrService taxOrService);
    }
}