using AppCore.Dtos;

namespace AppCore.Services;

public interface ICameraCaptureService
{
    Task<CameraCaptureDto?> GetById(Guid id);
    Task<CameraCaptureDto> ProcessCaptureAsync(CameraCaptureWithGateDto captureDto);
    Task RemoveCaptureAsync(Guid id);
}