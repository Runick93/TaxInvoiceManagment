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
        private readonly IValidator<InvoiceDto> _invoiceValidator;

        public InvoiceManager(ILogger<InvoiceManager> logger, IUnitOfWork unitOfWork, IValidator<InvoiceDto> invoiceValidator)
        {
            _unitOfWork = unitOfWork;
            _invoiceValidator = invoiceValidator;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<InvoiceDto>>> GetAllInvoices()
        {
            var invoices = await _unitOfWork.Invoices.GetAllAsync();
            return Result<IEnumerable<InvoiceDto>>.Success(invoices.Select(i => new InvoiceDto
            {
                Id = i.Id,
                TaxOrServiceId = i.TaxOrServiceId,
                Number = i.Number,
                Month = i.Month,
                InvoiceAmount = i.InvoiceAmount,
                PaymentStatus = i.PaymentStatus,
                PaymentDate = i.PaymentDate,
                PrimaryDueDate = i.PrimaryDueDate,
                SecondaryDueDate = i.SecondaryDueDate,
                InvoiceReceiptPath = i.InvoiceReceiptPath,
                PaymentReceiptPath = i.PaymentReceiptPath,
                Notes = i.Notes
            }));
        }

        public async Task<Result<InvoiceDto>> GetInvoiceById(int id)
        {
            var invoice = await _unitOfWork.Invoices.GetByIdAsync(id);

            if (invoice == null)
            {
                _logger.LogError($"No se encontro la factura con el id: {id}");
                return Result<InvoiceDto>.Failure(new List<string> { $"No se encontro la factura." });
            }

            return Result<InvoiceDto>.Success(new InvoiceDto
            {
                Id = invoice.Id,
                TaxOrServiceId = invoice.TaxOrServiceId,
                Number = invoice.Number,
                Month = invoice.Month,
                InvoiceAmount = invoice.InvoiceAmount,
                PaymentStatus = invoice.PaymentStatus,
                PaymentDate = invoice.PaymentDate,
                PrimaryDueDate = invoice.PrimaryDueDate,
                SecondaryDueDate = invoice.SecondaryDueDate,
                InvoiceReceiptPath = invoice.InvoiceReceiptPath,
                PaymentReceiptPath = invoice.PaymentReceiptPath,
                Notes = invoice.Notes
            });
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

            var invoice = new Invoice
            {
                TaxOrServiceId = invoiceDto.TaxOrServiceId,
                Number = invoiceDto.Number,
                Month = invoiceDto.Month,
                InvoiceAmount = invoiceDto.InvoiceAmount,
                PaymentStatus = invoiceDto.PaymentStatus,
                PaymentDate = invoiceDto.PaymentDate,
                PrimaryDueDate = invoiceDto.PrimaryDueDate,
                SecondaryDueDate = invoiceDto.SecondaryDueDate,
                InvoiceReceiptPath = invoiceDto.InvoiceReceiptPath,
                PaymentReceiptPath = invoiceDto.PaymentReceiptPath,
                Notes = invoiceDto.Notes
            };

            await _unitOfWork.Invoices.AddAsync(invoice);
            await _unitOfWork.SaveChangesAsync();

            invoiceDto.Id = invoice.Id;
            return Result<InvoiceDto>.Success(invoiceDto);
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

            existingInvoice.Number = invoiceDto.Number;
            existingInvoice.Month = invoiceDto.Month;
            existingInvoice.InvoiceAmount = invoiceDto.InvoiceAmount;
            existingInvoice.PaymentStatus = invoiceDto.PaymentStatus;
            existingInvoice.PaymentDate = invoiceDto.PaymentDate;
            existingInvoice.PrimaryDueDate = invoiceDto.PrimaryDueDate;
            existingInvoice.SecondaryDueDate = invoiceDto.SecondaryDueDate;
            existingInvoice.InvoiceReceiptPath = invoiceDto.InvoiceReceiptPath;
            existingInvoice.PaymentReceiptPath = invoiceDto.PaymentReceiptPath;
            existingInvoice.Notes = invoiceDto.Notes;

            await _unitOfWork.Invoices.UpdateAsync(existingInvoice);
            await _unitOfWork.SaveChangesAsync();

            return Result<InvoiceDto>.Success(invoiceDto);
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