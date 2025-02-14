using Microsoft.EntityFrameworkCore;
using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Persistence.Repositories
{
    /// <summary>
    /// Se deja ya creada la clase UserRepository por si a futuro se necesita
    /// agregar metodos de consulta especiales.
    /// </summary>
    public class UserRepository : Repository<User>, IUserRepository
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
