using AppCore.Models;
using AppCore.Repositories;

namespace Infrastructure.Memory;

public class MemoryParkingGateRepository : MemoryGenericRepository<ParkingGate>, IParkingGateRepository
{
    public Task<ParkingGate?> FindByGateNameAsync(string gateName)
    {
        var result = _data.Values.ToList().Find((pg) => pg.Name == gateName);
        return Task.FromResult(result);
    }
}