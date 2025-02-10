using TaxInvoiceManagment.Application.Interfaces;
using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Application.Managers
{
    public class HomeManager : IHomeManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ICollection<Home>> GetAllHomes()
        {
            var homes = await _unitOfWork.Homes.GetAllAsync();
            return homes.ToList();
        }

        public async Task<Home> GetHomeById(int id)
        {
            var home = await _unitOfWork.Homes.GetByIdAsync(id);
            if (home == null)
            {
                throw new KeyNotFoundException($"Home with ID {id} was not found.");
            }
            return home;
        }

        public async Task<bool> CreateHome(Home home)
        {
            await _unitOfWork.Homes.AddAsync(home);
            int rowsAffected = await _unitOfWork.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateHome(Home home)
        {
            await _unitOfWork.Homes.UpdateAsync(home);
            int rowsAffected = await _unitOfWork.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteHome(int id)
        {
            await _unitOfWork.Homes.DeleteAsync(id);
            int rowsAffected = await _unitOfWork.SaveChangesAsync();
            return rowsAffected > 0;
        }
    }
}
