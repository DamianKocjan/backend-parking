namespace AppCore.Dtos;

public record RevenueReportDto(
    DateTime Date,
    decimal TotalRevenue,
    int SessionCount
);
