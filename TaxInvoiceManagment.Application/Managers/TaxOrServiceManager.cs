using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using TaxInvoiceManagment.Application.Dtos;
using TaxInvoiceManagment.Application.Interfaces;
using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Application.Managers
{
    public class TaxOrServiceManager : ITaxOrServiceManager
    {
        private readonly ILogger<TaxOrServiceManager> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<TaxOrServiceDto> _taxOrServiceValidator;

        public TaxOrServiceManager(ILogger<TaxOrServiceManager> logger, IUnitOfWork unitOfWork, IMapper mapper, IValidator<TaxOrServiceDto> taxOrServiceValidator)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _taxOrServiceValidator = taxOrServiceValidator;
        }

        public async Task<Result<IEnumerable<TaxOrServiceDto>>> GetAllTaxesOrServices()
        {
            var taxesOrServices = await _unitOfWork.TaxesOrServices.GetAllAsync();

            var taxesOrServicesDto = _mapper.Map<IEnumerable<TaxOrServiceDto>>(taxesOrServices);
            return Result<IEnumerable<TaxOrServiceDto>>.Success(taxesOrServicesDto);
            
        }

        public async Task<Result<TaxOrServiceDto>> GetTaxOrServiceById(int id)
        {
            var taxOrService = await _unitOfWork.TaxesOrServices.GetByIdAsync(id);

            if (taxOrService == null)
            {
                _logger.LogError($"No se encontro el impuesto o servicio con el id: {id}");
                return Result<TaxOrServiceDto>.Failure(new List<string> { $"No se encontro el impuesto o servicio." });
            }

            var taxOrServiceDto = _mapper.Map<TaxOrServiceDto>(taxOrService);
            return Result<TaxOrServiceDto>.Success(taxOrServiceDto);
        }

        public async Task<Result<TaxOrServiceDto>> CreateTaxOrService(TaxOrServiceDto taxOrServiceDto)
        {
            var validationResult = await _taxOrServiceValidator.ValidateAsync(taxOrServiceDto);
            if (!validationResult.IsValid)
            {
                return Result<TaxOrServiceDto>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

            if (taxOrServiceDto.TaxableItemId > 0)
            {
                var taxableItem = await _unitOfWork.TaxableItems.GetByIdAsync(taxOrServiceDto.TaxableItemId);
                if (taxableItem == null)
                {
                    _logger.LogError($"No se encontro el objeto imponible asociado al ID '{taxOrServiceDto.TaxableItemId}'.");
                    return Result<TaxOrServiceDto>.Failure(new List<string> { $"No se encontro el objeto imponible." });
                }
            }

            var taxOrService = _mapper.Map<TaxOrService>(taxOrServiceDto);

            await _unitOfWork.TaxesOrServices.AddAsync(taxOrService);
            await _unitOfWork.SaveChangesAsync();

            var createdTaxOrServiceDto = _mapper.Map<TaxOrServiceDto>(taxOrService);
            return Result<TaxOrServiceDto>.Success(createdTaxOrServiceDto);
        }

        public async Task<Result<TaxOrServiceDto>> UpdateTaxOrService(TaxOrServiceDto taxOrServiceDto)
        {
            var validationResult = await _taxOrServiceValidator.ValidateAsync(taxOrServiceDto);
            if (!validationResult.IsValid)
            {
                _logger.LogError($"Error al validar el impuesto o servicio: {validationResult.Errors}");
                return Result<TaxOrServiceDto>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

            var existingTaxOrService = await _unitOfWork.TaxesOrServices.GetByIdAsync(taxOrServiceDto.Id);
            if (existingTaxOrService == null)
            {
                _logger.LogError($"No se encontro el impuesto o servicio con el ID '{taxOrServiceDto.Id}'.");
                return Result<TaxOrServiceDto>.Failure(new List<string> { $"No se encontro el impuesto o servicio." });
            }

            _mapper.Map(taxOrServiceDto, existingTaxOrService);

            await _unitOfWork.TaxesOrServices.UpdateAsync(existingTaxOrService);
            await _unitOfWork.SaveChangesAsync();

            var updatedTaxableItemDto = _mapper.Map<TaxOrServiceDto>(existingTaxOrService);
            return Result<TaxOrServiceDto>.Success(updatedTaxableItemDto);
        }

        public async Task<Result<bool>> DeleteTaxOrService(int id)
        {
            var existingService = await _unitOfWork.TaxesOrServices.GetByIdAsync(id);
            if (existingService == null)
            {
                _logger.LogError($"No se encontro el impuesto o servicio con el ID '{id}'.");
                return Result<bool>.Failure(new List<string> { $"No se encontro el impuesto o servicio." });
            }

            try
            {
                await _unitOfWork.TaxesOrServices.DeleteAsync(id);
                int rowsAffected = await _unitOfWork.SaveChangesAsync();
                return Result<bool>.Success(rowsAffected > 0);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el impuesto o servicio: {ex.Message}");
                return Result<bool>.Failure(new List<string> { $"Error al eliminar el impuesto o servicio." });
            }
        }
    }
}
