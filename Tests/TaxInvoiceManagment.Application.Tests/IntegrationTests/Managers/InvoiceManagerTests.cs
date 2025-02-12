using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TaxInvoiceManagment.Application.Managers;
using TaxInvoiceManagment.Domain.Models;
using TaxInvoiceManagment.Persistence.Managers;

namespace TaxInvoiceManagment.Application.Tests.IntegrationTests.Managers
{
    public class InvoiceManagerTests
    {
        [Fact]
        public async Task GetAllInvoices_ShouldReturnAllInvoices()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var invoiceManager = new InvoiceManager(unitOfWork);

            var user = new User { UserName = "Homero Simpson", Email = "homero@mail.com", Password = "Passw0rd" };
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            var taxableItem = new TaxableItem { Name = "Casa de Homero", Type = "Casa", UserId = user.Id };
            await unitOfWork.TaxableItems.AddAsync(taxableItem);
            await unitOfWork.SaveChangesAsync();

            var tax = new TaxOrService { ServiceType = "Luz", TaxableItemId = taxableItem.Id };
            await unitOfWork.TaxesOrServices.AddAsync(tax);
            await unitOfWork.SaveChangesAsync();

            var invoice1 = new Invoice { Month = "Enero", InvoiceReceiptPath = "/invoices/enero.pdf", PaymentStatus = true, InvoiceAmount = 500.00m, PrimaryDueDate = DateTime.Now.AddDays(30), TaxOrServiceId = tax.Id };
            var invoice2 = new Invoice { Month = "Febrero", InvoiceReceiptPath = "/invoices/febrero.pdf", PaymentStatus = false, InvoiceAmount = 300.00m, PrimaryDueDate = DateTime.Now.AddDays(15), TaxOrServiceId = tax.Id };
            await invoiceManager.CreateInvoice(invoice1);
            await invoiceManager.CreateInvoice(invoice2);

            // Act
            var invoices = await invoiceManager.GetAllInvoices();

            // Assert
            Assert.Equal(2, invoices.Count);
        }

        [Fact]
        public async Task GetInvoiceById_ShouldReturnCorrectInvoice()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var invoiceManager = new InvoiceManager(unitOfWork);

            var user = new User { UserName = "Homero Simpson", Email = "homero@mail.com", Password = "Passw0rd" };
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            var taxableItem = new TaxableItem { Name = "Casa de Homero", Type = "Casa", UserId = user.Id };
            await unitOfWork.TaxableItems.AddAsync(taxableItem);
            await unitOfWork.SaveChangesAsync();

            var tax = new TaxOrService { ServiceType = "Luz", TaxableItemId = taxableItem.Id };
            await unitOfWork.TaxesOrServices.AddAsync(tax);
            await unitOfWork.SaveChangesAsync();

            var invoice = new Invoice { Month = "Enero", InvoiceReceiptPath = "/invoices/enero.pdf", PaymentStatus = true, InvoiceAmount = 500.00m, PrimaryDueDate = DateTime.Now.AddDays(30), TaxOrServiceId = tax.Id };
            await invoiceManager.CreateInvoice(invoice);

            // Act
            var result = await invoiceManager.GetInvoiceById(invoice.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("/invoices/enero.pdf", result.InvoiceReceiptPath);
        }

        [Fact]
        public async Task GetInvoiceById_ShouldThrowKeyNotFoundExceptionForNonexistentInvoice()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var invoiceManager = new InvoiceManager(unitOfWork);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => invoiceManager.GetInvoiceById(999));
        }

        [Fact]
        public async Task CreateInvoice_ShouldAddInvoiceToDatabase()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var invoiceManager = new InvoiceManager(unitOfWork);

            var user = new User { UserName = "Homero Simpson", Email = "homero@mail.com", Password = "Passw0rd" };
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            var taxableItem = new TaxableItem { Name = "Casa de Homero", Type = "Casa", UserId = user.Id };
            await unitOfWork.TaxableItems.AddAsync(taxableItem);
            await unitOfWork.SaveChangesAsync();

            var tax = new TaxOrService { ServiceType = "Luz", TaxableItemId = taxableItem.Id };
            await unitOfWork.TaxesOrServices.AddAsync(tax);
            await unitOfWork.SaveChangesAsync();

            var invoice = new Invoice { Month = "Enero", InvoiceReceiptPath = "/invoices/enero.pdf", PaymentStatus = true, InvoiceAmount = 500.00m, PrimaryDueDate = DateTime.Now.AddDays(30), TaxOrServiceId = tax.Id };

            // Act
            var result = await invoiceManager.CreateInvoice(invoice);

            // Assert
            Assert.True(result);
            var invoices = await invoiceManager.GetAllInvoices();
            Assert.Single(invoices);
            Assert.Equal("/invoices/enero.pdf", invoices.First().InvoiceReceiptPath);
        }

        [Fact]
        public async Task UpdateInvoice_ShouldModifyInvoiceInDatabase()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var invoiceManager = new InvoiceManager(unitOfWork);

            var user = new User { UserName = "Homero Simpson", Email = "homero@mail.com", Password = "Passw0rd" };
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            var taxableItem = new TaxableItem { Name = "Casa de Homero", Type = "Casa", UserId = user.Id };
            await unitOfWork.TaxableItems.AddAsync(taxableItem);
            await unitOfWork.SaveChangesAsync();

            var tax = new TaxOrService { ServiceType = "Luz", TaxableItemId = taxableItem.Id };
            await unitOfWork.TaxesOrServices.AddAsync(tax);
            await unitOfWork.SaveChangesAsync();

            var invoice = new Invoice { Month = "Enero", InvoiceReceiptPath = "/invoices/enero.pdf", PaymentStatus = true, InvoiceAmount = 500.00m, PrimaryDueDate = DateTime.Now.AddDays(30), TaxOrServiceId = tax.Id };
            await invoiceManager.CreateInvoice(invoice);

            // Act
            invoice.PaymentStatus = true;
            invoice.InvoiceAmount = 180.00m;
            var result = await invoiceManager.UpdateInvoice(invoice);

            // Assert
            Assert.True(result);
            var updatedInvoice = await invoiceManager.GetInvoiceById(invoice.Id);
            Assert.True(updatedInvoice.PaymentStatus);
            Assert.Equal(180.00m, updatedInvoice.InvoiceAmount);
        }

        [Fact]
        public async Task DeleteInvoice_ShouldRemoveInvoiceFromDatabase()
        {
            // Arrange
            using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
            var unitOfWork = new UnitOfWork(context);
            var invoiceManager = new InvoiceManager(unitOfWork);

            var user = new User { UserName = "Homero Simpson", Email = "homero@mail.com", Password = "Passw0rd" };
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            var taxableItem = new TaxableItem { Name = "Casa de Homero", Type = "Casa", UserId = user.Id };
            await unitOfWork.TaxableItems.AddAsync(taxableItem);
            await unitOfWork.SaveChangesAsync();

            var tax = new TaxOrService { ServiceType = "Luz", TaxableItemId = taxableItem.Id };
            await unitOfWork.TaxesOrServices.AddAsync(tax);
            await unitOfWork.SaveChangesAsync();

            var invoice = new Invoice { Month = "Enero", InvoiceReceiptPath = "/invoices/enero.pdf", PaymentStatus = true, InvoiceAmount = 500.00m, PrimaryDueDate = DateTime.Now.AddDays(30), TaxOrServiceId = tax.Id };
            await invoiceManager.CreateInvoice(invoice);

            // Act
            var result = await invoiceManager.DeleteInvoice(invoice.Id);

            // Assert
            Assert.True(result);
            var invoices = await invoiceManager.GetAllInvoices();
            Assert.Empty(invoices);
        }
    }
}
