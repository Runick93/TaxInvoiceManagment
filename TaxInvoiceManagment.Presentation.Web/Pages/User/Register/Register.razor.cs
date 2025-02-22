using Microsoft.AspNetCore.Components;
using TaxInvoiceManagment.Application.Interfaces;
using TaxInvoiceManagment.Application.Models.Dtos;

namespace TaxInvoiceManagment.Presentation.Web.Pages.User
{

    public partial class Register
    {
        [Inject] public AntDesign.MessageService Message { get; set; } = default!;
        [Inject] public IUserService _userManager { get; set; } = default!;

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
                await Message.Error($"Error en el registro: {string.Join("\n", result.Errors)}", 10);
            }
        }
    }
}