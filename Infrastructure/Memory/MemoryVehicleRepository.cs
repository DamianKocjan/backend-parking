using AppCore.Models;
using AppCore.Repositories;

namespace Infrastructure.Memory;

public class MemoryVehicleRepository : MemoryGenericRepository<Vehicle>, IVehicleRepository
{
    public Task<Vehicle?> FindByPlateNumberAsync(string plate)
    {
        var result = _data.Values.ToList().Find(v => v.LicensePlate == plate);
        return Task.FromResult(result);
    }
}