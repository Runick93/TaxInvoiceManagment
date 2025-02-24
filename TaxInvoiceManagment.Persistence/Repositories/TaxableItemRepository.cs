using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Entities;
using TaxInvoiceManagment.Persistence.DbContexts;

namespace TaxInvoiceManagment.Persistence.Repositories
{
    public class TaxableItemRepository : BaseRepository<TaxableItem>, ITaxableItemRepository
    {
        private readonly TaxInvoiceManagmentDbContext _context;

        public TaxableItemRepository(TaxInvoiceManagmentDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
