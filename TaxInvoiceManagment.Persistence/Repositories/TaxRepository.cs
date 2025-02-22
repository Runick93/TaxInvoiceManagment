using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Entities;

namespace TaxInvoiceManagment.Persistence.Repositories
{
    public class TaxRepository : Repository<Domain.Entities.Tax>, ITaxRepository
    {
        private readonly TaxInvoiceManagmentDbContext _context;

        public TaxRepository(TaxInvoiceManagmentDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
