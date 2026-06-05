using AppCore.Dtos;

namespace AppCore.Services;

public interface IReportService
{
    Task<IEnumerable<RevenueReportDto>> GenerateRevenueReportAsync(DateTime? startDate, DateTime? endDate);
    Task<IEnumerable<OccupancyReportDto>> GenerateOccupancyReportAsync(DateTime date);
    Task<IEnumerable<SessionReportDto>> GenerateSessionReportAsync(DateTime? startDate, DateTime? endDate, string? gateName, string? licensePlate);
}
