using AppCore.Dtos;
using AppCore.ValueObjects;

namespace AppCore.Models;

public enum CaptureType
{
    Entry,
    Exit
}

public class CameraCapture : EntityBase
{
    public Guid ParkingGateId { get; set; }
    public ParkingGate ParkingGate { get; set; }
    public string LicensePlate { get; set; }
    public string DetectedBrand { get; set; }
    public string DetectedColor { get; set; }
    public DateTime CapturedAt { get; set; }
    public string ImagePath { get; set; }
    public CaptureType Type { get; set; }

    public static implicit operator CameraCaptureDto(CameraCapture entity) =>
        new (
            entity.LicensePlate,
            entity.DetectedBrand,
            entity.DetectedColor,
            entity.ParkingGate.Name,
            entity.ImagePath
        );
}