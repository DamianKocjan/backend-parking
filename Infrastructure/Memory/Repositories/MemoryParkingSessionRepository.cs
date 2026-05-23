using AppCore.Models;
using AppCore.Repositories;

namespace Infrastructure.Memory;

public class MemoryParkingSessionRepository : MemoryGenericRepository<ParkingSession>, IParkingSessionRepository
{
    public Task<ParkingSession?> FindByPlateNumberAsync(string plate)
    {
        var result = _data.Values.ToList().Find(ps => ps.Vehicle.LicensePlate == plate);
        return Task.FromResult(result);
    }

    public Task<IEnumerable<ParkingSession>> GetAllActiveAsync()
    {
        var result = _data.Values.ToList().Where(ps => ps.IsActive);
        return Task.FromResult(result);
    }

    public Task<IEnumerable<ParkingSession>> GetVehicleHistoryByLicensePlateAsync(string licensePlate)
    {
        var result = _data.Values.ToList().Where(ps => ps.Vehicle.LicensePlate == licensePlate);
        return Task.FromResult(result);
    }
}