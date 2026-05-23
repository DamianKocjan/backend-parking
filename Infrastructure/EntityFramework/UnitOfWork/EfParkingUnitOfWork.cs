using AppCore.Repositories;
using Infrastructure.EntityFramework.Context;

namespace Infrastructure.EntityFramework.UnitOfWork;

public class EfParkingUnitOfWork(
    IVehicleRepository vehicles,
    IParkingSessionRepository sessions,
    IParkingGateRepository gates,
    ICameraCaptureRepository captures,
    ParkingDbContext context
) : IParkingUnitOfWork, IAsyncDisposable
{
    public IVehicleRepository Vehicles => vehicles;
    public IParkingGateRepository Gates => gates;
    public IParkingSessionRepository Sessions => sessions;
    public ICameraCaptureRepository Captures => captures;

    public ValueTask DisposeAsync()
    {
        return context.DisposeAsync();
    }

    public Task<int> SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }

    public Task BeginTransactionAsync()
    {
        return context.Database.BeginTransactionAsync();
    }

    public Task CommitTransactionAsync()
    {
        return context.Database.CommitTransactionAsync();
    }

    public Task RollbackTransactionAsync()
    {
        return context.Database.RollbackTransactionAsync();
    }
}

