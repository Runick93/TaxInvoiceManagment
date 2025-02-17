using FluentValidation;
using TaxInvoiceManagment.Application.Dtos;
using TaxInvoiceManagment.Application.Interfaces;
using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Application.Managers
{
    public class TaxOrServiceManager : ITaxOrServiceManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<TaxOrServiceDto> _taxOrServiceValidator;

        public TaxOrServiceManager(IUnitOfWork unitOfWork, IValidator<TaxOrServiceDto> taxOrServiceValidator)
        {
            _unitOfWork = unitOfWork;
            _taxOrServiceValidator = taxOrServiceValidator;
        }

        public async Task<Result<IEnumerable<TaxOrServiceDto>>> GetAllTaxesOrServices()
        {
            var taxesOrServices = await _unitOfWork.TaxesOrServices.GetAllAsync();

            return Result<IEnumerable<TaxOrServiceDto>>.Success(taxesOrServices.Select(t => new TaxOrServiceDto
            {
                Id = t.Id,
                TaxableItemId = t.TaxableItemId,
                ServiceName = t.ServiceName,
                ServiceDescription = t.ServiceDescription,
                Owner = t.Owner,
                ServiceType = t.ServiceType,
                PayFrequency = t.PayFrequency.ToString(),
                AnnualPayment = t.AnnualPayment,
                ClientNumber = t.ClientNumber
            }));
        }

        public async Task<Result<TaxOrServiceDto>> GetTaxOrServiceById(int id)
        {
            var taxOrService = await _unitOfWork.TaxesOrServices.GetByIdAsync(id);

            if (taxOrService == null)
            {
                return Result<TaxOrServiceDto>.Failure(new List<string> { $"No se encontro el impuesto o servicio." });
            }

            return Result<TaxOrServiceDto>.Success(new TaxOrServiceDto
            {
                Id = taxOrService.Id,
                TaxableItemId = taxOrService.TaxableItemId,
                ServiceName = taxOrService.ServiceName,
                ServiceDescription = taxOrService.ServiceDescription,
                Owner = taxOrService.Owner,
                ServiceType = taxOrService.ServiceType,
                PayFrequency = taxOrService.PayFrequency.ToString(),
                AnnualPayment = taxOrService.AnnualPayment,
                ClientNumber = taxOrService.ClientNumber
            });
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
                    return Result<TaxOrServiceDto>.Failure(new List<string> { $"No se encontro el objeto imponible." });
                }
            }

            var taxOrService = new TaxOrService
            {
                TaxableItemId = taxOrServiceDto.TaxableItemId,
                ServiceName = taxOrServiceDto.ServiceName,
                ServiceDescription = taxOrServiceDto.ServiceDescription,
                Owner = taxOrServiceDto.Owner,
                ServiceType = taxOrServiceDto.ServiceType,
                PayFrequency = Enum.Parse<TaxOrService.PaymentFrequency>(taxOrServiceDto.PayFrequency),
                AnnualPayment = taxOrServiceDto.AnnualPayment,
                ClientNumber = taxOrServiceDto.ClientNumber
            };

            await _unitOfWork.TaxesOrServices.AddAsync(taxOrService);
            await _unitOfWork.SaveChangesAsync();

            taxOrServiceDto.Id = taxOrService.Id;
            return Result<TaxOrServiceDto>.Success(taxOrServiceDto);
        }

        public async Task<Result<TaxOrServiceDto>> UpdateTaxOrService(TaxOrServiceDto taxOrServiceDto)
        {
            var validationResult = await _taxOrServiceValidator.ValidateAsync(taxOrServiceDto);
            if (!validationResult.IsValid)
            {
                return Result<TaxOrServiceDto>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

            var existingTaxOrService = await _unitOfWork.TaxesOrServices.GetByIdAsync(taxOrServiceDto.Id);
            if (existingTaxOrService == null)
            {
                return Result<TaxOrServiceDto>.Failure(new List<string> { $"No se encontro el impuesto o servicio." });
            }

            existingTaxOrService.ServiceName = taxOrServiceDto.ServiceName;
            existingTaxOrService.ServiceDescription = taxOrServiceDto.ServiceDescription;
            existingTaxOrService.Owner = taxOrServiceDto.Owner;
            existingTaxOrService.ServiceType = taxOrServiceDto.ServiceType;
            existingTaxOrService.PayFrequency = Enum.Parse<TaxOrService.PaymentFrequency>(taxOrServiceDto.PayFrequency);
            existingTaxOrService.AnnualPayment = taxOrServiceDto.AnnualPayment;
            existingTaxOrService.ClientNumber = taxOrServiceDto.ClientNumber;

            await _unitOfWork.TaxesOrServices.UpdateAsync(existingTaxOrService);
            await _unitOfWork.SaveChangesAsync();

            return Result<TaxOrServiceDto>.Success(taxOrServiceDto);
        }

        public async Task<Result<bool>> DeleteTaxOrService(int id)
        {
            var existingService = await _unitOfWork.TaxesOrServices.GetByIdAsync(id);
            if (existingService == null)
            {
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
                return Result<bool>.Failure(new List<string> { $"Error al eliminar el impuesto o servicio." });
            }
        }
    }
}
