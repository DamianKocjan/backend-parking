namespace AppCore.Dtos;

public record VehicleDto(
    Guid Id,
    string LicensePlate,
    string Brand,
    string Color
);