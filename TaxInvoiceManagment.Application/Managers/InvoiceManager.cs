using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using TaxInvoiceManagment.Application.Dtos;
using TaxInvoiceManagment.Application.Interfaces;
using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Application.Managers
{
    public class InvoiceManager : IInvoiceManager
    {
        private readonly ILogger<InvoiceManager> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<InvoiceDto> _invoiceValidator;

        public InvoiceManager(ILogger<InvoiceManager> logger, IUnitOfWork unitOfWork, IMapper mapper, IValidator<InvoiceDto> invoiceValidator)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _invoiceValidator = invoiceValidator;
        }

        public async Task<Result<IEnumerable<InvoiceDto>>> GetAllInvoices()
        {
            var invoices = await _unitOfWork.Invoices.GetAllAsync();

            var invoicesDto = _mapper.Map<IEnumerable<InvoiceDto>>(invoices);
            return Result<IEnumerable<InvoiceDto>>.Success(invoicesDto);          
        }

        public async Task<Result<InvoiceDto>> GetInvoiceById(int id)
        {
            var invoice = await _unitOfWork.Invoices.GetByIdAsync(id);

            if (invoice == null)
            {
                _logger.LogError($"No se encontro la factura con el id: {id}");
                return Result<InvoiceDto>.Failure(new List<string> { $"No se encontro la factura." });
            }

            var invoiceDto = _mapper.Map<InvoiceDto>(invoice);
            return Result<InvoiceDto>.Success(invoiceDto);
        }

        public async Task<Result<InvoiceDto>> CreateInvoice(InvoiceDto invoiceDto)
        {
            var validationResult = await _invoiceValidator.ValidateAsync(invoiceDto);
            if (!validationResult.IsValid)
            {
                _logger.LogError($"Error al validar la factura: {validationResult.Errors}");
                return Result<InvoiceDto>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

            var taxOrService = await _unitOfWork.TaxesOrServices.GetByIdAsync(invoiceDto.TaxOrServiceId);
            if (taxOrService == null)
            {
                _logger.LogError($"No se encontro el impuesto o servicio asociado al ID '{invoiceDto.TaxOrServiceId}'.");
                return Result<InvoiceDto>.Failure(new List<string> { $"No se encontro el impuesto o servicio asociado." });
            }


            var invoice = _mapper.Map<Invoice>(invoiceDto);

            await _unitOfWork.Invoices.AddAsync(invoice);
            await _unitOfWork.SaveChangesAsync();   

            var createdInvoiceDto = _mapper.Map<InvoiceDto>(invoice);
            return Result<InvoiceDto>.Success(createdInvoiceDto);
        }

        public async Task<Result<InvoiceDto>> UpdateInvoice(InvoiceDto invoiceDto)
        {
            var validationResult = await _invoiceValidator.ValidateAsync(invoiceDto);
            if (!validationResult.IsValid)
            {
                _logger.LogError($"Error al validar la factura: {validationResult.Errors}");
                return Result<InvoiceDto>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

            var existingInvoice = await _unitOfWork.Invoices.GetByIdAsync(invoiceDto.Id);
            if (existingInvoice == null)
            {
                _logger.LogError($"No se encontro la factura con el id: {invoiceDto.Id}");
                return Result<InvoiceDto>.Failure(new List<string> { $"No se encontro la factura." });
            }

            _mapper.Map(invoiceDto, existingInvoice);

            await _unitOfWork.Invoices.UpdateAsync(existingInvoice);
            await _unitOfWork.SaveChangesAsync();

            var updatedInvoiceDto = _mapper.Map<InvoiceDto>(existingInvoice);
            return Result<InvoiceDto>.Success(updatedInvoiceDto);
        }

        public async Task<Result<bool>> DeleteInvoice(int id)
        {
            var existingInvoice = await _unitOfWork.Invoices.GetByIdAsync(id);
            if (existingInvoice == null)
            {
                _logger.LogError($"No se encontro la factura con el id: {id}");
                return Result<bool>.Failure(new List<string> { $"No se encontro la factura." });
            }

            try
            {
                await _unitOfWork.Invoices.DeleteAsync(id);
                int rowsAffected = await _unitOfWork.SaveChangesAsync();
                return Result<bool>.Success(rowsAffected > 0);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar la factura: {ex.Message}");
                return Result<bool>.Failure(new List<string> { $"Error al eliminar la factura: {ex.Message}."});
            }
        }
    }
}