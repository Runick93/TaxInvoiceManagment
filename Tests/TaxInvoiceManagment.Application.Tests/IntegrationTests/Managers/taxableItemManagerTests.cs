using TaxInvoiceManagment.Application.Dtos;
using TaxInvoiceManagment.Application.Managers;
using TaxInvoiceManagment.Application.Tests;
using TaxInvoiceManagment.Application.Validators;
using TaxInvoiceManagment.Domain.Models;
using TaxInvoiceManagment.Persistence.Managers;

public class TaxableItemManagerTests
{
    [Fact]
    public async Task GetAllTaxableItems_ShouldReturnAllTaxableItems()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var taxableItemManager = new TaxableItemManager(unitOfWork, new TaxableItemDtoValidator());

        var user = new User 
        {
            UserName = "Homero Simpson", 
            Email = "homero@mail.com", 
            Password = "Passw0rd!" 
        };
        await unitOfWork.Users.AddAsync(user);
        await unitOfWork.SaveChangesAsync();

        var taxableItem1 = new TaxableItemDto
        {
            Name = "Casa de Homero", 
            Type = "Casa", 
            UserId = 1 
        };
        await taxableItemManager.CreateTaxableItem(taxableItem1);

        var taxableItem2 = new TaxableItemDto 
        {
            Name = "Auto de Homero", 
            Type = "Auto", 
            UserId = 1 
        };
        await taxableItemManager.CreateTaxableItem(taxableItem2);

        // Act
        var result = await taxableItemManager.GetAllTaxableItems();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Count());
    }

    [Fact]
    public async Task GetTaxableItemById_ShouldReturnCorrectTaxableItem()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var taxableItemManager = new TaxableItemManager(unitOfWork, new TaxableItemDtoValidator());

        var user = new User 
        {
            UserName = "Homero Simpson", 
            Email = "homero@mail.com", 
            Password = "Passw0rd!" 
        };
        await unitOfWork.Users.AddAsync(user);
        await unitOfWork.SaveChangesAsync();

        var asset = new TaxableItemDto 
        { 
            Name = "Casa de Homero", 
            Type = "Casa", 
            UserId = 1 
        };
        var createdAsset = await taxableItemManager.CreateTaxableItem(asset);

        // Act
        var result = await taxableItemManager.GetTaxableItemById(createdAsset.Value.Id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Casa de Homero", result.Value.Name);
    }

    [Fact]
    public async Task GetTaxableItemById_ShouldReturnFailureForNonexistentTaxableItem()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var taxableItemManager = new TaxableItemManager(unitOfWork, new TaxableItemDtoValidator());

        // Act
        var result = await taxableItemManager.GetTaxableItemById(999);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("No se encontro el objeto imponible.", result.Errors);
    }

    [Fact]
    public async Task CreateTaxableItem_ShouldAddTaxableItemToDatabase()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var taxableItemManager = new TaxableItemManager(unitOfWork, new TaxableItemDtoValidator());

        var user = new User
        { 
            UserName = "Homero Simpson", 
            Email = "homero@mail.com", 
            Password = "Passw0rd!" 
        };
        await unitOfWork.Users.AddAsync(user);
        await unitOfWork.SaveChangesAsync();

        var TaxableItem = new TaxableItemDto
        { 
            Name = "Casa de Homero", 
            Type = "Casa", 
            UserId = 1 
        };

        // Act
        var result = await taxableItemManager.CreateTaxableItem(TaxableItem);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Casa de Homero", result.Value.Name);
    }

    [Fact]
    public async Task UpdateTaxableItem_ShouldModifyTaxableItemInDatabase()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var taxableItemManager = new TaxableItemManager(unitOfWork, new TaxableItemDtoValidator());

        var user = new User
        { 
            UserName = "Homero Simpson", 
            Email = "homero@mail.com", 
            Password = "Passw0rd!" 
        };
        await unitOfWork.Users.AddAsync(user);
        await unitOfWork.SaveChangesAsync();

        var TaxableItem = new TaxableItemDto 
        {
            Name = "Casa de Homero", 
            Type = "Casa", 
            UserId = 1 
        };
        var createdTaxableItem = await taxableItemManager.CreateTaxableItem(TaxableItem);

        // Act
        createdTaxableItem.Value.Name = "Casa rodante de Homero";
        var updateResult = await taxableItemManager.UpdateTaxableItem(createdTaxableItem.Value);

        // Assert
        Assert.True(updateResult.IsSuccess);
        var updatedTaxableItem = await taxableItemManager.GetTaxableItemById(createdTaxableItem.Value.Id);
        Assert.True(updatedTaxableItem.IsSuccess);
        Assert.Equal("Casa rodante de Homero", updatedTaxableItem.Value.Name);
    }

    [Fact]
    public async Task DeleteTaxableItem_ShouldRemoveTaxableItemFromDatabase()
    {
        // Arrange
        using var context = DbContextHelper.CreateSQLiteInMemoryDbContext();
        var unitOfWork = new UnitOfWork(context);
        var taxableItemManager = new TaxableItemManager(unitOfWork, new TaxableItemDtoValidator());

        var user = new User 
        {
            UserName = "Homero Simpson", 
            Email = "homero@mail.com", 
            Password = "Passw0rd!" 
        };
        await unitOfWork.Users.AddAsync(user);
        await unitOfWork.SaveChangesAsync();

        var TaxableItem = new TaxableItemDto
        {
            Name = "Moto de Homero", 
            Type = "Moto", 
            UserId = 1 
        };
        var createdTaxableItem = await taxableItemManager.CreateTaxableItem(TaxableItem);

        // Act
        var result = await taxableItemManager.DeleteTaxableItem(createdTaxableItem.Value.Id);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
