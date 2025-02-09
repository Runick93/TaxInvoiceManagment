using TaxInvoiceManagment.Application.Interfaces;
using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Application.Managers
{
    public class TaxOrServiceManager : ITaxOrServiceManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaxOrServiceManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ICollection<TaxOrService>> GetAllTaxesOrServices()
        {
            var taxesOrServices = await _unitOfWork.TaxesOrServices.GetAllAsync();
            return taxesOrServices.ToList();
        }

        public async Task<TaxOrService> GetTaxOrServiceById(int id)
        {
            var taxOrService = await _unitOfWork.TaxesOrServices.GetByIdAsync(id);
            if (taxOrService == null)
            {
                throw new KeyNotFoundException($"TaxOrService with ID {id} was not found.");
            }
            return taxOrService;
        }

        public async Task<bool> CreateTaxOrService(TaxOrService taxOrService)
        {            
            int rowsAffected = await _unitOfWork.TaxesOrServices.AddAsync(taxOrService);
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateTaxOrService(TaxOrService taxOrService)
        {            
            int rowsAffected = await _unitOfWork.TaxesOrServices.UpdateAsync(taxOrService);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteTaxOrService(int id)
        {            
            int rowsAffected = await _unitOfWork.TaxesOrServices.DeleteAsync(id);
            return rowsAffected > 0;
        }
    }
}
