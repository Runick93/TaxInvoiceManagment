using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TaxInvoiceManagment.Application.Automapper;
using TaxInvoiceManagment.Application.Dtos;
using TaxInvoiceManagment.Application.Managers;
using TaxInvoiceManagment.Application.Tests;
using TaxInvoiceManagment.Application.Validators;
using TaxInvoiceManagment.Domain.Models;
using TaxInvoiceManagment.Persistence.Managers;

public class InvoiceManagerTests
{
    [Fact]
    public async Task GetAllInvoices_ShouldReturnAllInvoices()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var mockLogger = Mock.Of<ILogger<InvoiceManager>>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<InvoiceMappingProfile>();
        });
        var mapper = config.CreateMapper();

        var invoiceManager = new InvoiceManager(mockLogger, unitOfWork, mapper, new InvoiceDtoValidator());

        var user = new User 
        {
            UserName = "Homero Simpson", 
            Email = "homero@mail.com", 
            Password = "Passw0rd!" 
        };
        await unitOfWork.Users.AddAsync(user);
        await unitOfWork.SaveChangesAsync();

        var taxableItem = new TaxableItem 
        {
            Name = "Casa de Homero", 
            Type = "Casa", 
            UserId = user.Id 
        };
        await unitOfWork.TaxableItems.AddAsync(taxableItem);
        await unitOfWork.SaveChangesAsync();

        var taxOrService = new TaxOrService 
        { 
            ServiceName = "Edesur", 
            Owner = "Homero Simpson", 
            ServiceType = "Luz", 
            TaxableItemId = 1 
        };
        await unitOfWork.TaxesOrServices.AddAsync(taxOrService);
        await unitOfWork.SaveChangesAsync();

        var invoice1 = new InvoiceDto 
        {
            Number = 1 ,
            Month = "Enero", 
            InvoiceReceiptPath = "/invoices/enero.pdf",
            PaymentStatus = true, 
            InvoiceAmount = 500.00m,
            PrimaryDueDate = DateTime.Now.AddDays(30), 
            TaxOrServiceId = 1
        };
        await invoiceManager.CreateInvoice(invoice1);

        var invoice2 = new InvoiceDto 
        { 
            Number = 2, 
            Month = "Febrero", 
            InvoiceReceiptPath = "/invoices/febrero.pdf", 
            PaymentStatus = false, 
            InvoiceAmount = 300.00m, 
            PrimaryDueDate = DateTime.Now.AddDays(15), 
            TaxOrServiceId = 1 
        };       
        await invoiceManager.CreateInvoice(invoice2);

        // Act
        var result = await invoiceManager.GetAllInvoices();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Count());
    }

    [Fact]
    public async Task GetInvoiceById_ShouldReturnCorrectInvoice()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var mockLogger = Mock.Of<ILogger<InvoiceManager>>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<InvoiceMappingProfile>();
        });
        var mapper = config.CreateMapper();

        var invoiceManager = new InvoiceManager(mockLogger, unitOfWork, mapper, new InvoiceDtoValidator());

        var user = new User 
        { 
            UserName = "Homero Simpson", 
            Email = "homero@mail.com",
            Password = "Passw0rd!"
        };
        await unitOfWork.Users.AddAsync(user);
        await unitOfWork.SaveChangesAsync();

        var taxableItem = new TaxableItem 
        {
            Name = "Casa de Homero", 
            Type = "Casa",
            UserId = user.Id 
        };
        await unitOfWork.TaxableItems.AddAsync(taxableItem);
        await unitOfWork.SaveChangesAsync();

        var taxOrService = new TaxOrService 
        { 
            ServiceName = "Edesur", 
            Owner = "Homero Simpson", 
            ServiceType = "Luz", 
            TaxableItemId = 1 
        };
        await unitOfWork.TaxesOrServices.AddAsync(taxOrService);
        await unitOfWork.SaveChangesAsync();

        var invoice = new InvoiceDto 
        {
            Number = 1,
            Month = "Enero", 
            InvoiceReceiptPath = "/invoices/enero.pdf", 
            PaymentStatus = true, 
            InvoiceAmount = 500.00m, 
            PrimaryDueDate = DateTime.Now.AddDays(30), 
            TaxOrServiceId = 1 
        };
        var createdInvoice = await invoiceManager.CreateInvoice(invoice);

        // Act
        var result = await invoiceManager.GetInvoiceById(1);

        // Assert
        Assert.True(result.IsSuccess);
        //Assert.Equal("/invoices/enero.pdf", result.Value.InvoiceReceiptPath);
    }

    [Fact]
    public async Task GetInvoiceById_ShouldReturnFailureForNonexistentInvoice()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var mockLogger = Mock.Of<ILogger<InvoiceManager>>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<InvoiceMappingProfile>();
        });
        var mapper = config.CreateMapper();

        var invoiceManager = new InvoiceManager(mockLogger, unitOfWork, mapper, new InvoiceDtoValidator());


        // Act
        var result = await invoiceManager.GetInvoiceById(999);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("No se encontro la factura.", result.Errors);
    }

    [Fact]
    public async Task CreateInvoice_ShouldAddInvoiceToDatabase()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var mockLogger = Mock.Of<ILogger<InvoiceManager>>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<InvoiceMappingProfile>();
        });
        var mapper = config.CreateMapper();

        var invoiceManager = new InvoiceManager(mockLogger, unitOfWork, mapper, new InvoiceDtoValidator());


        var user = new User 
        { 
            UserName = "Homero Simpson", 
            Email = "homero@mail.com", 
            Password = "Passw0rd!" };
        await unitOfWork.Users.AddAsync(user);
        await unitOfWork.SaveChangesAsync();

        var taxableItem = new TaxableItem 
        { 
            Name = "Casa de Homero",
            Type = "Casa", 
            UserId = user.Id 
        };
        await unitOfWork.TaxableItems.AddAsync(taxableItem);
        await unitOfWork.SaveChangesAsync();

        var taxOrService = new TaxOrService 
        { 
            ServiceName = "Edesur",
            Owner = "Homero Simpson", 
            ServiceType = "Luz", 
            TaxableItemId = 1 
        };
        await unitOfWork.TaxesOrServices.AddAsync(taxOrService);
        await unitOfWork.SaveChangesAsync();

        var invoice = new InvoiceDto {
            Number = 1, 
            Month = "Enero", 
            InvoiceReceiptPath = "/invoices/enero.pdf", 
            PaymentStatus = true, 
            InvoiceAmount = 500.00m, 
            PrimaryDueDate = DateTime.Now.AddDays(30), 
            TaxOrServiceId = 1 
        };

        // Act
        var result = await invoiceManager.CreateInvoice(invoice);

        // Assert
        Assert.True(result.IsSuccess);
        var invoices = await invoiceManager.GetAllInvoices();
        Assert.Equal("/invoices/enero.pdf", invoices.Value.First().InvoiceReceiptPath);
    }

    [Fact]
    public async Task UpdateInvoice_ShouldModifyInvoiceInDatabase()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);

        var mockLogger = Mock.Of<ILogger<InvoiceManager>>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<InvoiceMappingProfile>();
        });
        var mapper = config.CreateMapper();

        var invoiceManager = new InvoiceManager(mockLogger, unitOfWork,mapper, new InvoiceDtoValidator());

        var user = new User
        { 
            UserName = "Homero Simpson", 
            Email = "homero@mail.com", 
            Password = "Passw0rd!" 
        };
        await unitOfWork.Users.AddAsync(user);
        await unitOfWork.SaveChangesAsync();

        var taxableItem = new TaxableItem 
        { 
            Name = "Casa de Homero", 
            Type = "Casa", 
            UserId = user.Id
        };
        await unitOfWork.TaxableItems.AddAsync(taxableItem);
        await unitOfWork.SaveChangesAsync();

        var taxOrService = new TaxOrService 
        { 
            ServiceName = "Edesur", 
            Owner = "Homero Simpson", 
            ServiceType = "Luz", 
            TaxableItemId = 1 
        };
        await unitOfWork.TaxesOrServices.AddAsync(taxOrService);
        await unitOfWork.SaveChangesAsync();

        var invoice = new InvoiceDto 
        {
            Number = 1, 
            Month = "Enero", 
            InvoiceReceiptPath = "/invoices/enero.pdf", 
            PaymentStatus = false, 
            InvoiceAmount = 500.00m, 
            PrimaryDueDate = DateTime.Now.AddDays(30), 
            TaxOrServiceId = 1 
        };
        var createdInvoice = await invoiceManager.CreateInvoice(invoice);

        // Act
        createdInvoice.Value.PaymentStatus = true;
        createdInvoice.Value.Month = "Febrero";
        var updateResult = await invoiceManager.UpdateInvoice(createdInvoice.Value);

        // Assert
        Assert.True(updateResult.IsSuccess);
        var updatedInvoice = await invoiceManager.GetAllInvoices();
        Assert.True(updatedInvoice.IsSuccess);
        Assert.Equal("Febrero", updatedInvoice.Value.First().Month);
        Assert.Equal(true, updatedInvoice.Value.First().PaymentStatus);
    }

    [Fact]
    public async Task DeleteInvoice_ShouldRemoveInvoiceFromDatabase()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var mockLogger = Mock.Of<ILogger<InvoiceManager>>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<InvoiceMappingProfile>();
        });
        var mapper = config.CreateMapper();

        var invoiceManager = new InvoiceManager(mockLogger, unitOfWork, mapper, new InvoiceDtoValidator());


        var user = new User 
        { 
            UserName = "Homero Simpson",
            Email = "homero@mail.com",
            Password = "Passw0rd!" 
        };
        await unitOfWork.Users.AddAsync(user);
        await unitOfWork.SaveChangesAsync();

        var taxableItem = new TaxableItem 
        { 
            Name = "Casa de Homero", 
            Type = "Casa",
            UserId = user.Id 
        };
        await unitOfWork.TaxableItems.AddAsync(taxableItem);
        await unitOfWork.SaveChangesAsync();

        var taxOrService = new TaxOrService 
        { 
            ServiceName = "Edesur", 
            Owner = "Homero Simpson",
            ServiceType = "Luz", 
            TaxableItemId = 1
        };
        await unitOfWork.TaxesOrServices.AddAsync(taxOrService);
        await unitOfWork.SaveChangesAsync();

        var invoice = new InvoiceDto 
        {
            Number = 1, 
            Month = "Enero", 
            InvoiceReceiptPath = "/invoices/enero.pdf", 
            PaymentStatus = true, 
            InvoiceAmount = 500.00m, 
            PrimaryDueDate = DateTime.Now.AddDays(30), 
            TaxOrServiceId = 1 
        };
        var createdInvoice = await invoiceManager.CreateInvoice(invoice);

        // Act
        var result = await invoiceManager.DeleteInvoice(createdInvoice.Value.Id);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
