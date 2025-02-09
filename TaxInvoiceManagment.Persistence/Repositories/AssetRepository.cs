using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Persistence.Repositories
{
    /// <summary>
    /// Se deja ya creada la clase AssetRepository por si a futuro se necesita
    /// agregar metodos de consulta especiales.
    /// </summary>
    public class AssetRepository : Repository<Asset>, IAssetRepository
    {
        private readonly TaxInvoiceManagmentDbContext _context;

        public AssetRepository(TaxInvoiceManagmentDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
