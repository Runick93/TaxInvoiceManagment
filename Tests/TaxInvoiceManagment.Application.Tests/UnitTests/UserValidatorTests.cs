using FluentValidation.TestHelper;
using TaxInvoiceManagment.Application.Validators;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Application.Tests.UnitTests
{
    public class UserValidatorTests
    {
        private readonly UserValidator _validator;

        public UserValidatorTests()
        {
            _validator = new UserValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var model = new User { UserName = "", Email = "test@mail.com", Password = "Valid123!" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(user => user.UserName);
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            var model = new User { UserName = "Homero", Email = "invalid-email", Password = "Valid123!" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(user => user.Email);
        }

        [Fact]
        public void Should_Have_Error_When_Password_Is_Too_Short()
        {
            var model = new User { UserName = "Homero", Email = "homero@mail.com", Password = "123" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(user => user.Password);
        }

        [Fact]
        public void Should_Have_Error_When_Password_Missing_Uppercase()
        {
            var model = new User { UserName = "Homero", Email = "homero@mail.com", Password = "valid123!" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(user => user.Password);
        }

        [Fact]
        public void Should_Have_Error_When_Password_Missing_Digit()
        {
            var model = new User { UserName = "Homero", Email = "homero@mail.com", Password = "ValidPassword!" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(user => user.Password);
        }

        [Fact]
        public void Should_Have_Error_When_Password_Missing_Special_Character()
        {
            var model = new User { UserName = "Homero", Email = "homero@mail.com", Password = "Valid123" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(user => user.Password);
        }

        [Fact]
        public void Should_Not_Have_Error_When_User_Is_Valid()
        {
            var model = new User { UserName = "Homero", Email = "homero@mail.com", Password = "Valid123!" };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(user => user.UserName);
            result.ShouldNotHaveValidationErrorFor(user => user.Email);
            result.ShouldNotHaveValidationErrorFor(user => user.Password);
        }
    }
}
