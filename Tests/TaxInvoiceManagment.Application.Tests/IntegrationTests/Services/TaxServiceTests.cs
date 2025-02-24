using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TaxInvoiceManagment.Application.Mappers;
using TaxInvoiceManagment.Application.Models.Dtos;
using TaxInvoiceManagment.Application.Services;
using TaxInvoiceManagment.Application.Tests.Helpers;
using TaxInvoiceManagment.Application.Validators;
using TaxInvoiceManagment.Domain.Entities;
using TaxInvoiceManagment.Persistence;

public class TaxServiceTests
{
    [Fact]
    public async Task GetAllTaxes_ShouldReturnAllTaxes()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var mockLogger = Mock.Of<ILogger<TaxService>>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TaxMappingProfile>();
        });
        var mapper = config.CreateMapper();

        var taxService = new TaxService(mockLogger, unitOfWork, mapper, new TaxDtoValidator());


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

        var tax1 = new TaxDto 
        { 
            Name = "Edesur" , 
            Owner = "Homero Simpson",
            ServiceType = "Electricidad", 
            TaxableItemId = 1 
        };
        await taxService.CreateTax(tax1);

        var tax2 = new TaxDto 
        {
            Name = "Aysa", 
            Owner = "Homero Simpson", 
            ServiceType = "Agua", 
            TaxableItemId = 1 
        };
        await taxService.CreateTax(tax2);

        // Act
        var result = await taxService.GetAllTaxes();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Count());
    }

    [Fact]
    public async Task GetTaxById_ShouldReturnCorrectTax()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var mockLogger = Mock.Of<ILogger<TaxService>>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TaxMappingProfile>();
        });
        var mapper = config.CreateMapper();

        var taxService = new TaxService(mockLogger, unitOfWork, mapper, new TaxDtoValidator());


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

        var tax = new TaxDto
        {
            Name = "Edesur", 
            Owner = "Homero Simpson", 
            ServiceType = "Electricidad", 
            TaxableItemId = 1
        };
        var createdTax = await taxService.CreateTax(tax);

        // Act
        var result = await taxService.GetTaxById(createdTax.Value.Id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Electricidad", result.Value.ServiceType);
    }

    [Fact]
    public async Task GetTaxById_ShouldReturnFailureForNonExistentTax()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var mockLogger = Mock.Of<ILogger<TaxService>>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TaxMappingProfile>();
        });
        var mapper = config.CreateMapper();

        var taxService = new TaxService(mockLogger, unitOfWork, mapper, new TaxDtoValidator());


        // Act
        var result = await taxService.GetTaxById(999);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("No se encontro el impuesto o servicio.", result.Errors);
    }

    [Fact]
    public async Task CreateTax_ShouldAddTaxToDatabase()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var mockLogger = Mock.Of<ILogger<TaxService>>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TaxMappingProfile>();
        });
        var mapper = config.CreateMapper();

        var taxService = new TaxService(mockLogger, unitOfWork, mapper, new TaxDtoValidator());


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

        var tax = new TaxDto   
        {
            Name = "Edesur", 
            Owner = "Homero Simpson", 
            ServiceType = "Electricidad", 
            TaxableItemId = 1 
        };

        // Act
        var result = await taxService.CreateTax(tax);

        // Assert
        Assert.True(result.IsSuccess);
        var taxesOrServices = await taxService.GetAllTaxes();
        Assert.Single(taxesOrServices.Value);
        Assert.Equal("Electricidad", taxesOrServices.Value.First().ServiceType);
    }

    [Fact]
    public async Task UpdateTax_ShouldModifyTaxInDatabase()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var mockLogger = Mock.Of<ILogger<TaxService>>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TaxMappingProfile>();
        });
        var mapper = config.CreateMapper();

        var taxService = new TaxService(mockLogger, unitOfWork, mapper, new TaxDtoValidator());


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

        var tax = new TaxDto
        {
            Name = "Edesur", 
            Owner = "Homero Simpson", 
            ServiceType = "Electricidad", 
            TaxableItemId = 1 
        };
        var createdTax = await taxService.CreateTax(tax);

        // Act
        createdTax.Value.ServiceType = "Agua";
        var updateResult = await taxService.UpdateTax(createdTax.Value);

        // Assert
        Assert.True(updateResult.IsSuccess);
        var updatedTax = await taxService.GetAllTaxes();
        Assert.True(updatedTax.IsSuccess);
        Assert.Equal("Agua", updatedTax.Value.First().ServiceType);
    }

    [Fact]
    public async Task DeleteTax_ShouldRemoveTaxFromDatabase()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var mockLogger = Mock.Of<ILogger<TaxService>>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TaxMappingProfile>();
        });
        var mapper = config.CreateMapper();

        var taxService = new TaxService(mockLogger, unitOfWork, mapper, new TaxDtoValidator());


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

        var tax = new TaxDto
        {
            Name = "Edesur", 
            Owner = "Homero Simpson", 
            ServiceType = "Electricidad", 
            TaxableItemId = 1 
        };
        var createdTax = await taxService.CreateTax(tax);

        // Act
        var result = await taxService.DeleteTax(createdTax.Value.Id);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
