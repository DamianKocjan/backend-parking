using AppCore.Models;
using AppCore.Repositories;
using AppCore.ValueObjects;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories;

public class EfCameraCaptureRepository(ParkingDbContext context) : EfGenericRepository<CameraCapture>(context.Captures), ICameraCaptureRepository
{
    public async Task<PagedResult<CameraCapture>> FindByGateIdPagedAsync(Guid gateId, int pageNumber, int pageSize)
    {
        var query = Set
            .AsNoTracking()
            .Include(c => c.ParkingGate)
            .Where(c => c.ParkingGateId == gateId)
            .OrderByDescending(c => c.CapturedAt);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<CameraCapture>(items, totalCount, pageNumber, pageSize);
    }

    public async Task<CameraCapture?> FindByGateAndCaptureIdAsync(Guid gateId, Guid captureId)
    {
        return await Set
            .Include(c => c.ParkingGate)
            .FirstOrDefaultAsync(c => c.ParkingGateId == gateId && c.Id == captureId);
    }
}


