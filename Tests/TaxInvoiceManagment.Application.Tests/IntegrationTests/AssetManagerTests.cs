using System.Threading.Tasks;
using Xunit;
using TaxInvoiceManagment.Application.Managers;
using TaxInvoiceManagment.Domain.Models;
using TaxInvoiceManagment.Persistence.Managers;

namespace TaxInvoiceManagment.Application.Tests.IntegrationTests
{
    public class AssetManagerTests
    {
        [Fact]
        public async Task GetAllAssets_ShouldReturnAllAssets()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var assetManager = new AssetManager(unitOfWork);

            var user = new User { Name = "Homero Simpson", Email = "homero@mail.com" };
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            var asset1 = new Asset { Name = "Casa de Homero", Type = "Casa", UserId = user.Id };
            var asset2 = new Asset { Name = "Auto de Homero", Type = "Auto", UserId = user.Id };

            await assetManager.CreateAsset(asset1);
            await assetManager.CreateAsset(asset2);

            // Act
            var result = await assetManager.GetAllAssets();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAssetById_ShouldReturnCorrectAsset()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var assetManager = new AssetManager(unitOfWork);

            var user = new User { Name = "Homero Simpson", Email = "homero@mail.com" };
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            var asset = new Asset { Name = "Casa de Homero", Type = "Casa", UserId = user.Id };
            await assetManager.CreateAsset(asset);

            // Act
            var result = await assetManager.GetAssetById(asset.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Casa de Homero", result.Name);
        }

        [Fact]
        public async Task GetAssetById_ShouldThrowKeyNotFoundExceptionForNonexistentUser()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var assetManager = new AssetManager(unitOfWork);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => assetManager.GetAssetById(999));
        }

        [Fact]
        public async Task CreateAsset_ShouldAddAssetToDatabase()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var assetManager = new AssetManager(unitOfWork);

            var user = new User { Name = "Homero Simpson", Email = "homero@mail.com" };
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            var asset = new Asset { Name = "Casa de Homero", Type = "Casa", UserId = user.Id };

            // Act
            var result = await assetManager.CreateAsset(asset);

            // Assert
            Assert.True(result);
            Assert.Single(await unitOfWork.Assets.GetAllAsync());
        }

        [Fact]
        public async Task UpdateAsset_ShouldModifyAssetInDatabase()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var assetManager = new AssetManager(unitOfWork);

            var user = new User { Name = "Homero Simpson", Email = "homero@mail.com" };
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            var asset = new Asset { Name = "Casa de Homero", Type = "Casa", UserId = user.Id };
            await assetManager.CreateAsset(asset);

            // Act
            asset.Name = "Casa rodante de Homero";
            var result = await assetManager.UpdateAsset(asset);

            // Assert
            Assert.True(result);
            var updatedAsset = await unitOfWork.Assets.GetByIdAsync(asset.Id);
            Assert.Equal("Casa rodante de Homero", updatedAsset.Name);
        }

        [Fact]
        public async Task DeleteAsset_ShouldRemoveAssetFromDatabase()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var assetManager = new AssetManager(unitOfWork);

            var user = new User { Name = "Homero Simpson", Email = "homero@mail.com" };
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            var asset = new Asset { Name = "Casa de Homero", Type = "Casa", UserId = user.Id };
            await assetManager.CreateAsset(asset);

            // Act
            var result = await assetManager.DeleteAsset(asset.Id);

            // Assert
            Assert.True(result);
            Assert.Empty(await unitOfWork.Assets.GetAllAsync());
        }
    }
}
