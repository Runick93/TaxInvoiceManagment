using FluentValidation.TestHelper;
using TaxInvoiceManagment.Application.Validators;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Application.Tests.UnitTests
{
    public class TaxOrServiceValidatorTests
    {
        private readonly TaxOrServiceValidator _validator;

        public TaxOrServiceValidatorTests()
        {
            _validator = new TaxOrServiceValidator();
        }

        [Fact]
        public void Should_Have_Error_When_ServiceName_Is_Empty()
        {
            var model = new TaxOrService { ServiceName = "", Owner = "Homero" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(t => t.ServiceName);
        }

        [Fact]
        public void Should_Have_Error_When_Owner_Is_Empty()
        {
            var model = new TaxOrService { ServiceName = "Electricidad", Owner = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(t => t.Owner);
        }

        [Fact]
        public void Should_Not_Have_Error_When_TaxOrService_Is_Valid()
        {
            var model = new TaxOrService { ServiceName = "Agua", Owner = "Homero Simpson" };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(t => t.ServiceName);
            result.ShouldNotHaveValidationErrorFor(t => t.Owner);
        }
    }
}
