using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using TaxInvoiceManagment.Application.Interfaces;
using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Entities;
using TaxInvoiceManagment.Application.Models.Dtos;

namespace TaxInvoiceManagment.Application.Managers
{
    public class TaxableItemService : ITaxableItemService
    {
        private readonly ILogger<TaxableItemService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<TaxableItemDto> _taxableItemDtoValidator;

        public TaxableItemService(ILogger<TaxableItemService> logger, IUnitOfWork unitOfWork, IMapper mapper, IValidator<TaxableItemDto> taxableItemDtoValidator)
        {

            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _taxableItemDtoValidator = taxableItemDtoValidator;
        }

        public async Task<Result<IEnumerable<TaxableItemDto>>> GetAllTaxableItems()
        {
            var taxableItems = await _unitOfWork.TaxableItems.GetAllAsync();

            var taxableItemsDto = _mapper.Map<IEnumerable<TaxableItemDto>>(taxableItems);
            return Result<IEnumerable<TaxableItemDto>>.Success(taxableItemsDto);      
        }

        public async Task<Result<TaxableItemDto>> GetTaxableItemById(int id)
        {
            var taxableItem = await _unitOfWork.TaxableItems.GetByIdAsync(id);

            if (taxableItem == null)
            {
                _logger.LogError($"No se encontro el objeto imponible con el id: {id}");
                return Result<TaxableItemDto>.Failure(new List<string> { $"No se encontro el objeto imponible." });
            }

            var taxableItemDto = _mapper.Map<TaxableItemDto>(taxableItem);
            return Result<TaxableItemDto>.Success(taxableItemDto);
        }

        public async Task<Result<TaxableItemDto>> CreateTaxableItem(TaxableItemDto taxableItemDto)
        {
            var validationResult = await _taxableItemDtoValidator.ValidateAsync(taxableItemDto);
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

            var taxableItem = _mapper.Map<TaxableItem>(taxableItemDto);

            await _unitOfWork.TaxableItems.AddAsync(taxableItem);
            await _unitOfWork.SaveChangesAsync();

            var createdTaxableItemDto = _mapper.Map<TaxableItemDto>(taxableItem);
            return Result<TaxableItemDto>.Success(createdTaxableItemDto);
        }

        public async Task<Result<TaxableItemDto>> UpdateTaxableItem(TaxableItemDto taxableItemDto)
        {
            var validationResult = await _taxableItemDtoValidator.ValidateAsync(taxableItemDto);
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

            _mapper.Map(taxableItemDto, existingTaxableItem);

            await _unitOfWork.TaxableItems.UpdateAsync(existingTaxableItem);
            await _unitOfWork.SaveChangesAsync();

            var updatedTaxableItemDto = _mapper.Map<TaxableItemDto>(existingTaxableItem);
            return Result<TaxableItemDto>.Success(updatedTaxableItemDto);
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
