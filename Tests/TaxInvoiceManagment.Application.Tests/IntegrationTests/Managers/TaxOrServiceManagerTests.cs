using TaxInvoiceManagment.Application.Managers;
using TaxInvoiceManagment.Domain.Models;
using TaxInvoiceManagment.Persistence.Managers;

namespace TaxInvoiceManagment.Application.Tests.IntegrationTests.Managers
{
    public class TaxOrServiceManagerTests
    {
        [Fact]
        public async Task GetAllTaxesOrServices_ShouldReturnAllTaxesOrServices()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var taxOrServiceManager = new TaxOrServiceManager(unitOfWork);

            var user = new User { UserName = "Homero Simpson", Email = "homero@mail.com", Password = "Passw0rd" };
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            var taxableItem = new TaxableItem { Name = "Casa de Homero", Type = "Casa", UserId = user.Id };
            await unitOfWork.TaxableItems.AddAsync(taxableItem);
            await unitOfWork.SaveChangesAsync();

            var tax1 = new TaxOrService { ServiceType = "Luz", TaxableItemId = taxableItem.Id };
            var tax2 = new TaxOrService { ServiceType = "Agua", TaxableItemId = taxableItem.Id };
            await taxOrServiceManager.CreateTaxOrService(tax1);
            await taxOrServiceManager.CreateTaxOrService(tax2);

            // Act
            var taxesOrServices = await taxOrServiceManager.GetAllTaxesOrServices();

            // Assert
            Assert.Equal(2, taxesOrServices.Count);
        }

        [Fact]
        public async Task GetTaxOrServiceById_ShouldReturnCorrectTaxOrService()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var taxOrServiceManager = new TaxOrServiceManager(unitOfWork);

            var user = new User { UserName = "Homero Simpson", Email = "homero@mail.com", Password = "Passw0rd" };
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            var taxableItem = new TaxableItem { Name = "Casa de Homero", Type = "Casa", UserId = user.Id };
            await unitOfWork.TaxableItems.AddAsync(taxableItem);
            await unitOfWork.SaveChangesAsync();

            var tax = new TaxOrService { ServiceType = "Luz", TaxableItemId = taxableItem.Id };
            await taxOrServiceManager.CreateTaxOrService(tax);

            // Act
            var result = await taxOrServiceManager.GetTaxOrServiceById(tax.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Luz", result.ServiceType);
        }

        [Fact]
        public async Task GetTaxOrServiceById_ShouldThrowKeyNotFoundExceptionForNonexistentTaxOrService()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var taxOrServiceManager = new TaxOrServiceManager(unitOfWork);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => taxOrServiceManager.GetTaxOrServiceById(999));
        }

        [Fact]
        public async Task CreateTaxOrService_ShouldAddTaxOrServiceToDatabase()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var taxOrServiceManager = new TaxOrServiceManager(unitOfWork);

            var user = new User { UserName = "Homero Simpson", Email = "homero@mail.com", Password = "Passw0rd" };
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            var taxableItem = new TaxableItem { Name = "Casa de Homero", Type = "Casa", UserId = user.Id };
            await unitOfWork.TaxableItems.AddAsync(taxableItem);
            await unitOfWork.SaveChangesAsync();

            var tax = new TaxOrService { ServiceType = "Luz", TaxableItemId = taxableItem.Id };

            // Act
            var result = await taxOrServiceManager.CreateTaxOrService(tax);

            // Assert
            Assert.True(result);
            var taxesOrServices = await taxOrServiceManager.GetAllTaxesOrServices();
            Assert.Single(taxesOrServices);
            Assert.Equal("Luz", taxesOrServices.First().ServiceType);
        }

        [Fact]
        public async Task UpdateTaxOrService_ShouldModifyTaxOrServiceInDatabase()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var taxOrServiceManager = new TaxOrServiceManager(unitOfWork);

            var user = new User { UserName = "Homero Simpson", Email = "homero@mail.com", Password = "Passw0rd" };
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            var taxableItem = new TaxableItem { Name = "Casa de Homero", Type = "Casa", UserId = user.Id };
            await unitOfWork.TaxableItems.AddAsync(taxableItem);
            await unitOfWork.SaveChangesAsync();

            var tax = new TaxOrService { ServiceType = "Luz", TaxableItemId = taxableItem.Id };
            await taxOrServiceManager.CreateTaxOrService(tax);

            // Act
            tax.ServiceType = "Agua";
            var result = await taxOrServiceManager.UpdateTaxOrService(tax);

            // Assert
            Assert.True(result);
            var updatedTax = await taxOrServiceManager.GetTaxOrServiceById(tax.Id);
            Assert.Equal("Agua", updatedTax.ServiceType);
        }

        [Fact]
        public async Task DeleteTaxOrService_ShouldRemoveTaxOrServiceFromDatabase()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var taxOrServiceManager = new TaxOrServiceManager(unitOfWork);

            var user = new User { UserName = "Homero Simpson", Email = "homero@mail.com", Password = "Passw0rd" };
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            var taxableItem = new TaxableItem { Name = "Casa de Homero", Type = "Casa", UserId = user.Id };
            await unitOfWork.TaxableItems.AddAsync(taxableItem);
            await unitOfWork.SaveChangesAsync();

            var tax = new TaxOrService { ServiceType = "Luz", TaxableItemId = taxableItem.Id };
            await taxOrServiceManager.CreateTaxOrService(tax);

            // Act
            var result = await taxOrServiceManager.DeleteTaxOrService(tax.Id);

            // Assert
            Assert.True(result);
            var taxesOrServices = await taxOrServiceManager.GetAllTaxesOrServices();
            Assert.Empty(taxesOrServices);
        }
    }
}
