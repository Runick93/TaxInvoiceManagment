using FluentValidation;
using Microsoft.Extensions.Logging;
using TaxInvoiceManagment.Application.Dtos;
using TaxInvoiceManagment.Application.Interfaces;
using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Application.Managers
{
    public class TaxableItemManager : ITaxableItemManager
    {
        private readonly ILogger<TaxableItemManager> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<TaxableItemDto> _taxableItemValidator;

        public TaxableItemManager(ILogger<TaxableItemManager> logger, IUnitOfWork unitOfWork, IValidator<TaxableItemDto> taxableItemValidator)
        {
            _unitOfWork = unitOfWork;
            _taxableItemValidator = taxableItemValidator;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<TaxableItemDto>>> GetAllTaxableItems()
        {
            var taxableItems = await _unitOfWork.TaxableItems.GetAllAsync();

            return Result<IEnumerable<TaxableItemDto>>.Success(taxableItems.Select(t => new TaxableItemDto
            {
                Id = t.Id,
                UserId = t.UserId,
                Name = t.Name,
                Type = t.Type,
                Address = t.Address,
                VehicleNumberPlate = t.VehicleNumberPlate
            }));
        }

        public async Task<Result<TaxableItemDto>> GetTaxableItemById(int id)
        {
            var taxableItem = await _unitOfWork.TaxableItems.GetByIdAsync(id);

            if (taxableItem == null)
            {
                _logger.LogError($"No se encontro el objeto imponible con el id: {id}");
                return Result<TaxableItemDto>.Failure(new List<string> { $"No se encontro el objeto imponible." });
            }

            return Result<TaxableItemDto>.Success(new TaxableItemDto
            {
                Id = taxableItem.Id,
                UserId = taxableItem.UserId,
                Name = taxableItem.Name,
                Type = taxableItem.Type,
                Address = taxableItem.Address,
                VehicleNumberPlate = taxableItem.VehicleNumberPlate
            });
        }

        public async Task<Result<TaxableItemDto>> CreateTaxableItem(TaxableItemDto taxableItemDto)
        {
            var validationResult = await _taxableItemValidator.ValidateAsync(taxableItemDto);
            if (!validationResult.IsValid)
            {
                return Result<TaxableItemDto>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

            if (taxableItemDto.UserId > 0)
            {
                var user = await _unitOfWork.Users.GetByIdAsync(taxableItemDto.UserId);
                if (user == null)
                {
                    _logger.LogError($"No se encontro el usuario con el id: {taxableItemDto.UserId}");
                    return Result<TaxableItemDto>.Failure(new List<string> { $"No se encontro el objeto imponible." });
                }
            }

            var taxableItem = new TaxableItem
            {
                UserId = taxableItemDto.UserId,
                Name = taxableItemDto.Name,
                Type = taxableItemDto.Type,
                Address = taxableItemDto.Address,
                VehicleNumberPlate = taxableItemDto.VehicleNumberPlate
            };

            await _unitOfWork.TaxableItems.AddAsync(taxableItem);
            await _unitOfWork.SaveChangesAsync();

            taxableItemDto.Id = taxableItem.Id;
            return Result<TaxableItemDto>.Success(taxableItemDto);
        }

        public async Task<Result<TaxableItemDto>> UpdateTaxableItem(TaxableItemDto taxableItemDto)
        {
            var validationResult = await _taxableItemValidator.ValidateAsync(taxableItemDto);
            if (!validationResult.IsValid)
            {
                _logger.LogError($"Error al validar el objeto imponible: {validationResult.Errors}");
                return Result<TaxableItemDto>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

            var existingTaxableItem = await _unitOfWork.TaxableItems.GetByIdAsync(taxableItemDto.Id);
            if (existingTaxableItem == null)
            {
                _logger.LogError($"No se encontro el objeto imponible con el ID '{taxableItemDto.Id}'.");
                return Result<TaxableItemDto>.Failure(new List<string> { $"No se encontro el objeto imponible." });
            }

            existingTaxableItem.Name = taxableItemDto.Name;
            existingTaxableItem.Type = taxableItemDto.Type;
            existingTaxableItem.Address = taxableItemDto.Address;
            existingTaxableItem.VehicleNumberPlate = taxableItemDto.VehicleNumberPlate;

            await _unitOfWork.TaxableItems.UpdateAsync(existingTaxableItem);
            await _unitOfWork.SaveChangesAsync();

            return Result<TaxableItemDto>.Success(taxableItemDto);
        }

        public async Task<Result<bool>> DeleteTaxableItem(int id)
        {
            var existingItem = await _unitOfWork.TaxableItems.GetByIdAsync(id);

            if (existingItem == null)
            {
                _logger.LogError($"No se encontro el objeto imponible con el ID '{id}'.");
                return Result<bool>.Failure(new List<string> { $"No se encontro el objeto imponible ." });
            }

            try
            {
                await _unitOfWork.TaxableItems.DeleteAsync(id);
                int rowsAffected = await _unitOfWork.SaveChangesAsync();
                return Result<bool>.Success(rowsAffected > 0);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el objeto imponible: {ex.Message}");
                return Result<bool>.Failure(new List<string> { $"Error al eliminar el objeto imponible: {ex.Message}." });
            }
        }
    }
}
