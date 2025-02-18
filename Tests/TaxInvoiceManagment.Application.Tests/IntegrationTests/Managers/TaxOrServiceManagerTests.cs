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

public class TaxOrServiceManagerTests
{
    [Fact]
    public async Task GetAllTaxesOrServices_ShouldReturnAllTaxesOrServices()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var mockLogger = Mock.Of<ILogger<TaxOrServiceManager>>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TaxOrServiceMappingProfile>();
        });
        var mapper = config.CreateMapper();

        var taxOrServiceManager = new TaxOrServiceManager(mockLogger, unitOfWork, mapper, new TaxOrServiceDtoValidator());


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
            UserId = 1
        };
        await unitOfWork.TaxableItems.AddAsync(taxableItem);
        await unitOfWork.SaveChangesAsync();

        var taxOrService1 = new TaxOrServiceDto 
        { 
            ServiceName = "Edesur" , 
            Owner = "Homero Simpson",
            ServiceType = "Luz", 
            TaxableItemId = 1 
        };
        await taxOrServiceManager.CreateTaxOrService(taxOrService1);

        var taxOrService2 = new TaxOrServiceDto 
        {
            ServiceName = "Aysa", 
            Owner = "Homero Simpson", 
            ServiceType = "Agua", 
            TaxableItemId = 1 
        };
        await taxOrServiceManager.CreateTaxOrService(taxOrService2);

        // Act
        var result = await taxOrServiceManager.GetAllTaxesOrServices();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Count());
    }

    [Fact]
    public async Task GetTaxOrServiceById_ShouldReturnCorrectTaxOrService()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var mockLogger = Mock.Of<ILogger<TaxOrServiceManager>>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TaxOrServiceMappingProfile>();
        });
        var mapper = config.CreateMapper();

        var taxOrServiceManager = new TaxOrServiceManager(mockLogger, unitOfWork, mapper, new TaxOrServiceDtoValidator());


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

        var taxOrService = new TaxOrServiceDto 
        {
            ServiceName = "Edesur", 
            Owner = "Homero Simpson", 
            ServiceType = "Luz", 
            TaxableItemId = 1
        };
        var createdTaxOrService = await taxOrServiceManager.CreateTaxOrService(taxOrService);

        // Act
        var result = await taxOrServiceManager.GetTaxOrServiceById(createdTaxOrService.Value.Id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Luz", result.Value.ServiceType);
    }

    [Fact]
    public async Task GetTaxOrServiceById_ShouldReturnFailureForNonexistentTaxOrService()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var mockLogger = Mock.Of<ILogger<TaxOrServiceManager>>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TaxOrServiceMappingProfile>();
        });
        var mapper = config.CreateMapper();

        var taxOrServiceManager = new TaxOrServiceManager(mockLogger, unitOfWork, mapper, new TaxOrServiceDtoValidator());


        // Act
        var result = await taxOrServiceManager.GetTaxOrServiceById(999);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("No se encontro el impuesto o servicio.", result.Errors);
    }

    [Fact]
    public async Task CreateTaxOrService_ShouldAddTaxOrServiceToDatabase()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var mockLogger = Mock.Of<ILogger<TaxOrServiceManager>>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TaxOrServiceMappingProfile>();
        });
        var mapper = config.CreateMapper();

        var taxOrServiceManager = new TaxOrServiceManager(mockLogger, unitOfWork, mapper, new TaxOrServiceDtoValidator());


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

        var taxOrService = new TaxOrServiceDto 
        {
            ServiceName = "Edesur", 
            Owner = "Homero Simpson", 
            ServiceType = "Luz", 
            TaxableItemId = 1 
        };

        // Act
        var result = await taxOrServiceManager.CreateTaxOrService(taxOrService);

        // Assert
        Assert.True(result.IsSuccess);
        var taxesOrServices = await taxOrServiceManager.GetAllTaxesOrServices();
        Assert.Single(taxesOrServices.Value);
        Assert.Equal("Luz", taxesOrServices.Value.First().ServiceType);
    }

    [Fact]
    public async Task UpdateTaxOrService_ShouldModifyTaxOrServiceInDatabase()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var mockLogger = Mock.Of<ILogger<TaxOrServiceManager>>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TaxOrServiceMappingProfile>();
        });
        var mapper = config.CreateMapper();

        var taxOrServiceManager = new TaxOrServiceManager(mockLogger, unitOfWork, mapper, new TaxOrServiceDtoValidator());


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

        var taxOrService = new TaxOrServiceDto
        {
            ServiceName = "Edesur", 
            Owner = "Homero Simpson", 
            ServiceType = "Luz", 
            TaxableItemId = 1 
        };
        var createdTaxOrService = await taxOrServiceManager.CreateTaxOrService(taxOrService);

        // Act
        createdTaxOrService.Value.ServiceType = "Agua";
        var updateResult = await taxOrServiceManager.UpdateTaxOrService(createdTaxOrService.Value);

        // Assert
        Assert.True(updateResult.IsSuccess);
        var updatedTaxOrService = await taxOrServiceManager.GetAllTaxesOrServices();
        Assert.True(updatedTaxOrService.IsSuccess);
        Assert.Equal("Agua", updatedTaxOrService.Value.First().ServiceType);
    }

    [Fact]
    public async Task DeleteTaxOrService_ShouldRemoveTaxOrServiceFromDatabase()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var mockLogger = Mock.Of<ILogger<TaxOrServiceManager>>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TaxOrServiceMappingProfile>();
        });
        var mapper = config.CreateMapper();

        var taxOrServiceManager = new TaxOrServiceManager(mockLogger, unitOfWork, mapper, new TaxOrServiceDtoValidator());


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

        var taxOrService = new TaxOrServiceDto 
        {
            ServiceName = "Edesur", 
            Owner = "Homero Simpson", 
            ServiceType = "Luz", 
            TaxableItemId = 1 
        };
        var createdTaxOrService = await taxOrServiceManager.CreateTaxOrService(taxOrService);

        // Act
        var result = await taxOrServiceManager.DeleteTaxOrService(createdTaxOrService.Value.Id);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
