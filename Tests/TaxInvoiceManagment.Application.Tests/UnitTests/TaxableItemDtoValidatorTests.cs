using FluentValidation.TestHelper;
using TaxInvoiceManagment.Application.Models;
using TaxInvoiceManagment.Application.Models.Dtos;
using TaxInvoiceManagment.Application.Validators;

namespace TaxInvoiceManagment.Application.Tests.UnitTests
{
    public class TaxableItemDtoValidatorTests
    {
        private readonly TaxableItemDtoValidator _taxableItemDtoValidator;

        public TaxableItemDtoValidatorTests()
        {
            _taxableItemDtoValidator = new TaxableItemDtoValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var taxableItemDtoValidator = new TaxableItemDto { Name = "", Type = "Casa", UserId = 1 };
            var result = _taxableItemDtoValidator.TestValidate(taxableItemDtoValidator);
            result.ShouldHaveValidationErrorFor(ti => ti.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Type_Is_Empty()
        {
            var taxableItemDtoValidator = new TaxableItemDto { Name = "Casa", Type = "", UserId = 1 };
            var result = _taxableItemDtoValidator.TestValidate(taxableItemDtoValidator);
            result.ShouldHaveValidationErrorFor(ti => ti.Type);
        }

        [Fact]
        public void Should_Not_Have_Error_When_TaxableItem_Is_Valid()
        {
            var taxableItemDtoValidator = new TaxableItemDto { Name = "Casa", Type = "Residencial", UserId = 1 };
            var result = _taxableItemDtoValidator.TestValidate(taxableItemDtoValidator);
            result.ShouldNotHaveValidationErrorFor(ti => ti.Name);
            result.ShouldNotHaveValidationErrorFor(ti => ti.Type);
        }
    }
}
