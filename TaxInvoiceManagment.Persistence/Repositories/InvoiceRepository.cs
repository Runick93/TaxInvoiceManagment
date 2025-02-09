using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Persistence.Repositories
{
    /// <summary>
    /// Se deja ya creada la clase InvoiceRepository por si a futuro se necesita
    /// agregar metodos de consulta especiales.
    /// </summary>
    public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
    {
        private readonly TaxInvoiceManagmentDbContext _context;

        public InvoiceRepository(TaxInvoiceManagmentDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
