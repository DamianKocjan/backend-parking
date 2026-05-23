using AppCore.Models;
using AppCore.Repositories;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories;

public class EfParkingSessionRepository(ParkingDbContext context) : EfGenericRepository<ParkingSession>(context.Sessions), IParkingSessionRepository
{
	public async Task<ParkingSession?> FindByPlateNumberAsync(string plate)
	{
		return await Set
			.AsNoTracking()
			.Include(ps => ps.Vehicle)
			.OrderByDescending(ps => ps.EntryTime)
			.FirstOrDefaultAsync(ps => ps.Vehicle.LicensePlate == plate);
	}

	public async Task<IEnumerable<ParkingSession>> GetAllActiveAsync()
	{
		return await Set
			.AsNoTracking()
			.Include(ps => ps.Vehicle)
			.Where(ps => ps.IsActive)
			.OrderByDescending(ps => ps.EntryTime)
			.ToListAsync();
	}

	public async Task<IEnumerable<ParkingSession>> GetVehicleHistoryByLicensePlateAsync(string licensePlate)
	{
		return await Set
			.AsNoTracking()
			.Include(ps => ps.Vehicle)
			.Where(ps => ps.Vehicle.LicensePlate == licensePlate)
			.OrderByDescending(ps => ps.EntryTime)
			.ToListAsync();
	}
}


