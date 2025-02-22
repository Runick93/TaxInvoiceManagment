using FluentValidation.TestHelper;
using TaxInvoiceManagment.Application.Models;
using TaxInvoiceManagment.Application.Models.Dtos;
using TaxInvoiceManagment.Application.Validators;

namespace TaxInvoiceManagment.Application.Tests.UnitTests
{
    public class TaxDtoValidatorTests
    {
        private readonly TaxDtoValidator _taxDtoValidator;

        public TaxDtoValidatorTests()
        {
            _taxDtoValidator = new TaxDtoValidator();
        }

        [Fact]
        public void Should_Have_Error_When_ServiceName_Is_Empty()
        {
            var model = new TaxDto { ServiceName = "", Owner = "Homero" };
            var result = _taxDtoValidator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(t => t.ServiceName);
        }

        [Fact]
        public void Should_Have_Error_When_Owner_Is_Empty()
        {
            var model = new TaxDto { ServiceName = "Electricidad", Owner = "" };
            var result = _taxDtoValidator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(t => t.Owner);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Tax_Is_Valid()
        {
            var model = new TaxDto { ServiceName = "Agua", Owner = "Homero Simpson" };
            var result = _taxDtoValidator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(t => t.ServiceName);
            result.ShouldNotHaveValidationErrorFor(t => t.Owner);
        }
    }
}
