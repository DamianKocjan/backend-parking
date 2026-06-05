namespace AppCore.Dtos;

public record SessionReportDto(
    Guid SessionId,
    string LicensePlate,
    string GateName,
    DateTime EntryTime,
    DateTime? ExitTime,
    TimeSpan? Duration,
    decimal? ParkingFee,
    bool IsActive
);
