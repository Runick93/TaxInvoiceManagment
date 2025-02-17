using FluentValidation;
using Microsoft.Extensions.Logging;
using TaxInvoiceManagment.Application.Dtos;
using TaxInvoiceManagment.Application.Interfaces;
using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Application.Managers
{
    public class UserManager : IUserManager
    {
        private readonly ILogger<UserManager> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UserDto> _userDtoValidator;
        

        public UserManager(ILogger<UserManager> logger, IUnitOfWork unitOfWork, IValidator<UserDto> userDtoValidator)
        {
            _unitOfWork = unitOfWork;
            _userDtoValidator = userDtoValidator;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _unitOfWork.Users.GetAllAsync();

            return Result<IEnumerable<UserDto>>.Success(users.Select(user => new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            }));
        }

        public async Task<Result<UserDto>> GetUserById(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);

            if (user == null)
            {
                _logger.LogError($"No se encontro el usuario con el ID: {id}");
                return Result<UserDto>.Failure(new List<string> { $"No se encontro el usuario." });
            }

            return Result<UserDto>.Success(new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            });
        }

        public async Task<Result<UserDto>> CreateUser(UserDto userDto)
        {
            var validationResult = await _userDtoValidator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return Result<UserDto>.Failure(errors);
            }

            var errorsList = new List<string>();

            if (await _unitOfWork.Users.ExistsByEmail(userDto.Email))
            {
                _logger.LogError($"Este email ya esta registrado: {userDto.Email}");
                errorsList.Add("Este email ya esta registrado.");
            }

            if (await _unitOfWork.Users.ExistsByUserName(userDto.UserName))
            {
                _logger.LogError($"Este usuario ya esta registrado: {userDto.UserName}");
                errorsList.Add("Este usuario ya esta registrado.");
            }

            if (errorsList.Any())
            {
                return Result<UserDto>.Failure(errorsList); 
            }

            var user = new User
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                Password = userDto.Password
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return Result<UserDto>.Success(new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Password = user.Password // Becareful, this is not a good practice.
            });
        }

        public async Task<Result<UserDto>> UpdateUser(UserDto userDto)
        {
            var validationResult = await _userDtoValidator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
            {
                _logger.LogError($"Error al validar el usuario: {validationResult.Errors}");
                return Result<UserDto>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

            var existingUser = await _unitOfWork.Users.GetByIdAsync(userDto.Id);
            if (existingUser == null)
            {
                _logger.LogError($"No se encontro el usuario con el ID: {userDto.Id}");
                return Result<UserDto>.Failure(new List<string> { $"No se encontro el usuario." });        
            }

            existingUser.UserName = userDto.UserName ?? existingUser.UserName;
            existingUser.Email = userDto.Email ?? existingUser.Email;

            if (!string.IsNullOrEmpty(userDto.Password))
            {
                existingUser.Password = userDto.Password;
            }

            await _unitOfWork.Users.UpdateAsync(existingUser);
            await _unitOfWork.SaveChangesAsync();

            return Result<UserDto>.Success(new UserDto
            {
                Id = existingUser.Id,
                UserName = existingUser.UserName,
                Email = existingUser.Email
            });
        }

        public async Task<Result<bool>> DeleteUser(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogError($"No se encontro el usuario con el ID: {id}");
                return Result<bool>.Failure(new List<string> { $"No se encontro el usuario." });
            }

            try
            {
                await _unitOfWork.Users.DeleteAsync(id);
                int rowsAffected = await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(rowsAffected > 0);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el usuario: {ex.Message}");
                return Result<bool>.Failure(new List<string> { $"Error al eliminar el usuario: {ex.Message}" });
            }
        }
    }
}
