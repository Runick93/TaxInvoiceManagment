using TaxInvoiceManagment.Domain.Models;
using TaxInvoiceManagment.Persistence.Repositories;

namespace TaxInvoiceManagment.Persistence.Tests
{
    public class UserRepositoryTests
    {
        [Fact]
        public async Task AddUser_ShouldAddUserToDatabase()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var repository = new UserRepository(context);

            var user = new User { Name = "Homero Simpson", Email = "HSimpsone@mail.com" };

            // Act
            await repository.AddAsync(user);
            await context.SaveChangesAsync();

            // Assert
            var users = await repository.GetAllAsync();
            Assert.Single(users);
            Assert.Equal("Homero Simpson", users.First().Name);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnCorrectUser()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var repository = new UserRepository(context);

            var user = new User { Name = "Homero Simpson", Email = "HSimpsone@mail.com" };
            await repository.AddAsync(user);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByIdAsync(user.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Homero Simpson", result!.Name);
        }
    }
}
