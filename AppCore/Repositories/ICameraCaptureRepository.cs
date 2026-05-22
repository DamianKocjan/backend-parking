using AppCore.Models;
using AppCore.ValueObjects;

namespace AppCore.Repositories;

public interface ICameraCaptureRepository : IGenericRepositoryAsync<CameraCapture>
{
    Task<PagedResult<CameraCapture>> FindByGateIdPagedAsync(Guid gateId, int pageNumber, int pageSize);
}