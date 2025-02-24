using FluentValidation;
using TaxInvoiceManagment.Application.Models.Dtos;

namespace TaxInvoiceManagment.Application.Validators
{
    public class TaxDtoValidator : AbstractValidator<TaxDto>
    {
        public TaxDtoValidator()
        {
            RuleFor(t => t.TaxableItemId)
                .GreaterThan(0).When(t => t.TaxableItemId != null)
                .WithMessage("Debe ser un ID de ítem imponible válido.");

            RuleFor(t => t.Name)
                .NotEmpty().WithMessage("El nombre del servicio es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre del servicio no puede superar los 100 caracteres.");

            RuleFor(t => t.ServiceDescription)
                .MaximumLength(500).WithMessage("La descripción no puede superar los 500 caracteres.");

            RuleFor(t => t.Owner)
                .NotEmpty().WithMessage("Debe especificar un responsable.")
                .MaximumLength(100).WithMessage("El nombre del responsable no puede superar los 100 caracteres.");

            RuleFor(t => t.ServiceType)
                .NotEmpty().WithMessage("El tipo de servicio es obligatorio.")
                .MaximumLength(50).WithMessage("El tipo de servicio no puede superar los 50 caracteres.");

            RuleFor(t => t.ClientNumber)
                .MaximumLength(20).WithMessage("El número de cliente no puede superar los 20 caracteres.");
        }
    }
}
