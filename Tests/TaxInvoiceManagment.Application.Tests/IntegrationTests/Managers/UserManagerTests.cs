﻿using TaxInvoiceManagment.Application.Dtos;
using TaxInvoiceManagment.Application.Managers;
using TaxInvoiceManagment.Application.Tests;
using TaxInvoiceManagment.Application.Validators;
using TaxInvoiceManagment.Persistence.Managers;

public class UserManagerIntegrationTests
{
    [Fact]
    public async Task GetAllUsers_ShouldReturnAllUsers()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var userManager = new UserManager(unitOfWork, new UserDtoValidator());

        var user1 = new UserDto 
        {
            UserName = "Homero Simpson", 
            Email = "homero@mail.com", 
            Password = "Passw0rd!", 
            ConfirmPassword = "Passw0rd!" 
        };
        await userManager.CreateUser(user1);

        var user2 = new UserDto 
        { 
            UserName = "Bart Simpson", 
            Email = "bart@mail.com", 
            Password = "Passw0rd!", 
            ConfirmPassword = "Passw0rd!" 
        };
        await userManager.CreateUser(user2);

        // Act
        var result = await userManager.GetAllUsers();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(2, result.Value.Count());
    }

    [Fact]
    public async Task GetUserById_ShouldReturnCorrectUser()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var userManager = new UserManager(unitOfWork, new UserDtoValidator());

        var user = new UserDto 
        { 
            UserName = "Homero Simpson", 
            Email = "homero@mail.com", 
            Password = "Passw0rd!", 
            ConfirmPassword = "Passw0rd!" 
        };
        var createdUser = await userManager.CreateUser(user);

        // Act
        var result = await userManager.GetUserById(createdUser.Value.Id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Homero Simpson", result.Value.UserName);
    }

    [Fact]
    public async Task GetUserById_ShouldReturnFailureForNonexistentUser()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var userManager = new UserManager(unitOfWork, new UserDtoValidator());

        // Act
        var result = await userManager.GetUserById(999);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("No se encontro el usuario.", result.Errors);
    }

    [Fact]
    public async Task CreateUser_ShouldAddUserToDatabase()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var userManager = new UserManager(unitOfWork, new UserDtoValidator());

        var user = new UserDto 
        { 
            UserName = "Homero Simpson", 
            Email = "homero@mail.com", 
            Password = "Passw0rd!", 
            ConfirmPassword = "Passw0rd!" 
        };

        // Act
        var result = await userManager.CreateUser(user);

        // Assert
        Assert.True(result.IsSuccess);
        var users = await userManager.GetAllUsers();
        Assert.Single(users.Value);
        Assert.Equal("Homero Simpson", users.Value.First().UserName);
    }

    [Fact]
    public async Task UpdateUser_ShouldModifyUserInDatabase()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var userManager = new UserManager(unitOfWork, new UserDtoValidator());

        var user = new UserDto 
        { 
            UserName = "Homero Simpson", 
            Email = "homero@mail.com", 
            Password = "Passw0rd!", 
            ConfirmPassword = "Passw0rd!"
        };
        var createdUser = await userManager.CreateUser(user);

        // Act
        createdUser.Value.UserName = "Homero J Simpson";
        var updateResult = await userManager.UpdateUser(createdUser.Value);

        // Assert
        Assert.True(updateResult.IsSuccess);
        var updatedUser = await userManager.GetUserById(createdUser.Value.Id);
        Assert.True(updatedUser.IsSuccess);
        Assert.Equal("Homero J Simpson", updatedUser.Value.UserName);
    }

    [Fact]
    public async Task DeleteUser_ShouldRemoveUserFromDatabase()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var userManager = new UserManager(unitOfWork, new UserDtoValidator());

        var user = new UserDto 
        { 
            UserName = "Homero Simpson",
            Email = "homero@mail.com", 
            Password = "Passw0rd!",
            ConfirmPassword = "Passw0rd!" 
        };
        var createdUser = await userManager.CreateUser(user);

        // Act
        var result = await userManager.DeleteUser(createdUser.Value.Id);

        // Assert
        Assert.True(result.IsSuccess);
        var users = await userManager.GetAllUsers();
        Assert.Empty(users.Value);
    }
}
