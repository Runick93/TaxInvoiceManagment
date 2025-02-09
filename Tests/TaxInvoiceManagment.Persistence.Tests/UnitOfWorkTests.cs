using TaxInvoiceManagment.Domain.Models;
using TaxInvoiceManagment.Persistence.Managers;

namespace TaxInvoiceManagment.Persistence.Tests
{
    public class UnitOfWorkTests
    {
        // User.
        [Fact]
        public async Task UnitOfWork_ShouldHandleUserGetAllAsync()
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
        public async Task UnitOfWork_ShouldHandleUserGetByIdAsync()
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
        public async Task UnitOfWork_ShouldHandleUserAddAsync()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var user = new User { Name = "Homero Simpson", Email = "homero@mail.com" };

            // Act
            await unitOfWork.Users.AddAsync(user);
            var users = await unitOfWork.Users.GetAllAsync();

            // Assert
            Assert.Single(users);
            Assert.Equal("Homero Simpson", users.First().Name);
        }

        [Fact]
        public async Task UnitOfWork_ShouldHandleUserUpdateAsync()
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
        public async Task UnitOfWork_ShouldHandleUserDeleteAsync()
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
        public async Task UnitOfWork_ShouldHandleAssetGetAllAsync()
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
        public async Task UnitOfWork_ShouldHandleAssetGetByIdAsync()
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
        public async Task UnitOfWork_ShouldHandleAssetAddAsync()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var asset = new Asset { Name = "Casa de Homero", Type = "Casa", UserId = 1 };

            // Act
            await unitOfWork.Assets.AddAsync(asset);
            var assets = await unitOfWork.Assets.GetAllAsync();

            // Assert
            Assert.Single(assets);
            Assert.Equal("Casa de Homero", assets.First().Name);
        }

        [Fact]
        public async Task UnitOfWork_ShouldHandleAssetUpdateAsync()
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
        public async Task UnitOfWork_ShouldHandleAssetDeleteAsync()
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

        // TaxOrService.
        [Fact]
        public async Task UnitOfWork_ShouldHandleTaxOrServiceGetAllAsync()
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
        public async Task UnitOfWork_ShouldHandleTaxOrServiceGetByIdAsync()
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
        public async Task UnitOfWork_ShouldHandleTaxOrServiceAddAsync()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var taxOrService = new TaxOrService { ServiceName = "Aysa", ServiceType = "Agua", AssetId = 1 };

            // Act
            await unitOfWork.TaxesOrServices.AddAsync(taxOrService);
            var taxesOrServices = await unitOfWork.TaxesOrServices.GetAllAsync();

            // Assert
            Assert.Single(taxesOrServices);
            Assert.Equal("Aysa", taxesOrServices.First().ServiceName);
        }

        [Fact]
        public async Task UnitOfWork_ShouldHandleTaxOrServiceUpdateAsync()
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
        public async Task UnitOfWork_ShouldHandleTaxOrServiceDeleteAsync()
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
        public async Task UnitOfWork_ShouldHandleInvoiceGetAllAsync()
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
        public async Task UnitOfWork_ShouldHandleInvoiceGetByIdAsync()
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
        public async Task UnitOfWork_ShouldHandleInvoiceAddAsync()
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
            var invoices = await unitOfWork.Invoices.GetAllAsync();

            // Assert
            Assert.Single(invoices);
            Assert.Equal("/invoices/Enero.pdf", invoices.First().InvoiceReceiptPath);
        }

        [Fact]
        public async Task UnitOfWork_ShouldHandleInvoiceUpdateAsync()
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
        public async Task UnitOfWork_ShouldHandleInvoiceDeleteAsync()
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
