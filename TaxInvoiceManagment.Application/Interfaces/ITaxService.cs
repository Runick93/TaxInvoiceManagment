using TaxInvoiceManagment.Application.Models.Dtos;

namespace TaxInvoiceManagment.Application.Interfaces
{
    public interface ITaxService
    {
        Task<Result<TaxDto>> CreateTax(TaxDto taxDto);
        Task<Result<IEnumerable<TaxDto>>> GetAllTaxes();
        Task<Result<TaxDto>> GetTaxById(int id);
        Task<Result<TaxDto>> UpdateTax(TaxDto taxDto);
        Task<Result<bool>> DeleteTax(int id);
    }
}