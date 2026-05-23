using AppCore.Models;
using AppCore.Repositories;

namespace Infrastructure.Memory;

public class MemoryParkingGateRepository : MemoryGenericRepository<ParkingGate>, IParkingGateRepository
{
    public MemoryParkingGateRepository()
    {
        var gate = new ParkingGate()
        {
            Id = new Guid("8fbd7b91-5c7f-4d85-8a33-22193b2ef718"),
            Name = "Entry Gate",
            Type = GateType.Entry,
            Location = "Main Gate",
            IsOperational = false
        };
        _data.Add(gate.Id, gate);

        var gate2 = new ParkingGate()
        {
            Id = new Guid("cce6ee16-89fb-462b-9dff-969b39e847e6"),
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