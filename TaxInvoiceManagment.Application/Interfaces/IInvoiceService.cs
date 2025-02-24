using TaxInvoiceManagment.Application.Common;
using TaxInvoiceManagment.Application.Models.Dtos;

namespace TaxInvoiceManagment.Application.Interfaces
{
    public interface IInvoiceService
    {
        Task<Result<InvoiceDto>> CreateInvoice(InvoiceDto invoiceDto);
        Task<Result<IEnumerable<InvoiceDto>>> GetAllInvoices();
        Task<Result<InvoiceDto>> GetInvoiceById(int id);
        Task<Result<InvoiceDto>> UpdateInvoice(InvoiceDto invoiceDto);
        Task<Result<bool>> DeleteInvoice(int id);
    }
}