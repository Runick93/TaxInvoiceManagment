using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using TaxInvoiceManagment.Application.Common;
using TaxInvoiceManagment.Application.Interfaces;
using TaxInvoiceManagment.Application.Models.Dtos;
using TaxInvoiceManagment.Domain.Entities;
using TaxInvoiceManagment.Domain.Interfaces;

namespace TaxInvoiceManagment.Application.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<UserDto> _userDtoValidator;
        //private readonly IFileSystemService _fileSystemService;

        public UserService(
            ILogger<UserService> logger, 
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IValidator<UserDto> userDtoValidator 
            //IFileSystemService fileSystemService
            )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userDtoValidator = userDtoValidator;
            //_fileSystemService = fileSystemService;
        }

        public async Task<Result<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);
            return Result<IEnumerable<UserDto>>.Success(usersDto);
        }

        public async Task<Result<UserDto>> GetUserById(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);

            if (user == null)
            {
                _logger.LogError($"No se encontro el usuario con el ID: {id}");
                return Result<UserDto>.Failure(new List<string> { $"No se encontro el usuario." });
            }

            var userDto = _mapper.Map<UserDto>(user);
            return Result<UserDto>.Success(userDto);
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

            var user = _mapper.Map<User>(userDto);

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            //_fileSystemService.CreateUserFolder(user.Id, user.UserName);

            var createdUserDto = _mapper.Map<UserDto>(user);
            return Result<UserDto>.Success(createdUserDto);
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

            _mapper.Map(userDto, existingUser);

            await _unitOfWork.Users.UpdateAsync(existingUser);
            await _unitOfWork.SaveChangesAsync();

            var updatedUserDto = _mapper.Map<UserDto>(existingUser);
            return Result<UserDto>.Success(updatedUserDto);
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
