using TaxInvoiceManagment.Domain.Entities;
using TaxInvoiceManagment.Persistence.Tests.Helpers;

namespace TaxInvoiceManagment.Persistence.Tests.UnitTests
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

            var user = new User { UserName = "Homero Simpson", Email = "homero@mail.com", Password = "Passw0rd" };

            // Act
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            // Assert
            var users = await unitOfWork.Users.GetAllAsync();
            Assert.Single(users);
            Assert.Equal("Homero Simpson", users.First().UserName);
        }

        [Fact]
        public async Task User_GetByIdAsync_ShouldReturnUser()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var user = new User { UserName = "Homero Simpson", Email = "homero@mail.com", Password = "Passw0rd" };
            await unitOfWork.Users.AddAsync(user);

            // Act
            var result = await unitOfWork.Users.GetByIdAsync(user.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Homero Simpson", result.UserName);
        }

        [Fact]
        public async Task User_AddAsync_ShouldReturnUserAdded()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var user = new User { UserName = "Homero Simpson", Email = "homero@mail.com", Password = "Passw0rd" };

            // Act
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            var users = await unitOfWork.Users.GetAllAsync();

            // Assert
            Assert.Single(users);
            Assert.Equal("Homero Simpson", users.First().UserName);
        }

        [Fact]
        public async Task User_UpdateAsync_ShouldReturnUserUpdated()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var user = new User { UserName = "Homero Simpson", Email = "homero@mail.com", Password = "Passw0rd" };
            await unitOfWork.Users.AddAsync(user);

            // Act
            user.UserName = "Homero J Simpson";
            await unitOfWork.Users.UpdateAsync(user);
            var updatedUser = await unitOfWork.Users.GetByIdAsync(user.Id);

            // Assert
            Assert.NotNull(updatedUser);
            Assert.Equal("Homero J Simpson", updatedUser!.UserName);
        }

        [Fact]
        public async Task User_DeleteAsync_ShouldNotReturnUserDeleted()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var user = new User { UserName = "Homero Simpson", Email = "homero@mail.com", Password = "Passw0rd" };
            await unitOfWork.Users.AddAsync(user);

            // Act
            await unitOfWork.Users.DeleteAsync(user.Id);
            var users = await unitOfWork.Users.GetAllAsync();

            // Assert
            Assert.Empty(users);
        }

        // Taxable Item.
        [Fact]
        public async Task TaxableItem_GetAllAsync_ShouldReturnTaxableItems()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var taxableItem = new TaxableItem { Name = "Casa de Homero", Type = "Casa", UserId = 1 };

            // Act
            await unitOfWork.TaxableItems.AddAsync(taxableItem);
            await unitOfWork.SaveChangesAsync();

            // Assert
            var taxableItems = await unitOfWork.TaxableItems.GetAllAsync();
            Assert.Single(taxableItems);
            Assert.Equal("Casa de Homero", taxableItems.First().Name);
        }

        [Fact]
        public async Task TaxableItem_GetByIdAsync_ShouldReturnTaxableItem()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var taxableItem = new TaxableItem { Name = "Auto de Homero", Type = "Auto", UserId = 1 };
            await unitOfWork.TaxableItems.AddAsync(taxableItem);

            // Act
            var result = await unitOfWork.TaxableItems.GetByIdAsync(taxableItem.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Auto de Homero", result.Name);
        }

        [Fact]
        public async Task TaxableItemAddAsync_ShouldReturnTaxableItemAdded()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var taxableItem = new TaxableItem { Name = "Casa de Homero", Type = "Casa", UserId = 1 };

            // Act
            await unitOfWork.TaxableItems.AddAsync(taxableItem);
            await unitOfWork.SaveChangesAsync();

            var taxableItems = await unitOfWork.TaxableItems.GetAllAsync();

            // Assert
            Assert.Single(taxableItems);
            Assert.Equal("Casa de Homero", taxableItems.First().Name);
        }

        [Fact]
        public async Task TaxableItem_UpdateAsync_ShouldReturnTaxableItemUpdated()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var taxableItem = new TaxableItem { Name = "Casa de Homero", Type = "Casa", UserId = 1 };
            await unitOfWork.TaxableItems.AddAsync(taxableItem);

            // Act
            taxableItem.Name = "Casa Rodante de Homero";
            await unitOfWork.TaxableItems.UpdateAsync(taxableItem);
            var updatedtaxableItem = await unitOfWork.TaxableItems.GetByIdAsync(taxableItem.Id);

            // Assert
            Assert.NotNull(updatedtaxableItem);
            Assert.Equal("Casa Rodante de Homero", updatedtaxableItem!.Name);
        }

        [Fact]
        public async Task TaxableItem_DeleteAsync_ShouldNotReturnTaxableItemDeleted()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var taxableItem = new TaxableItem { Name = "Casa de Homero", Type = "Casa", UserId = 1 };
            await unitOfWork.TaxableItems.AddAsync(taxableItem);

            // Act
            await unitOfWork.TaxableItems.DeleteAsync(taxableItem.Id);
            var taxableItems = await unitOfWork.TaxableItems.GetAllAsync();

            // Assert
            Assert.Empty(taxableItems);
        }

        // Tax.
        [Fact]
        public async Task Tax_GetAllAsync_ShouldReturnTaxes()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var tax = new Tax { Name = "Aysa", ServiceType = "Agua", TaxableItemId = 1 };

            // Act
            await unitOfWork.Taxes.AddAsync(tax);
            await unitOfWork.SaveChangesAsync();

            // Assert
            var taxes = await unitOfWork.Taxes.GetAllAsync();
            Assert.Single(taxes);
            Assert.Equal("Aysa", taxes.First().Name);
        }

        [Fact]
        public async Task Tax_GetByIdAsync_ShouldReturnTax()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var tax = new Tax { Name = "Aysa", ServiceType = "Agua", TaxableItemId = 1 };
            await unitOfWork.Taxes.AddAsync(tax);

            // Act
            var result = await unitOfWork.Taxes.GetByIdAsync(tax.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Aysa", result.Name);
        }

        [Fact]
        public async Task Tax_AddAsync_ShouldReturnTaxAdded()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var tax = new Tax { Name = "Aysa", ServiceType = "Agua", TaxableItemId = 1 };

            // Act
            await unitOfWork.Taxes.AddAsync(tax);
            await unitOfWork.SaveChangesAsync();

            var taxes = await unitOfWork.Taxes.GetAllAsync();

            // Assert
            Assert.Single(taxes);
            Assert.Equal("Aysa", taxes.First().Name);
        }

        [Fact]
        public async Task Tax_UpdateAsync_ShouldReturnTaxUpdated()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var tax = new Tax { Name = "Aysa", ServiceType = "Agua", TaxableItemId = 1 };
            await unitOfWork.Taxes.AddAsync(tax);

            // Act
            tax.Name = "Edesur";
            await unitOfWork.Taxes.UpdateAsync(tax);
            var updatedTax = await unitOfWork.Taxes.GetByIdAsync(tax.Id);

            // Assert
            Assert.NotNull(updatedTax);
            Assert.Equal("Edesur", updatedTax!.Name);
        }

        [Fact]
        public async Task Tax_DeleteAsync_ShouldNotReturnTaxDeleted()
        {
            // Arrange
            using var context = DbContextHelper.CreateInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);

            var tax = new Tax { Name = "Aysa", ServiceType = "Agua", TaxableItemId = 1 };
            await unitOfWork.Taxes.AddAsync(tax);

            // Act
            await unitOfWork.Taxes.DeleteAsync(tax.Id);
            var taxes = await unitOfWork.Taxes.GetAllAsync();

            // Assert
            Assert.Empty(taxes);
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
                TaxId = 1,
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
                TaxId = 1,
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
                TaxId = 1,
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
                TaxId = 1,
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
                TaxId = 1,
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
