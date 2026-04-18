using AppCore.Models;
using AppCore.Repositories;
using Infrastructure.Memory;

namespace UnitTest;

public class MemoryGenericRepositoryTest
{
    private IGenericRepositoryAsync<Vehicle> _repo = new MemoryGenericRepository<Vehicle>();
    
    [Fact]
    public async Task AddVehicleToRepositoryTestAsync()
    {
        // Arrange
        var expected = new Vehicle()
        {
            Id = Guid.NewGuid(),
            LicensePlate = "TK 8434Y",
            Brand = "Toyota",
            Color = "Black",
            ParkingSessions = [],
        };
        
        // Act
        await _repo.AddAsync(expected);
        
        // Assert
        var actual = await _repo.FindByIdAsync(expected.Id);
        Assert.NotNull(actual);
        Assert.Equal(expected, actual);
        Assert.Equal(expected.Id, actual?.Id);
    }

    [Fact]
    public async Task DeleteVehicleFromRepositoryTestAsync()
    {
        // Arrange
        var expected = new Vehicle()
        {
            Id = Guid.NewGuid(),
            LicensePlate = "TK 8434Y",
            Brand = "Toyota",
            Color = "Black",
            ParkingSessions = [],
        };
        
        await _repo.AddAsync(expected);
        var actual = await _repo.FindByIdAsync(expected.Id);
        Assert.NotNull(actual);
        
        // Act
        await _repo.RemoveByIdAsync(expected.Id);
        
        // Assert
        actual = await _repo.FindByIdAsync(expected.Id);
        Assert.Null(actual);
    }

    [Fact]
    public async Task EditVehicleToRepositoryTestAsync()
    {
        // Arrange
        var expected = new Vehicle()
        {
            Id = Guid.NewGuid(),
            LicensePlate = "TK 8434Y",
            Brand = "Toyota",
            Color = "Black",
            ParkingSessions = [],
        };
        await _repo.AddAsync(expected);
        expected = await _repo.FindByIdAsync(expected.Id);
        
        // Act
        expected.LicensePlate = "NEW PLATE";
        await _repo.UpdateAsync(expected);
        
        // Assert
        var actual = await _repo.FindByIdAsync(expected.Id);
        Assert.NotNull(actual);
        Assert.Equal(expected, actual);
        Assert.Equal(expected.Id, actual?.Id);
        Assert.Equal(expected.LicensePlate, actual.LicensePlate);
    }

    [Fact]
    public async Task FindVehicleFromRepositoryTestAsync()
    {
        // Arrange
        var expected = new Vehicle()
        {
            Id = Guid.NewGuid(),
            LicensePlate = "TK 8434Y",
            Brand = "Toyota",
            Color = "Black",
            ParkingSessions = [],
        };
        await _repo.AddAsync(expected);
        
        // Act
        var actual = await _repo.FindByIdAsync(expected.Id);
        
        // Assert
        Assert.NotNull(actual);
        Assert.Equal(expected, actual);
        Assert.Equal(expected.Id, actual?.Id);
    }

    [Fact]
    public async Task FindAllVehicleFromRepositoryTestAsync()
    {
        // Arrange
        var expected = new Vehicle()
        {
            Id = Guid.NewGuid(),
            LicensePlate = "TK 8434Y",
            Brand = "Toyota",
            Color = "Black",
            ParkingSessions = [],
        };
        await _repo.AddAsync(expected);
        
        // Act
        var actual = await _repo.FindAllAsync();
        
        // Assert
        Assert.NotNull(actual);
        Assert.Contains(expected, actual);
    }
    
    [Fact]
    public async Task FindPagedVehiclesFromRepositoryTestAsync()
    {
        // Arrange
        var expected = new Vehicle()
        {
            Id = Guid.NewGuid(),
            LicensePlate = "TK 8434Y",
            Brand = "Toyota",
            Color = "Black",
            ParkingSessions = [],
        };
        await _repo.AddAsync(expected);
        
        // Act
        var actual = await _repo.FindPagedAsync(1, 10);
        
        // Assert
        Assert.NotNull(actual);
        Assert.Contains(expected, actual.Items);
        Assert.Equal(1, actual.TotalCount);
        Assert.Equal(10, actual.PageSize);
    }
}