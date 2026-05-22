using AppCore.Dtos;
using AppCore.Services;

namespace Infrastructure.Memory.Services;

public class MemoryCameraCaptureService : ICameraCaptureService
{
    public Task<CameraCaptureDto> ProcessCaptureAsync(CameraCaptureWithGateDto captureDto)
    {
        // TODO: Implement file saving
        // Simulate processing the capture and returning a result
        var result = new CameraCaptureDto(
            LicensePlate: "ABC123",
            Brand: "Toyota",
            Color: "Red",
            GateName: captureDto.GateName,
            ImagePath: captureDto.ImagePath
        );

        return Task.FromResult(result);
    }

    public Task<bool> RemoveCaptureAsync(CameraCaptureDto captureDto)
    {
        // TODO: Implement logic for removing captures from file storage
        return Task.FromResult(true);
    }
}