using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Persistence.Repositories
{
    /// <summary>
    /// Se deja ya creada la clase VehicleRepository por si a futuro se necesita
    /// agregar metodos de consulta especiales.
    /// </summary>
    public class VehicleRepository : Repository<Vehicle>, IVehicleRepository
    {
        private readonly TaxInvoiceManagmentDbContext _context;

        public VehicleRepository(TaxInvoiceManagmentDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
