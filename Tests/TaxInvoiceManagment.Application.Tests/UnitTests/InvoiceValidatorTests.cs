using FluentValidation.TestHelper;
using TaxInvoiceManagment.Application.Validators;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Application.Tests.UnitTests
{
    public class InvoiceValidatorTests
    {
        private readonly InvoiceValidator _validator;

        public InvoiceValidatorTests()
        {
            _validator = new InvoiceValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Month_Is_Empty()
        {
            var model = new Invoice { Number = 1, Month = "", InvoiceAmount = 100.50m, PrimaryDueDate = DateTime.Now };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(i => i.Month);
        }

        [Fact]
        public void Should_Have_Error_When_InvoiceAmount_Is_Zero()
        {
            var model = new Invoice { Number = 1, Month = "Enero", InvoiceAmount = 0, PrimaryDueDate = DateTime.Now };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(i => i.InvoiceAmount);
        }

        [Fact]
        public void Should_Have_Error_When_PrimaryDueDate_Is_Default()
        {
            var model = new Invoice { Number = 1, Month = "Enero", InvoiceAmount = 100.50m, PrimaryDueDate = default };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(i => i.PrimaryDueDate);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Invoice_Is_Valid()
        {
            var model = new Invoice { Number = 1, Month = "Enero", InvoiceAmount = 100.50m, PrimaryDueDate = DateTime.Now };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(i => i.Month);
            result.ShouldNotHaveValidationErrorFor(i => i.InvoiceAmount);
            result.ShouldNotHaveValidationErrorFor(i => i.PrimaryDueDate);
        }
    }
}
