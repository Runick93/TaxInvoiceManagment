using FluentValidation;
using TaxInvoiceManagment.Application.Models.Dtos;

namespace TaxInvoiceManagment.Application.Validators
{
    public class InvoiceDtoValidator : AbstractValidator<InvoiceDto>
    {
        public InvoiceDtoValidator()
        {
            RuleFor(i => i.TaxId)
                .GreaterThan(0).WithMessage("Debe estar asociado a un servicio válido.");

            RuleFor(i => i.Number)
                .GreaterThan(0).WithMessage("El número de cuota debe ser mayor a 0.");

            RuleFor(i => i.Month)
                .NotEmpty().WithMessage("El mes es obligatorio.")
                .MaximumLength(20).WithMessage("El mes no puede superar los 20 caracteres.");

            RuleFor(i => i.InvoiceAmount)
                .GreaterThan(0).WithMessage("El monto debe ser mayor a 0.");

            RuleFor(i => i.PrimaryDueDate)
                .NotEmpty().WithMessage("Debe indicar la fecha de vencimiento principal.");

            RuleFor(i => i.InvoiceReceiptPath)
                .MaximumLength(255).WithMessage("La ruta del recibo no puede superar los 255 caracteres.");

            RuleFor(i => i.PaymentReceiptPath)
                .MaximumLength(255).WithMessage("La ruta del comprobante de pago no puede superar los 255 caracteres.");

            RuleFor(i => i.Notes)
                .MaximumLength(500).WithMessage("Las notas no pueden superar los 500 caracteres.");
        }
    }
}
