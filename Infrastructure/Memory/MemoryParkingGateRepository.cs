using AppCore.Models;
using AppCore.Repositories;

namespace Infrastructure.Memory;

public class MemoryParkingGateRepository : MemoryGenericRepository<ParkingGate>, IParkingGateRepository
{
    public MemoryParkingGateRepository()
    {
        var gate = new ParkingGate()
        {
            Id = Guid.NewGuid(),
            Name = "Entry Gate",
            Type = GateType.Entry,
            Location = "Main Gate",
            IsOperational = false
        };
        _data.Add(gate.Id, gate);

        var gate2 = new ParkingGate()
        {
            Id = Guid.NewGuid(),
            Name = "Parking Gate",
            Type = GateType.Exit,
            Location = "Main Gate",
            IsOperational = false
        };
        _data.Add(gate2.Id, gate2);
    }
    
    public Task<ParkingGate?> FindByGateNameAsync(string gateName)
    {
        var result = _data.Values.ToList().Find((pg) => pg.Name == gateName);
        return Task.FromResult(result);
    }
}