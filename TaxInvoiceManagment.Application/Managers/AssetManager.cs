using TaxInvoiceManagment.Application.Interfaces;
using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Application.Managers
{
    public class AssetManager : IAssetManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public AssetManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ICollection<Asset>> GetAllAssets()
        {
            var assets = await _unitOfWork.Assets.GetAllAsync();
            return assets.ToList();
        }

        public async Task<Asset> GetAssetById(int id)
        {
            var asset = await _unitOfWork.Assets.GetByIdAsync(id);
            if (asset == null)
            {
                throw new KeyNotFoundException($"Asset with ID {id} was not found.");
            }
            return asset;
        }

        public async Task<bool> CreateAsset(Asset asset)
        {            
            await _unitOfWork.Assets.AddAsync(asset);
            int rowsAffected = await _unitOfWork.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateAsset(Asset asset)
        {
            await _unitOfWork.Assets.UpdateAsync(asset);
            int rowsAffected = await _unitOfWork.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsset(int id)
        {
            await _unitOfWork.Assets.DeleteAsync(id);
            int rowsAffected = await _unitOfWork.SaveChangesAsync();
            return rowsAffected > 0;
        }
    }
}
