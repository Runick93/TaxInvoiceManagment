using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Application.Interfaces
{
    public interface IHomeManager
    {
        Task<bool> CreateHome(Home home);
        Task<bool> DeleteHome(int id);
        Task<ICollection<Home>> GetAllHomes();
        Task<Home> GetHomeById(int id);
        Task<bool> UpdateHome(Home home);
    }
}