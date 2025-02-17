using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Persistence.Repositories
{
    public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
    {
        private readonly TaxInvoiceManagmentDbContext _context;

        public InvoiceRepository(TaxInvoiceManagmentDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
