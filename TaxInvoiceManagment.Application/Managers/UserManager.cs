﻿using FluentValidation;
using Microsoft.AspNetCore.Identity;
using TaxInvoiceManagment.Application.Dtos;
using TaxInvoiceManagment.Application.Interfaces;
using TaxInvoiceManagment.Application.Validators;
using TaxInvoiceManagment.Domain.Interfaces;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Application.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IValidator<UserDto> _userDtoValidator;
        private readonly IUnitOfWork _unitOfWork;

        public UserManager(IValidator<UserDto> userDtoValidator, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userDtoValidator = userDtoValidator;
        }

        //-----------------------------------------------------------------------------------------
        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return users?.Select(user => new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            }) ?? new List<UserDto>();
        }

        public async Task<Result<UserDto>> GetUserById(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);

            if (user == null)
            {
                return Result<UserDto>.Failure(new List<string> { $"No se encontro el usuario con el ID '{id}'" });
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
                errorsList.Add("Este email ya está registrado.");
            }

            if (await _unitOfWork.Users.ExistsByUserName(userDto.UserName))
            {
                errorsList.Add("Este usuario ya está registrado.");
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
                Email = user.Email
            });
        }

        public async Task<Result<UserDto>> UpdateUser(UserDto userDto)
        {
            var validationResult = await _userDtoValidator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return Result<UserDto>.Failure(errors);
            }

            var existingUser = await _unitOfWork.Users.GetByIdAsync(userDto.Id);
            if (existingUser == null)
            {
                return Result<UserDto>.Failure(new List<string> { $"No se encontro el usuario con el ID '{userDto.Id}'" });        
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
                return Result<bool>.Failure(new List<string> { $"No se encontro el usuario con el ID '{id}'" });
            }

            try
            {
                await _unitOfWork.Users.DeleteAsync(id);
                int rowsAffected = await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(rowsAffected > 0);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(new List<string> { $"Error al eliminar el usuario: {ex.Message}" });
            }
        }
    }
}
