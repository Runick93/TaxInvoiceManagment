using TaxInvoiceManagment.Domain.Models;
using TaxInvoiceManagment.Persistence.Managers;

namespace TaxInvoiceManagment.Persistence.Tests
{
    public class UnitOfWorkTests
    {
        // User.
        [Fact]
        public async Task User_GetAllAsync_ShouldReturnUsers()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var user = new User { Name = "Homero Simpson", Email = "homero@mail.com" };

            // Act
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            // Assert
            var users = await unitOfWork.Users.GetAllAsync();
            Assert.Single(users);
            Assert.Equal("Homero Simpson", users.First().Name);
        }

        [Fact]
        public async Task User_GetByIdAsync_ShouldReturnUser()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var user = new User { Name = "Homero Simpson", Email = "homero@mail.com" };
            await unitOfWork.Users.AddAsync(user);

            // Act
            var result = await unitOfWork.Users.GetByIdAsync(user.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Homero Simpson", result.Name);
        }

        [Fact]
        public async Task User_AddAsync_ShouldReturnUserAdded()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var user = new User { Name = "Homero Simpson", Email = "homero@mail.com" };

            // Act
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            var users = await unitOfWork.Users.GetAllAsync();

            // Assert
            Assert.Single(users);
            Assert.Equal("Homero Simpson", users.First().Name);
        }

        [Fact]
        public async Task User_UpdateAsync_ShouldReturnUserUpdated()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var user = new User { Name = "Homero Simpson", Email = "homero@mail.com" };
            await unitOfWork.Users.AddAsync(user);

            // Act
            user.Name = "Homero J Simpson";
            await unitOfWork.Users.UpdateAsync(user);
            var updatedUser = await unitOfWork.Users.GetByIdAsync(user.Id);

            // Assert
            Assert.NotNull(updatedUser);
            Assert.Equal("Homero J Simpson", updatedUser!.Name);
        }

        [Fact]
        public async Task User_DeleteAsync_ShouldNotReturnUserDeleted()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var user = new User { Name = "Homero Simpson", Email = "homero@mail.com" };
            await unitOfWork.Users.AddAsync(user);

            // Act
            await unitOfWork.Users.DeleteAsync(user.Id);
            var users = await unitOfWork.Users.GetAllAsync();

            // Assert
            Assert.Empty(users);
        }

        // Asset.
        [Fact]
        public async Task Asset_GetAllAsync_ShouldReturnAssets()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var asset = new Asset { Name = "Casa de Homero", Type = "Casa", UserId = 1 };

            // Act
            await unitOfWork.Assets.AddAsync(asset);
            await unitOfWork.SaveChangesAsync();

            // Assert
            var assets = await unitOfWork.Assets.GetAllAsync();
            Assert.Single(assets);
            Assert.Equal("Casa de Homero", assets.First().Name);
        }

        [Fact]
        public async Task Asset_GetByIdAsync_ShouldReturnAsset()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var asset = new Asset { Name = "Auto de Homero", Type = "Auto", UserId = 1 };
            await unitOfWork.Assets.AddAsync(asset);

            // Act
            var result = await unitOfWork.Assets.GetByIdAsync(asset.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Auto de Homero", result.Name);
        }

        [Fact]
        public async Task Asset_AddAsync_ShouldReturnAssetAdded()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var asset = new Asset { Name = "Casa de Homero", Type = "Casa", UserId = 1 };

            // Act
            await unitOfWork.Assets.AddAsync(asset);
            await unitOfWork.SaveChangesAsync();

            var assets = await unitOfWork.Assets.GetAllAsync();

            // Assert
            Assert.Single(assets);
            Assert.Equal("Casa de Homero", assets.First().Name);
        }

        [Fact]
        public async Task Asset_UpdateAsync_ShouldReturnAssetUpdated()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var asset = new Asset { Name = "Casa de Homero", Type = "Casa", UserId = 1 };
            await unitOfWork.Assets.AddAsync(asset);

            // Act
            asset.Name = "Casa Rodante de Homero";
            await unitOfWork.Assets.UpdateAsync(asset);
            var updatedAsset = await unitOfWork.Assets.GetByIdAsync(asset.Id);

            // Assert
            Assert.NotNull(updatedAsset);
            Assert.Equal("Casa Rodante de Homero", updatedAsset!.Name);
        }

        [Fact]
        public async Task Asset_DeleteAsync_ShouldNotReturnAssetDeleted()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var asset = new Asset { Name = "Casa de Homero", Type = "Casa", UserId = 1 };
            await unitOfWork.Assets.AddAsync(asset);

            // Act
            await unitOfWork.Assets.DeleteAsync(asset.Id);
            var assets = await unitOfWork.Assets.GetAllAsync();

            // Assert
            Assert.Empty(assets);
        }

        // Home.
        [Fact]
        public async Task Home_GetAllAsync_ShouldReturnHomes()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var home = new Home { Name = "Casa de Homero", Type = "Casa", UserId = 1 };

            // Act
            await unitOfWork.Homes.AddAsync(home);
            await unitOfWork.SaveChangesAsync();

            // Assert
            var homes = await unitOfWork.Homes.GetAllAsync();
            Assert.Single(homes);
            Assert.Equal("Casa de Homero", homes.First().Name);
        }

        [Fact]
        public async Task Home_GetByIdAsync_ShouldReturnHome()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var home = new Home { Name = "Casa de Homero", Type = "Casa", UserId = 1 };
            await unitOfWork.Homes.AddAsync(home);

            // Act
            var result = await unitOfWork.Homes.GetByIdAsync(home.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Casa de Homero", result.Name);
        }

        [Fact]
        public async Task Home_AddAsync_ShouldReturnHomeAdded()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var home = new Home { Name = "Casa de Homero", Type = "Casa", UserId = 1 };

            // Act
            await unitOfWork.Homes.AddAsync(home);
            await unitOfWork.SaveChangesAsync();

            var homes = await unitOfWork.Homes.GetAllAsync();

            // Assert
            Assert.Single(homes);
            Assert.Equal("Casa de Homero", homes.First().Name);
        }

        [Fact]
        public async Task Home_UpdateAsync_ShouldReturnHomeUpdated()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var home = new Home { Name = "Casa de Homero", Type = "Casa", UserId = 1 };
            await unitOfWork.Homes.AddAsync(home);

            // Act
            home.Name = "Casa Rodante de Homero";
            await unitOfWork.Homes.UpdateAsync(home);
            var updatedHome = await unitOfWork.Homes.GetByIdAsync(home.Id);

            // Assert
            Assert.NotNull(updatedHome);
            Assert.Equal("Casa Rodante de Homero", updatedHome!.Name);
        }

        [Fact]
        public async Task Home_DeleteAsync_ShouldNotReturnHomeDeleted()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var home = new Home { Name = "Casa de Homero", Type = "Casa", UserId = 1 };
            await unitOfWork.Homes.AddAsync(home);

            // Act
            await unitOfWork.Homes.DeleteAsync(home.Id);
            var homes = await unitOfWork.Homes.GetAllAsync();

            // Assert
            Assert.Empty(homes);
        }

        // Vehicle
        [Fact]
        public async Task Vehicle_GetAllAsync_ShouldReturnVehicles()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var vehicle = new Vehicle { Name = "Auto de Homero", Type = "Auto", UserId = 1 };

            // Act
            await unitOfWork.Vehicles.AddAsync(vehicle);
            await unitOfWork.SaveChangesAsync();

            // Assert
            var vehicles = await unitOfWork.Vehicles.GetAllAsync();
            Assert.Single(vehicles);
            Assert.Equal("Auto de Homero", vehicles.First().Name);
        }

        [Fact]
        public async Task Vehicle_GetByIdAsync_ShouldReturnVehicle()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var vehicle = new Vehicle { Name = "Auto de Homero", Type = "Auto", UserId = 1 };
            await unitOfWork.Vehicles.AddAsync(vehicle);

            // Act
            var result = await unitOfWork.Vehicles.GetByIdAsync(vehicle.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Auto de Homero", result.Name);
        }

        [Fact]
        public async Task Vehicle_AddAsync_ShouldReturnVehicleAdded()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var vehicle = new Vehicle { Name = "Auto de Homero", Type = "Auto", UserId = 1 };

            // Act
            await unitOfWork.Vehicles.AddAsync(vehicle);
            await unitOfWork.SaveChangesAsync();

            var vehicles = await unitOfWork.Vehicles.GetAllAsync();

            // Assert
            Assert.Single(vehicles);
            Assert.Equal("Auto de Homero", vehicles.First().Name);
        }

        [Fact]
        public async Task Vehicle_UpdateAsync_ShouldReturnVehicleUpdated()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var vehicle = new Vehicle { Name = "Auto de Homero", Type = "Auto", UserId = 1 };
            await unitOfWork.Vehicles.AddAsync(vehicle);

            // Act
            vehicle.Name = "Moto de Homero";
            await unitOfWork.Vehicles.UpdateAsync(vehicle);
            var updatedHome = await unitOfWork.Vehicles.GetByIdAsync(vehicle.Id);

            // Assert
            Assert.NotNull(updatedHome);
            Assert.Equal("Moto de Homero", updatedHome.Name);
        }

        [Fact]
        public async Task Vehicle_DeleteAsync_ShouldNotReturnVehicleDeleted()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var vehicle = new Vehicle { Name = "Auto de Homero", Type = "Auto", UserId = 1 };
            await unitOfWork.Vehicles.AddAsync(vehicle);

            // Act
            await unitOfWork.Vehicles.DeleteAsync(vehicle.Id);
            var vehicles = await unitOfWork.Vehicles.GetAllAsync();

            // Assert
            Assert.Empty(vehicles);
        }

        // TaxOrService.
        [Fact]
        public async Task TaxOrService_GetAllAsync_ShouldReturnTaxesOrServices()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var taxOrService = new TaxOrService { ServiceName = "Aysa", ServiceType= "Agua", AssetId = 1 };

            // Act
            await unitOfWork.TaxesOrServices.AddAsync(taxOrService);
            await unitOfWork.SaveChangesAsync();

            // Assert
            var taxesOrServices = await unitOfWork.TaxesOrServices.GetAllAsync();
            Assert.Single(taxesOrServices);
            Assert.Equal("Aysa", taxesOrServices.First().ServiceName);
        }

        [Fact]
        public async Task TaxOrService_GetByIdAsync_ShouldReturnTaxeOrService()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var taxOrService = new TaxOrService { ServiceName = "Aysa", ServiceType = "Agua", AssetId = 1 };
            await unitOfWork.TaxesOrServices.AddAsync(taxOrService);

            // Act
            var result = await unitOfWork.TaxesOrServices.GetByIdAsync(taxOrService.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Aysa", result.ServiceName);
        }

        [Fact]
        public async Task TaxOrService_AddAsync_ShouldReturnTaxeOrServiceAdded()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var taxOrService = new TaxOrService { ServiceName = "Aysa", ServiceType = "Agua", AssetId = 1 };

            // Act
            await unitOfWork.TaxesOrServices.AddAsync(taxOrService);
            await unitOfWork.SaveChangesAsync();

            var taxesOrServices = await unitOfWork.TaxesOrServices.GetAllAsync();

            // Assert
            Assert.Single(taxesOrServices);
            Assert.Equal("Aysa", taxesOrServices.First().ServiceName);
        }

        [Fact]
        public async Task TaxOrService_UpdateAsync_ShouldReturnTaxeOrServiceUpdated()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var taxOrService = new TaxOrService { ServiceName = "Aysa", ServiceType = "Agua", AssetId = 1 };
            await unitOfWork.TaxesOrServices.AddAsync(taxOrService);

            // Act
            taxOrService.ServiceName = "Edesur";
            await unitOfWork.TaxesOrServices.UpdateAsync(taxOrService);
            var updatedTaxOrService = await unitOfWork.TaxesOrServices.GetByIdAsync(taxOrService.Id);

            // Assert
            Assert.NotNull(updatedTaxOrService);
            Assert.Equal("Edesur", updatedTaxOrService!.ServiceName);
        }

        [Fact]
        public async Task TaxOrService_DeleteAsync_ShouldNotReturnTaxeOrServiceDeleted()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var taxOrService = new TaxOrService {ServiceName = "Aysa", ServiceType = "Agua", AssetId = 1 };
            await unitOfWork.TaxesOrServices.AddAsync(taxOrService);

            // Act
            await unitOfWork.TaxesOrServices.DeleteAsync(taxOrService.Id);
            var taxesOrServices = await unitOfWork.TaxesOrServices.GetAllAsync();

            // Assert
            Assert.Empty(taxesOrServices);
        }

        // Invoice.
        [Fact]
        public async Task Invoice_GetAllAsync_ShouldReturnInvoices()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var invoice = new Invoice
            {
                InvoiceReceiptPath = "/invoices/Enero.pdf",
                PaymentStatus = false,
                TaxOrServiceId = 1,
                Month = "Enero"
            };


            // Act
            await unitOfWork.Invoices.AddAsync(invoice);
            await unitOfWork.SaveChangesAsync();

            // Assert
            var invoices = await unitOfWork.Invoices.GetAllAsync();
            Assert.Single(invoices);
            Assert.Equal("/invoices/Enero.pdf", invoices.First().InvoiceReceiptPath);
        }

        [Fact]
        public async Task Invoice_GetByIdAsync_ShouldReturnInvoice()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var invoice = new Invoice
            {
                InvoiceReceiptPath = "/invoices/Enero.pdf",
                PaymentStatus = false,
                TaxOrServiceId = 1,
                Month = "Enero"            
            };
            await unitOfWork.Invoices.AddAsync(invoice);

            // Act
            var result = await unitOfWork.Invoices.GetByIdAsync(invoice.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("/invoices/Enero.pdf", result.InvoiceReceiptPath);
        }

        [Fact]
        public async Task Invoice_AddAsync_ShouldReturnInvoiceAdded()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var invoice = new Invoice
            {
                InvoiceReceiptPath = "/invoices/Enero.pdf",
                PaymentStatus = false,
                TaxOrServiceId = 1,
                Month = "Enero"
            };

            // Act
            await unitOfWork.Invoices.AddAsync(invoice);
            await unitOfWork.SaveChangesAsync();

            var invoices = await unitOfWork.Invoices.GetAllAsync();

            // Assert
            Assert.Single(invoices);
            Assert.Equal("/invoices/Enero.pdf", invoices.First().InvoiceReceiptPath);
        }

        [Fact]
        public async Task Invoice_UpdateAsync_ShouldReturnInvoiceUpdated()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var invoice = new Invoice
            {
                InvoiceReceiptPath = "/invoices/Enero.pdf",
                PaymentStatus = false,
                TaxOrServiceId = 1,
                Month = "Enero"
            };

            await unitOfWork.Invoices.AddAsync(invoice);

            // Act
            invoice.PaymentStatus = true;
            await unitOfWork.Invoices.UpdateAsync(invoice);
            var updatedInvoice = await unitOfWork.Invoices.GetByIdAsync(invoice.Id);

            // Assert
            Assert.NotNull(updatedInvoice);
            Assert.True(updatedInvoice!.PaymentStatus);
        }

        [Fact]
        public async Task Invoice_DeleteAsync_ShouldNotReturnInvoiceDeleted()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var invoice = new Invoice
            {
                InvoiceReceiptPath = "/invoices/Enero.pdf",
                PaymentStatus = false,
                TaxOrServiceId = 1,
                Month = "Enero"
            };

            await unitOfWork.Invoices.AddAsync(invoice);

            // Act
            await unitOfWork.Invoices.DeleteAsync(invoice.Id);
            var invoices = await unitOfWork.Invoices.GetAllAsync();

            // Assert
            Assert.Empty(invoices);
        }
    }
}
