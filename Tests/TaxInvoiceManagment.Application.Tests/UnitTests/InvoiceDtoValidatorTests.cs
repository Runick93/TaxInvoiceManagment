using FluentValidation.TestHelper;
using TaxInvoiceManagment.Application.Dtos;
using TaxInvoiceManagment.Application.Validators;

namespace TaxInvoiceManagment.Application.Tests.UnitTests
{
    public class InvoiceDtoValidatorTests
    {
        private readonly InvoiceDtoValidator invoiceDtoValidator;

        public InvoiceDtoValidatorTests()
        {
            invoiceDtoValidator = new InvoiceDtoValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Month_Is_Empty()
        {
            var invoice = new InvoiceDto { Number = 1, Month = "", InvoiceAmount = 100.50m, PrimaryDueDate = DateTime.Now };
            var result = invoiceDtoValidator.TestValidate(invoice);
            result.ShouldHaveValidationErrorFor(i => i.Month);
        }

        [Fact]
        public void Should_Have_Error_When_InvoiceAmount_Is_Zero()
        {
            var invoice = new InvoiceDto { Number = 1, Month = "Enero", InvoiceAmount = 0, PrimaryDueDate = DateTime.Now };
            var result = invoiceDtoValidator.TestValidate(invoice);
            result.ShouldHaveValidationErrorFor(i => i.InvoiceAmount);
        }

        [Fact]
        public void Should_Have_Error_When_PrimaryDueDate_Is_Default()
        {
            var invoice = new InvoiceDto { Number = 1, Month = "Enero", InvoiceAmount = 100.50m, PrimaryDueDate = default };
            var result = invoiceDtoValidator.TestValidate(invoice);
            result.ShouldHaveValidationErrorFor(i => i.PrimaryDueDate);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Invoice_Is_Valid()
        {
            var invoice = new InvoiceDto { Number = 1, Month = "Enero", InvoiceAmount = 100.50m, PrimaryDueDate = DateTime.Now };
            var result = invoiceDtoValidator.TestValidate(invoice);
            result.ShouldNotHaveValidationErrorFor(i => i.Month);
            result.ShouldNotHaveValidationErrorFor(i => i.InvoiceAmount);
            result.ShouldNotHaveValidationErrorFor(i => i.PrimaryDueDate);
        }
    }
}
