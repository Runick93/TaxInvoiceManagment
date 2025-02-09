using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Application.Interfaces
{
    public interface IInvoiceManager
    {
        Task<bool> CreateInvoice(Invoice invoice);
        Task<bool> DeleteInvoice(int id);
        Task<ICollection<Invoice>> GetAllInvoices();
        Task<Invoice> GetInvoiceById(int id);
        Task<bool> UpdateInvoice(Invoice invoice);
    }
}