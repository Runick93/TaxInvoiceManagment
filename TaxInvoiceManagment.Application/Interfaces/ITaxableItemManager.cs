using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Application.Interfaces
{
    public interface ITaxableItemManager
    {
        Task<bool> CreateAsset(TaxableItem asset);
        Task<bool> DeleteAsset(int id);
        Task<ICollection<TaxableItem>> GetAllAssets();
        Task<TaxableItem> GetAssetById(int id);
        Task<bool> UpdateAsset(TaxableItem asset);
    }
}