using FluentValidation.TestHelper;
using TaxInvoiceManagment.Application.Dtos;
using TaxInvoiceManagment.Application.Validators;

namespace TaxInvoiceManagment.Application.Tests.UnitTests
{
    public class TaxOrServiceDtoValidatorTests
    {
        private readonly TaxOrServiceDtoValidator _taxOrServiceDtoValidator;

        public TaxOrServiceDtoValidatorTests()
        {
            _taxOrServiceDtoValidator = new TaxOrServiceDtoValidator();
        }

        [Fact]
        public void Should_Have_Error_When_ServiceName_Is_Empty()
        {
            var model = new TaxOrServiceDto { ServiceName = "", Owner = "Homero" };
            var result = _taxOrServiceDtoValidator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(t => t.ServiceName);
        }

        [Fact]
        public void Should_Have_Error_When_Owner_Is_Empty()
        {
            var model = new TaxOrServiceDto { ServiceName = "Electricidad", Owner = "" };
            var result = _taxOrServiceDtoValidator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(t => t.Owner);
        }

        [Fact]
        public void Should_Not_Have_Error_When_TaxOrService_Is_Valid()
        {
            var model = new TaxOrServiceDto { ServiceName = "Agua", Owner = "Homero Simpson" };
            var result = _taxOrServiceDtoValidator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(t => t.ServiceName);
            result.ShouldNotHaveValidationErrorFor(t => t.Owner);
        }
    }
}
