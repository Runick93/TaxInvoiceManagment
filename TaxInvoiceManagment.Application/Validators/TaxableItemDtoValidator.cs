using FluentValidation;
using TaxInvoiceManagment.Application.Dtos;

namespace TaxInvoiceManagment.Application.Validators
{
    public class TaxableItemDtoValidator : AbstractValidator<TaxableItemDto>
    {
        public TaxableItemDtoValidator()
        {
            RuleFor(t => t.UserId)
                .GreaterThan(0).WithMessage("Debe asociarse a un usuario válido.");

            RuleFor(t => t.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

            RuleFor(t => t.Type)
                .NotEmpty().WithMessage("El tipo es obligatorio.")
                .MaximumLength(50).WithMessage("El tipo no puede superar los 50 caracteres.");

            RuleFor(t => t.Address)
                .MaximumLength(255).WithMessage("La dirección no puede superar los 255 caracteres.");

            RuleFor(t => t.VehicleNumberPlate)
                .MaximumLength(20).WithMessage("La patente no puede superar los 20 caracteres.");
        }
    }
}
