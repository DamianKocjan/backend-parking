using AppCore.Models;

namespace AppCore.Repositories;

public interface IParkingSessionRepository : IGenericRepositoryAsync<ParkingSession>
{
    Task<ParkingSession?> FindByPlateNumberAsync(string plate);
    Task<IEnumerable<ParkingSession>> GetAllActiveAsync();
    Task<IEnumerable<ParkingSession>> GetVehicleHistoryByLicensePlateAsync(string licensePlate);
}