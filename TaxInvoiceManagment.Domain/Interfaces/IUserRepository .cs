using TaxInvoiceManagment.Domain.Entities;

namespace TaxInvoiceManagment.Domain.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<bool> ExistsByEmail(string email);
        Task<bool> ExistsByUserName(string userName);
    }
}
