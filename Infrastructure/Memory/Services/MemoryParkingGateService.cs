using AppCore.Dtos;
using AppCore.Repositories;
using AppCore.Services;
using AppCore.ValueObjects;

namespace Infrastructure.Memory.Services;

public class MemoryParkingGateService(IParkingUnitOfWork unit) : IParkingGateService
{
    public async Task<ParkingGateDto?> GetById(Guid id)
    {
        var entity = await unit.Gates.FindByIdAsync(id);
        if (entity is null)
        {
            return null;
        }

        return entity;
    }

    public async Task<ParkingGateDto?> GetByName(string name)
    {
        var entity = await unit.Gates.FindByGateNameAsync(name);
        if (entity is null)
        {
            return null;
        }

        return entity;
    }

    public async Task<PagedResult<ParkingGateDto>> GetAllPaged(int page, int size)
    {
        var entities = await unit.Gates.FindPagedAsync(page, size);
        
        return new PagedResult<ParkingGateDto>(
            entities.Items.Select(x => new ParkingGateDto(
                x.Id,
                x.Name,
                x.Type.ToString(),
                x.Location,
                x.IsOperational
            )).ToList(),
            entities.TotalCount,
            entities.Page,
            entities.PageSize
        );
    }

    public async Task<ParkingGateDto> Create(CreateGateDto createGateDto)
    {
        return await unit.Gates.AddAsync(createGateDto.ToEntity());
    }

    public async Task<ParkingGateDto?> UpdateOperationalStatus(Guid id, bool operationalStatus)
    {
        var entity = await unit.Gates.FindByIdAsync(id);
        if (entity is null)
        {
            return null;
        }
        
        entity.IsOperational = operationalStatus;
        await unit.Gates.UpdateAsync(entity);
        return entity;
    }
}