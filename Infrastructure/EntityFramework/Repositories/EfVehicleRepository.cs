using AppCore.Models;
using AppCore.Repositories;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories;

public class EfVehicleRepository(ParkingDbContext context) : EfGenericRepository<Vehicle>(context.Vehicles), IVehicleRepository
{
	public async Task<Vehicle?> FindByPlateNumberAsync(string plate)
	{
		return await Set.AsNoTracking().FirstOrDefaultAsync(v => v.LicensePlate == plate);
	}
}


