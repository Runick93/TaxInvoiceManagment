using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Application.Interfaces
{
    public interface IVehicleManager
    {
        Task<bool> CreateVehicle(Vehicle vehicle);
        Task<bool> DeleteVehicle(int id);
        Task<ICollection<Vehicle>> GetAllVehicles();
        Task<Vehicle> GetVehicleById(int id);
        Task<bool> UpdateVehicle(Vehicle vechicle);
    }
}