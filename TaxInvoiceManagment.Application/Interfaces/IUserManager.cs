using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Application.Interfaces
{
    public interface IUserManager
    {
        Task<bool> CreateUser(User user);
        Task<bool> DeleteUser(int id);
        Task<ICollection<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<bool> UpdateUser(User user);
    }
}