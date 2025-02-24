using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TaxInvoiceManagment.Application.Mappers;
using TaxInvoiceManagment.Application.Models.Dtos;
using TaxInvoiceManagment.Application.Services;
using TaxInvoiceManagment.Application.Tests.Helpers;
using TaxInvoiceManagment.Application.Validators;
using TaxInvoiceManagment.Persistence;

public class UserServiceTests
{

    [Fact]
    public async Task GetAllUsers_ShouldReturnAllUsers()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);

        var mockLogger = Mock.Of<ILogger<UserService>>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UserMappingProfile>();
        });
        var mapper = config.CreateMapper(); 

        var userManager = new UserService(mockLogger, unitOfWork, mapper, new UserDtoValidator());

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
        var mockLogger = Mock.Of<ILogger<UserService>>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UserMappingProfile>();
        });
        var mapper = config.CreateMapper();

        var userManager = new UserService(mockLogger, unitOfWork, mapper, new UserDtoValidator());


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
    public async Task GetUserById_ShouldReturnFailureForNonExistentUser()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var mockLogger = Mock.Of<ILogger<UserService>>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UserMappingProfile>();
        });
        var mapper = config.CreateMapper();

        var userManager = new UserService(mockLogger, unitOfWork, mapper, new UserDtoValidator());


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
        var mockLogger = Mock.Of<ILogger<UserService>>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UserMappingProfile>();
        });
        var mapper = config.CreateMapper();

        var userManager = new UserService(mockLogger, unitOfWork, mapper, new UserDtoValidator());


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
        var mockLogger = Mock.Of<ILogger<UserService>>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UserMappingProfile>();
        });
        var mapper = config.CreateMapper();

        var userManager = new UserService(mockLogger, unitOfWork, mapper, new UserDtoValidator());


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
        var updatedUser = await userManager.GetAllUsers();
        Assert.True(updatedUser.IsSuccess);
        Assert.Equal("Homero J Simpson", updatedUser.Value.First().UserName);
    }

    [Fact]
    public async Task DeleteUser_ShouldRemoveUserFromDatabase()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var mockLogger = Mock.Of<ILogger<UserService>>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UserMappingProfile>();
        });
        var mapper = config.CreateMapper();

        var userManager = new UserService(mockLogger, unitOfWork, mapper, new UserDtoValidator());


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
