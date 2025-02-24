using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using TaxInvoiceManagment.Application.Common;
using TaxInvoiceManagment.Application.Interfaces;
using TaxInvoiceManagment.Application.Models.Dtos;
using TaxInvoiceManagment.Domain.Entities;
using TaxInvoiceManagment.Domain.Interfaces;

namespace TaxInvoiceManagment.Application.Services
{
    public class TaxService : ITaxService
    {
        private readonly ILogger<TaxService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<TaxDto> _taxDtoValidator;
        //private readonly IFileSystemService _fileSystemService;

        public TaxService(
            ILogger<TaxService> logger, 
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IValidator<TaxDto> taxDtoValidator 
            //IFileSystemService fileSystemService
            )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _taxDtoValidator = taxDtoValidator;
            //_fileSystemService = fileSystemService;
        }

        public async Task<Result<IEnumerable<TaxDto>>> GetAllTaxes()
        {
            var taxes = await _unitOfWork.Taxes.GetAllAsync();

            var taxesDto = _mapper.Map<IEnumerable<TaxDto>>(taxes);
            return Result<IEnumerable<TaxDto>>.Success(taxesDto);
            
        }

        public async Task<Result<TaxDto>> GetTaxById(int id)
        {
            var tax = await _unitOfWork.Taxes.GetByIdAsync(id);

            if (tax == null)
            {
                _logger.LogError($"No se encontro el impuesto o servicio con el id: {id}");
                return Result<TaxDto>.Failure(new List<string> { $"No se encontro el impuesto o servicio." });
            }

            var taxDto = _mapper.Map<TaxDto>(tax);
            return Result<TaxDto>.Success(taxDto);
        }

        public async Task<Result<TaxDto>> CreateTax(TaxDto taxDto)
        {
            var validationResult = await _taxDtoValidator.ValidateAsync(taxDto);
            if (!validationResult.IsValid)
            {
                return Result<TaxDto>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

            TaxableItem? taxableItem = null;
            if (taxDto.TaxableItemId > 0)
            {
                taxableItem = await _unitOfWork.TaxableItems.GetByIdAsync(taxDto.TaxableItemId);
                if (taxableItem == null)
                {
                    _logger.LogError($"No se encontro el objeto imponible asociado al ID '{taxDto.TaxableItemId}'.");
                    return Result<TaxDto>.Failure(new List<string> { $"No se encontro el objeto imponible." });
                }
            }

            var tax = _mapper.Map<Tax>(taxDto);

            await _unitOfWork.Taxes.AddAsync(tax);
            await _unitOfWork.SaveChangesAsync();

            //_fileSystemService.CreateTaxFolder(taxableItem.Id, tax.Id, tax.Name);

            var createdTaxDto = _mapper.Map<TaxDto>(tax);
            return Result<TaxDto>.Success(createdTaxDto);
        }

        public async Task<Result<TaxDto>> UpdateTax(TaxDto taxDto)
        {
            var validationResult = await _taxDtoValidator.ValidateAsync(taxDto);
            if (!validationResult.IsValid)
            {
                _logger.LogError($"Error al validar el impuesto o servicio: {validationResult.Errors}");
                return Result<TaxDto>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

            var existingTax = await _unitOfWork.Taxes.GetByIdAsync(taxDto.Id);
            if (existingTax == null)
            {
                _logger.LogError($"No se encontro el impuesto o servicio con el ID '{taxDto.Id}'.");
                return Result<TaxDto>.Failure(new List<string> { $"No se encontro el impuesto o servicio." });
            }

            _mapper.Map(taxDto, existingTax);

            await _unitOfWork.Taxes.UpdateAsync(existingTax);
            await _unitOfWork.SaveChangesAsync();

            var updatedTaxDto = _mapper.Map<TaxDto>(existingTax);
            return Result<TaxDto>.Success(updatedTaxDto);
        }

        public async Task<Result<bool>> DeleteTax(int id)
        {
            var existingTax = await _unitOfWork.Taxes.GetByIdAsync(id);
            if (existingTax == null)
            {
                _logger.LogError($"No se encontro el impuesto o servicio con el ID '{id}'.");
                return Result<bool>.Failure(new List<string> { $"No se encontro el impuesto o servicio." });
            }

            try
            {
                await _unitOfWork.Taxes.DeleteAsync(id);
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
