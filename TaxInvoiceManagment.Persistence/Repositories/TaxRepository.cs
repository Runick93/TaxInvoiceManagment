using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Entities;
using TaxInvoiceManagment.Persistence.DbContexts;

namespace TaxInvoiceManagment.Persistence.Repositories
{
    public class TaxRepository : BaseRepository<Domain.Entities.Tax>, ITaxRepository
    {
        private readonly TaxInvoiceManagmentDbContext _context;

        public TaxRepository(TaxInvoiceManagmentDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
