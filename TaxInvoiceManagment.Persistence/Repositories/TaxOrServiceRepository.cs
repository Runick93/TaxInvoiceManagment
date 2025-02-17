using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Persistence.Repositories
{
    public class TaxOrServiceRepository : Repository<TaxOrService>, ITaxOrServiceRepository
    {
        private readonly TaxInvoiceManagmentDbContext _context;

        public TaxOrServiceRepository(TaxInvoiceManagmentDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
