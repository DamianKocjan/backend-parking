using AppCore.Dtos;
using AppCore.Repositories;
using AppCore.Services;

namespace Infrastructure.Services;

public class ReportService(IParkingUnitOfWork unitOfWork) : IReportService
{
    public async Task<IEnumerable<RevenueReportDto>> GenerateRevenueReportAsync(DateTime? startDate, DateTime? endDate)
    {
        var sessions = await unitOfWork.Sessions.GetSessionsAsync(startDate, endDate, null, null);
        
        return sessions
            .Where(s => s.ParkingFee.HasValue)
            .GroupBy(s => s.EntryTime.Date)
            .Select(g => new RevenueReportDto(
                g.Key,
                g.Sum(s => s.ParkingFee!.Value),
                g.Count()
            ))
            .OrderBy(r => r.Date)
            .ToList();
    }

    public async Task<IEnumerable<OccupancyReportDto>> GenerateOccupancyReportAsync(DateTime date)
    {
        var startOfDay = date.Date;
        var endOfDay = startOfDay.AddDays(1);
        
        var sessions = await unitOfWork.Sessions.GetSessionsAsync(startOfDay, endOfDay, null, null);
        
        return sessions
            .GroupBy(s => s.EntryTime.Hour)
            .Select(g => new OccupancyReportDto(
                startOfDay,
                g.Key,
                g.Count()
            ))
            .OrderBy(r => r.Hour)
            .ToList();
    }

    public async Task<IEnumerable<SessionReportDto>> GenerateSessionReportAsync(DateTime? startDate, DateTime? endDate, string? gateName, string? licensePlate)
    {
        var sessions = await unitOfWork.Sessions.GetSessionsAsync(startDate, endDate, gateName, licensePlate);
        
        return sessions.Select(s => new SessionReportDto(
            s.Id,
            s.Vehicle?.LicensePlate ?? "Unknown",
            s.GateName,
            s.EntryTime,
            s.ExitTime,
            s.ExitTime.HasValue ? s.ExitTime.Value - s.EntryTime : null,
            s.ParkingFee,
            s.IsActive
        )).ToList();
    }
}
