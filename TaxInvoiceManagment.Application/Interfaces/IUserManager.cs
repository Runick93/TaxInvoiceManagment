using TaxInvoiceManagment.Application.Dtos;

namespace TaxInvoiceManagment.Application.Interfaces
{
    public interface IUserManager
    {
        Task<Result<UserDto>> CreateUser(UserDto userDto);
        Task<Result<IEnumerable<UserDto>>> GetAllUsers();
        Task<Result<UserDto>> GetUserById(int id);
        Task<Result<UserDto>> UpdateUser(UserDto userDto);
        Task<Result<bool>> DeleteUser(int id);
    }
}