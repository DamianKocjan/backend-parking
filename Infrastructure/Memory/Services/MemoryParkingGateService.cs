using AppCore.Dtos;
using AppCore.Exceptions;
using AppCore.Models;
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
            throw new GateNotFoundException($"Gate with id={id} not found!");
        }

        return await Task.FromResult(new ParkingGateDto(
            entity.Id,
            entity.Name,
            entity.Type.ToString(),
            entity.Location,
            entity.IsOperational)
        );
    }

    public async Task<ParkingGateDto?> Update(Guid id, UpdateGateDto updateGateDto)
    {
        var entity = await unit.Gates.FindByIdAsync(id);
        if (entity is null)
        {
            throw new GateNotFoundException($"Gate with id={id} not found!");
        }
        
        entity.Name = updateGateDto.Name;
        entity.Type = Enum.Parse<GateType>(updateGateDto.Type);
        await unit.Gates.UpdateAsync(entity);
        return entity;
    }

    public async Task<ParkingGateDto?> GetByName(string name)
    {
        var entity = await unit.Gates.FindByGateNameAsync(name);
        if (entity is null)
        {
            throw new GateNotFoundException($"Gate with name={name} not found!");
        }

        return await Task.FromResult(new ParkingGateDto(
            entity.Id,
            entity.Name,
            entity.Type.ToString(),
            entity.Location,
            entity.IsOperational)
        );
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
            throw new GateNotFoundException($"Gate with id={id} not found!");
        }
        
        entity.IsOperational = operationalStatus;
        await unit.Gates.UpdateAsync(entity);
        return entity;
    }

    public async Task<CameraCaptureDto?> AddCapture(Guid id, CreateCameraCaptureDto dto)
    {
        var entity = await unit.Gates.FindByIdAsync(id);
        if (entity is null)
        {
            throw new GateNotFoundException($"Gate with id={id} not found!");
        }

        return await unit.Captures.AddAsync(dto.ToEntity());
    }
    
    public async Task<PagedResult<CameraCaptureDto>> GetCameraCaptures(Guid id, int page, int size)
    {
        var entities = await unit.Captures.FindByGateIdPagedAsync(id, page, size);
        
        return new PagedResult<CameraCaptureDto>(
            entities.Items.Select(x => new CameraCaptureDto(
                x.LicensePlate,
                x.DetectedBrand,
                x.DetectedColor,
                x.ParkingGate.Name,
                x.ImagePath
            )).ToList(),
            entities.TotalCount,
            entities.Page,
            entities.PageSize
        );
    }

    public async Task DeleteCapture(Guid id, Guid captureId)
    {
        var gate = await unit.Gates.FindByIdAsync(id);
        if (gate is null)
        {
            throw new GateNotFoundException($"Gate with id={id} not found!");
        }
        
        var capture = await unit.Captures.FindByIdAsync(captureId);
        if (capture is null)
        {
            throw new CaptureNotFoundException($"Capture with id={captureId} not found!");
        }
        
        await unit.Captures.RemoveByIdAsync(captureId);
    }
}