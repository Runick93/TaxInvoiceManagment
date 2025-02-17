using TaxInvoiceManagment.Application.Dtos;

namespace TaxInvoiceManagment.Application.Interfaces
{
    public interface ITaxOrServiceManager
    {
        Task<Result<TaxOrServiceDto>> CreateTaxOrService(TaxOrServiceDto taxOrServiceDto);
        Task<Result<IEnumerable<TaxOrServiceDto>>> GetAllTaxesOrServices();
        Task<Result<TaxOrServiceDto>> GetTaxOrServiceById(int id);
        Task<Result<TaxOrServiceDto>> UpdateTaxOrService(TaxOrServiceDto taxOrServiceDto);
        Task<Result<bool>> DeleteTaxOrService(int id);
    }
}