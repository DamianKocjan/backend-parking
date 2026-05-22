using AppCore.Repositories;

namespace Infrastructure.Memory;

public class MemoryParkingUnitOfWork(
    IVehicleRepository vehicles,
    IParkingSessionRepository sessions,
    IParkingGateRepository gates,
    ICameraCaptureRepository captures
): IParkingUnitOfWork
{
    public IVehicleRepository Vehicles => vehicles;
    public IParkingGateRepository Gates => gates;
    public IParkingSessionRepository Sessions => sessions;
    public ICameraCaptureRepository Captures => captures;
    
    public Task<int> SaveChangesAsync()
    {
        return Task.FromResult(0);
    }

    public Task BeginTransactionAsync()
    {
        return Task.CompletedTask;
    }

    public Task CommitTransactionAsync()
    {
        return Task.CompletedTask;
    }

    public Task RollbackTransactionAsync()
    {
        return Task.CompletedTask;
    }
}