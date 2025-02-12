using FluentValidation.TestHelper;
using TaxInvoiceManagment.Application.Validators;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Application.Tests.UnitTests
{
    public class TaxableItemValidatorTests
    {
        private readonly TaxableItemValidator _validator;

        public TaxableItemValidatorTests()
        {
            _validator = new TaxableItemValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var model = new TaxableItem { Name = "", Type = "Casa", UserId = 1 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(ti => ti.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Type_Is_Empty()
        {
            var model = new TaxableItem { Name = "Casa", Type = "", UserId = 1 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(ti => ti.Type);
        }

        [Fact]
        public void Should_Not_Have_Error_When_TaxableItem_Is_Valid()
        {
            var model = new TaxableItem { Name = "Casa", Type = "Residencial", UserId = 1 };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(ti => ti.Name);
            result.ShouldNotHaveValidationErrorFor(ti => ti.Type);
        }
    }
}
