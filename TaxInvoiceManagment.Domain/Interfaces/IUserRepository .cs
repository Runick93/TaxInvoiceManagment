using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserWithAssetsAsync(int id);
    }
}
