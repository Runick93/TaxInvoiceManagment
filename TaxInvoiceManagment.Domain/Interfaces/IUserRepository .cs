using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> ExistsByEmail(string email);
        Task<bool> ExistsByUserName(string userName);
    }
}
