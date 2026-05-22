using AppCore.Models;

namespace AppCore.Dtos;

public record CameraCaptureDto(
    string LicensePlate,
    string Brand,
    string Color,
    string GateName,
    string? ImagePath = null
);

public record CreateCameraCaptureDto(
    string ImagePath
)
{
    public CameraCapture ToEntity()
    {
        return new CameraCapture()
        {
            ImagePath = ImagePath
        };
    }
};

public record CameraCaptureWithGateDto(
    string GateName,
    string ImagePath
);