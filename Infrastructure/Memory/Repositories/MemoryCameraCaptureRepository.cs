using AppCore.Dtos;
using AppCore.Models;
using AppCore.Repositories;
using AppCore.ValueObjects;

namespace Infrastructure.Memory;

public class MemoryCameraCaptureRepository : MemoryGenericRepository<CameraCapture>, ICameraCaptureRepository
{
    public Task<PagedResult<CameraCapture>> FindByGateIdPagedAsync(Guid gateId, int pageNumber, int pageSize)
    {
        var results = _data.Values.Where(c => c.ParkingGateId == gateId).ToList();
        var pageResults = results.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return Task.FromResult(new PagedResult<CameraCapture>(pageResults, results.Count, pageNumber, pageSize));
    }
}

