using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Application.Interfaces
{
    public interface IAssetManager
    {
        Task<bool> CreateAsset(Asset asset);
        Task<bool> DeleteAsset(int id);
        Task<ICollection<Asset>> GetAllAssets();
        Task<Asset> GetAssetById(int id);
        Task<bool> UpdateAsset(Asset asset);
    }
}