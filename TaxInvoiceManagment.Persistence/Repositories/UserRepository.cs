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

        public async Task<User?> GetUserWithAssetsAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Assets)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
