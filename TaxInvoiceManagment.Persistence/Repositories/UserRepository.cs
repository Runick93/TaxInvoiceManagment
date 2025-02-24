using Microsoft.EntityFrameworkCore;
using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Entities;
using TaxInvoiceManagment.Persistence.DbContexts;

namespace TaxInvoiceManagment.Persistence.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly TaxInvoiceManagmentDbContext _context;

        public UserRepository(TaxInvoiceManagmentDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByEmail(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> ExistsByUserName(string userName)
        {
            return await _context.Users.AnyAsync(u => u.UserName == userName);
        }
    }
}
