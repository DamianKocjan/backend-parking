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

    public Task<IEnumerable<ParkingSession>> GetSessionsAsync(DateTime? startDate, DateTime? endDate, string? gateName, string? licensePlate)
    {
        var query = _data.Values.AsQueryable();

        if (startDate.HasValue)
            query = query.Where(ps => ps.EntryTime >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(ps => ps.EntryTime <= endDate.Value);

        if (!string.IsNullOrWhiteSpace(gateName))
            query = query.Where(ps => ps.GateName == gateName);

        if (!string.IsNullOrWhiteSpace(licensePlate))
            query = query.Where(ps => ps.Vehicle != null && ps.Vehicle.LicensePlate == licensePlate);

        return Task.FromResult(query.OrderByDescending(ps => ps.EntryTime).AsEnumerable());
    }
}