using AppCore.Models;
using AppCore.Repositories;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories;

public class EfParkingGateRepository(ParkingDbContext context) : EfGenericRepository<ParkingGate>(context.Gates), IParkingGateRepository
{
	public async Task<ParkingGate?> FindByGateNameAsync(string gateName)
	{
		return await Set.AsNoTracking().FirstOrDefaultAsync(pg => pg.Name == gateName);
	}
}


