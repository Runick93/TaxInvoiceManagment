using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Persistence.Repositories
{
    /// <summary>
    /// Se deja ya creada la clase VehicleRepository por si a futuro se necesita
    /// agregar metodos de consulta especiales.
    /// </summary>
    public class HomeRepository : Repository<Home>, IHomeRepository
    {
        private readonly TaxInvoiceManagmentDbContext _context;

        public HomeRepository(TaxInvoiceManagmentDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
