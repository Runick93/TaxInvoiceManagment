using FluentValidation.TestHelper;
using TaxInvoiceManagment.Application.Models;
using TaxInvoiceManagment.Application.Models.Dtos;
using TaxInvoiceManagment.Application.Validators;

namespace TaxInvoiceManagment.Application.Tests.UnitTests.Validators
{
    public class UserDtoValidatorTests
    {
        private readonly UserDtoValidator _userDtoValidator;

        public UserDtoValidatorTests()
        {
            _userDtoValidator = new UserDtoValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var userDto = new UserDto { UserName = "", Email = "test@mail.com", Password = "Valid123!" };
            var result = _userDtoValidator.TestValidate(userDto);
            result.ShouldHaveValidationErrorFor(user => user.UserName);
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            var userDto = new UserDto { UserName = "Homero", Email = "invalid-email", Password = "Valid123!" };
            var result = _userDtoValidator.TestValidate(userDto);
            result.ShouldHaveValidationErrorFor(user => user.Email);
        }

        //[Fact]
        //public void Should_Have_Error_When_Password_Is_Too_Short()
        //{
        //    var userDto = new UserDto { UserName = "Homero", Email = "homero@mail.com", Password = "123" };
        //    var result = _userDtoValidator.TestValidate(userDto);
        //    result.ShouldHaveValidationErrorFor(user => user.Password);
        //}

        //[Fact]
        //public void Should_Have_Error_When_Password_Missing_Uppercase()
        //{
        //    var userDto = new UserDto { UserName = "Homero", Email = "homero@mail.com", Password = "valid123!" };
        //    var result = _userDtoValidator.TestValidate(userDto);
        //    result.ShouldHaveValidationErrorFor(user => user.Password);
        //}

        //[Fact]
        //public void Should_Have_Error_When_Password_Missing_Digit()
        //{
        //    var userDto = new UserDto { UserName = "Homero", Email = "homero@mail.com", Password = "ValidPassword!" };
        //    var result = _userDtoValidator.TestValidate(userDto);
        //    result.ShouldHaveValidationErrorFor(user => user.Password);
        //}

        //[Fact]
        //public void Should_Have_Error_When_Password_Missing_Special_Character()
        //{
        //    var userDto = new UserDto { UserName = "Homero", Email = "homero@mail.com", Password = "Valid123" };
        //    var result = _userDtoValidator.TestValidate(userDto);
        //    result.ShouldHaveValidationErrorFor(user => user.Password);
        //}

        [Fact]
        public void Should_Not_Have_Error_When_User_Is_Valid()
        {
            var userDto = new UserDto { UserName = "Homero", Email = "homero@mail.com", Password = "Valid123!" };
            var result = _userDtoValidator.TestValidate(userDto);
            result.ShouldNotHaveValidationErrorFor(user => user.UserName);
            result.ShouldNotHaveValidationErrorFor(user => user.Email);
            result.ShouldNotHaveValidationErrorFor(user => user.Password);
        }
    }
}
