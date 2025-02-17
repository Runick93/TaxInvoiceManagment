using TaxInvoiceManagment.Application.Dtos;

namespace TaxInvoiceManagment.Application.Interfaces
{
    public interface ITaxableItemManager
    {
        Task<Result<TaxableItemDto>> CreateTaxableItem(TaxableItemDto taxableItemDto);
        Task<Result<IEnumerable<TaxableItemDto>>> GetAllTaxableItems();
        Task<Result<TaxableItemDto>> GetTaxableItemById(int id);
        Task<Result<TaxableItemDto>> UpdateTaxableItem(TaxableItemDto taxableItemDto);
        Task<Result<bool>> DeleteTaxableItem(int id);
    }
}