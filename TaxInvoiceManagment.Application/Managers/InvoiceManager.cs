using TaxInvoiceManagment.Application.Interfaces;
using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Application.Managers
{
    public class InvoiceManager : IInvoiceManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ICollection<Invoice>> GetAllInvoices()
        {
            var invoices = await _unitOfWork.Invoices.GetAllAsync();
            return invoices.ToList();
        }

        public async Task<Invoice> GetInvoiceById(int id)
        {
            var invoice = await _unitOfWork.Invoices.GetByIdAsync(id);
            if (invoice == null)
            {
                throw new KeyNotFoundException($"Invoice with ID {id} was not found.");
            }
            return invoice;
        }

        public async Task<bool> CreateInvoice(Invoice invoice)
        {            
            int rowsAffected = await _unitOfWork.Invoices.AddAsync(invoice);
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateInvoice(Invoice invoice)
        {            
            int rowsAffected = await _unitOfWork.Invoices.UpdateAsync(invoice);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteInvoice(int id)
        {            
            int rowsAffected = await _unitOfWork.Invoices.DeleteAsync(id);
            return rowsAffected > 0;
        }
    }

}
