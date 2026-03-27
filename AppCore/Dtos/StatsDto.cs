namespace AppCore.Dtos;

public record ParkingStatsDto(
    int ActiveVehicles,
    decimal TodayRevenue,
    int TodayEntries,
    int TodayExits
);