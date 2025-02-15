﻿using TaxInvoiceManagment.Application.Managers;
using TaxInvoiceManagment.Domain.Models;
using TaxInvoiceManagment.Persistence.Managers;

namespace TaxInvoiceManagment.Application.Tests.IntegrationTests.Managers
{
    public class UserManagerIntegrationTests
    {
        [Fact]
        public async Task GetAllUsers_ShouldReturnAllUsers()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var userManager = new UserManager(unitOfWork);

            var user1 = new User { UserName = "Homero Simpson", Email = "homero@mail.com", Password = "Passw0rd" };
            await userManager.CreateUser(user1);

            var user2 = new User { UserName = "Bart Simpson", Email = "bart@mail.com", Password = "Passw0rd" };
            await userManager.CreateUser(user2);

            // Act
            var users = await userManager.GetAllUsers();

            // Assert
            Assert.Equal(2, users.Count);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnCorrectUser()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var userManager = new UserManager(unitOfWork);

            var user = new User { UserName = "Homero Simpson", Email = "homero@mail.com", Password = "Passw0rd" };
            await userManager.CreateUser(user);

            // Act
            var result = await userManager.GetUserById(user.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Homero Simpson", result.UserName);
        }

        [Fact]
        public async Task GetUserById_ShouldThrowKeyNotFoundExceptionForNonexistentUser()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var userManager = new UserManager(unitOfWork);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => userManager.GetUserById(999));
        }

        [Fact]
        public async Task CreateUser_ShouldAddUserToDatabase()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var userManager = new UserManager(unitOfWork);

            var user = new User { UserName = "Homero Simpson", Email = "homero@mail.com", Password = "Passw0rd" };

            // Act
            var result = await userManager.CreateUser(user);

            // Assert
            Assert.True(result);
            var users = await userManager.GetAllUsers();
            Assert.Single(users);
            Assert.Equal("Homero Simpson", users.First().UserName);
        }

        [Fact]
        public async Task UpdateUser_ShouldModifyUserInDatabase()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var userManager = new UserManager(unitOfWork);

            var user = new User { UserName = "Homero Simpson", Email = "homero@mail.com", Password = "Passw0rd" };
            await userManager.CreateUser(user);

            // Act
            user.UserName = "Homero J Simpson";
            var result = await userManager.UpdateUser(user);

            // Assert
            Assert.True(result);
            var updatedUser = await userManager.GetUserById(user.Id);
            Assert.Equal("Homero J Simpson", updatedUser.UserName);
        }

        [Fact]
        public async Task DeleteUser_ShouldRemoveUserFromDatabase()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var userManager = new UserManager(unitOfWork);

            var user = new User { UserName = "Homero Simpson", Email = "homero@mail.com", Password = "Passw0rd" };
            await userManager.CreateUser(user);

            // Act
            var result = await userManager.DeleteUser(user.Id);

            // Assert
            Assert.True(result);
            var users = await userManager.GetAllUsers();
            Assert.Empty(users);
        }
    }
}
