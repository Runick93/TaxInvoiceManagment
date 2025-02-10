using TaxInvoiceManagment.Application.Interfaces;
using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Application.Managers
{
    public class VehicleManager : IVehicleManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public VehicleManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ICollection<Vehicle>> GetAllVehicles()
        {
            var vehicles = await _unitOfWork.Vehicles.GetAllAsync();
            return vehicles.ToList();
        }

        public async Task<Vehicle> GetVehicleById(int id)
        {
            var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(id);
            if (vehicle == null)
            {
                throw new KeyNotFoundException($"Vehicle with ID {id} was not found.");
            }
            return vehicle;
        }

        public async Task<bool> CreateVehicle(Vehicle vehicle)
        {
            await _unitOfWork.Vehicles.AddAsync(vehicle);
            int rowsAffected = await _unitOfWork.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateVehicle(Vehicle vechicle)
        {
            await _unitOfWork.Vehicles.UpdateAsync(vechicle);
            int rowsAffected = await _unitOfWork.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteVehicle(int id)
        {
            await _unitOfWork.Vehicles.DeleteAsync(id);
            int rowsAffected = await _unitOfWork.SaveChangesAsync();
            return rowsAffected > 0;
        }
    }
}
