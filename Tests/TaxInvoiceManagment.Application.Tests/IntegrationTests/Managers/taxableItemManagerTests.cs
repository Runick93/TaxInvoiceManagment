using TaxInvoiceManagment.Application.Managers;
using TaxInvoiceManagment.Domain.Models;
using TaxInvoiceManagment.Persistence.Managers;

namespace TaxInvoiceManagment.Application.Tests.IntegrationTests.Managers
{
    public class taxableItemManagerTests
    {
        [Fact]
        public async Task GetAlltaxableItems_ShouldReturnAllTaxableItems()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var taxableItemManager = new TaxableItemManager(unitOfWork);

            var user = new User { Name = "Homero Simpson", Email = "homero@mail.com", Password = "Passw0rd" };
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            var taxableItem1 = new TaxableItem { Name = "Casa de Homero", Type = "Casa", UserId = user.Id };
            var taxableItem2 = new TaxableItem { Name = "Auto de Homero", Type = "Auto", UserId = user.Id };

            await taxableItemManager.CreateAsset(taxableItem1);
            await taxableItemManager.CreateAsset(taxableItem2);

            // Act
            var result = await taxableItemManager.GetAllAssets();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetTaxableItemById_ShouldReturnCorrectTaxableItem()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var taxableItemManager = new TaxableItemManager(unitOfWork);

            var user = new User { Name = "Homero Simpson", Email = "homero@mail.com", Password = "Passw0rd" };
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            var taxableItem = new TaxableItem { Name = "Casa de Homero", Type = "Casa", UserId = user.Id };
            await taxableItemManager.CreateAsset(taxableItem);

            // Act
            var result = await taxableItemManager.GetAssetById(taxableItem.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Casa de Homero", result.Name);
        }

        [Fact]
        public async Task GetTaxableItemById_ShouldThrowKeyNotFoundExceptionForNonexistentTaxableItem()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var taxableItemManager = new TaxableItemManager(unitOfWork);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => taxableItemManager.GetAssetById(999));
        }

        [Fact]
        public async Task CreateTaxableItem_ShouldAddTaxableItemToDatabase()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var assetManager = new TaxableItemManager(unitOfWork);

            var user = new User { Name = "Homero Simpson", Email = "homero@mail.com", Password = "Passw0rd" };
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            var taxableItem = new TaxableItem { Name = "Casa de Homero", Type = "Casa", UserId = 1 /*user.Id*/ };

            // Act
            var result = await assetManager.CreateAsset(taxableItem);

            // Assert
            Assert.True(result);
            Assert.Single(await unitOfWork.TaxableItems.GetAllAsync());
        }

        [Fact]
        public async Task UpdateTaxableItem_ShouldModifyTaxableItemInDatabase()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var taxableItemManager = new TaxableItemManager(unitOfWork);

            var user = new User { Name = "Homero Simpson", Email = "homero@mail.com", Password = "Passw0rd" };
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            var taxableItem = new TaxableItem { Name = "Casa de Homero", Type = "Casa", UserId = user.Id };
            await taxableItemManager.CreateAsset(taxableItem);

            // Act
            taxableItem.Name = "Casa rodante de Homero";
            var result = await taxableItemManager.UpdateAsset(taxableItem);

            // Assert
            Assert.True(result);
            var updatedTaxableItem = await unitOfWork.TaxableItems.GetByIdAsync(taxableItem.Id);
            Assert.Equal("Casa rodante de Homero", updatedTaxableItem.Name);
        }

        [Fact]
        public async Task DeleteTaxableItem_ShouldRemoveTaxableItemFromDatabase()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var taxableItemManager = new TaxableItemManager(unitOfWork);

            var user = new User { Name = "Homero Simpson", Email = "homero@mail.com", Password = "Passw0rd" };
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            var taxableItem = new TaxableItem { Name = "Casa de Homero", Type = "Casa", UserId = user.Id };
            await taxableItemManager.CreateAsset(taxableItem);

            // Act
            var result = await taxableItemManager.DeleteAsset(taxableItem.Id);

            // Assert
            Assert.True(result);
            Assert.Empty(await unitOfWork.TaxableItems.GetAllAsync());
        }
    }
}
