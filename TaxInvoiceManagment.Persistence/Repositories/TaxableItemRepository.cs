using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Persistence.Repositories
{
    /// <summary>
    /// Se deja ya creada la clase AssetRepository por si a futuro se necesita
    /// agregar metodos de consulta especiales.
    /// </summary>
    public class TaxableItemRepository : Repository<TaxableItem>, ITaxableItemRepository
    {
        private readonly TaxInvoiceManagmentDbContext _context;

        public TaxableItemRepository(TaxInvoiceManagmentDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
