namespace AppCore.Dtos;

public record OccupancyReportDto(
    DateTime Date,
    int Hour,
    int OccupancyCount
);
