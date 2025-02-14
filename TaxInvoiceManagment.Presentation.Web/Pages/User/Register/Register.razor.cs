using FluentValidation;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using TaxInvoiceManagment.Application.Dtos;
using TaxInvoiceManagment.Application.Interfaces;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Presentation.Web.Pages.User
{

    public partial class Register
    {
        [Inject] public AntDesign.MessageService Message { get; set; } = default!;
        [Inject] public IUserManager _userManager { get; set; } = default!;

        private UserDto _userDto = new();
        private string _confirmPassword = "";

        private async Task Reg()
        {
            if (_userDto.Password != _userDto.ConfirmPassword)
            {
                await Message.Error("Las contraseñas no coinciden.");
                return;
            }

            var result = await _userManager.CreateUser(_userDto);

            if (result.IsSuccess)
            {
                await Message.Success("Usuario registrado exitosamente!");
                _userDto = new UserDto();
                _confirmPassword = "";
            }
            else
            {
                await Message.Error($"Error en el registro: {string.Join("<br>", result.Errors)}", 10);
            }
        }
    }
}