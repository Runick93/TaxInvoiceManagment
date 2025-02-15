﻿using TaxInvoiceManagment.Application.Interfaces;
using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Application.Managers
{
    public class TaxableItemManager : ITaxableItemManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaxableItemManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ICollection<TaxableItem>> GetAllAssets()
        {
            var taxableItem = await _unitOfWork.TaxableItems.GetAllAsync();
            return taxableItem.ToList();
        }

        public async Task<TaxableItem> GetAssetById(int id)
        {
            var taxableItem = await _unitOfWork.TaxableItems.GetByIdAsync(id);
            if (taxableItem == null)
            {
                throw new KeyNotFoundException($"Asset with ID {id} was not found.");
            }
            return taxableItem;
        }

        public async Task<bool> CreateAsset(TaxableItem asset)
        {            
            await _unitOfWork.TaxableItems.AddAsync(asset);
            int rowsAffected = await _unitOfWork.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateAsset(TaxableItem asset)
        {
            await _unitOfWork.TaxableItems.UpdateAsync(asset);
            int rowsAffected = await _unitOfWork.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsset(int id)
        {
            await _unitOfWork.TaxableItems.DeleteAsync(id);
            int rowsAffected = await _unitOfWork.SaveChangesAsync();
            return rowsAffected > 0;
        }
    }
}
