using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Persistence.Repositories
{
    /// <summary>
    /// Se deja ya creada la clase TaxOrServiceRepository por si a futuro se necesita
    /// agregar metodos de consulta especiales.
    /// </summary>
    public class TaxOrServiceRepository : Repository<TaxOrService>, ITaxOrServiceRepository
    {
        private readonly TaxInvoiceManagmentDbContext _context;

        public TaxOrServiceRepository(TaxInvoiceManagmentDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
