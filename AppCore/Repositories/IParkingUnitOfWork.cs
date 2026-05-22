namespace AppCore.Repositories;

public interface IParkingUnitOfWork
{
    IVehicleRepository Vehicles { get; }
    IParkingGateRepository Gates { get; }
    IParkingSessionRepository Sessions { get; }
    ICameraCaptureRepository Captures { get; }
    // pozostałe repozytoria

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}