using AppCore.Dtos;
using AppCore.Exceptions;
using AppCore.Models;
using AppCore.Repositories;
using AppCore.Services;

namespace Infrastructure.Services;

public class CameraCaptureService(IParkingUnitOfWork unit) : ICameraCaptureService
{
    public async Task<CameraCaptureDto?> GetById(Guid id)
    {
        var entity = await unit.Captures.FindByIdAsync(id);
        if (entity is null)
        {
            throw new CaptureNotFoundException($"Capture with id={id} not found!");
        }

        return await Task.FromResult(new CameraCaptureDto(
            LicensePlate: entity.LicensePlate,
            Brand: entity.DetectedBrand,
            Color: entity.DetectedColor,
            GateName: entity.ParkingGate.Name,
            ImagePath: entity.ImagePath
        ));
    }

    public async Task<CameraCaptureDto> ProcessCaptureAsync(CameraCaptureWithGateDto captureDto)
    {
        var parkingGate = await unit.Gates.FindByGateNameAsync(captureDto.GateName);
        if (parkingGate is null)
        {
            throw new GateNotFoundException($"Gate with name={captureDto.GateName} not found!");
        }
     
        // TODO: Implement file saving
        // Simulate processing the capture and returning a result   
        return await unit.Captures.AddAsync(new CameraCapture()
        {
            LicensePlate = "ABC123",
            DetectedBrand = "Toyota",
            DetectedColor = "Red",
            ParkingGate = parkingGate,
            ImagePath = captureDto.ImagePath,
        });
    }

    public async Task RemoveCaptureAsync(Guid id)
    {
        var entity = await unit.Captures.FindByIdAsync(id);
        if (entity is null)
        {
            throw new CaptureNotFoundException($"Capture with id={id} not found!");
        }
        
        // TODO: Implement logic for removing captures from file storage
        await unit.Captures.RemoveByIdAsync(id);
    }
}