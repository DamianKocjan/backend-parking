using AppCore.Dtos;
using AppCore.ValueObjects;

namespace AppCore.Services;

public interface IParkingGateService
{
    Task<ParkingGateDto?> GetById(Guid id);
    Task<ParkingGateDto?> GetByName(string name);
    Task<PagedResult<ParkingGateDto>> GetAllPaged(int page, int size);
    Task<ParkingGateDto> Create(CreateGateDto createGateDto);
    Task<ParkingGateDto?> UpdateOperationalStatus(Guid id, bool operationalStatus);
}