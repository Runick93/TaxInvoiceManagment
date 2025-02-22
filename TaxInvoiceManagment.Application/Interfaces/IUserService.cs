using TaxInvoiceManagment.Application.Models.Dtos;

namespace TaxInvoiceManagment.Application.Interfaces
{
    public interface IUserService
    {
        Task<Result<UserDto>> CreateUser(UserDto userDto);
        Task<Result<IEnumerable<UserDto>>> GetAllUsers();
        Task<Result<UserDto>> GetUserById(int id);
        Task<Result<UserDto>> UpdateUser(UserDto userDto);
        Task<Result<bool>> DeleteUser(int id);
    }
}