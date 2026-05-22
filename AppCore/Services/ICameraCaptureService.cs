using AppCore.Dtos;

namespace AppCore.Services;

public interface ICameraCaptureService
{
    Task<CameraCaptureDto> ProcessCaptureAsync(CameraCaptureWithGateDto captureDto);
    Task<bool> RemoveCaptureAsync(CameraCaptureDto captureDto);
}