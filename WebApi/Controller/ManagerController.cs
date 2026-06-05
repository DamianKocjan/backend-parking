using AppCore.Authorization;
using AppCore.Dtos;
using AppCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[ApiController]
[Route("/api/[controller]")]
[Authorize(Policy = nameof(AppPolicies.AdminOnly))]
public class ManagerController(IReportService reportService, IDataExportService exportService) : ControllerBase
{
    [HttpGet("reports/revenue")]
    [ProducesResponseType(typeof(IEnumerable<RevenueReportDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRevenueReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var result = await reportService.GenerateRevenueReportAsync(startDate, endDate);
        return Ok(result);
    }

    [HttpGet("reports/occupancy")]
    [ProducesResponseType(typeof(IEnumerable<OccupancyReportDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOccupancyReport([FromQuery] DateTime date)
    {
        var result = await reportService.GenerateOccupancyReportAsync(date);
        return Ok(result);
    }

    [HttpGet("reports/sessions")]
    [ProducesResponseType(typeof(IEnumerable<SessionReportDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSessionReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] string? gateName, [FromQuery] string? licensePlate)
    {
        var result = await reportService.GenerateSessionReportAsync(startDate, endDate, gateName, licensePlate);
        return Ok(result);
    }

    [HttpGet("reports/sessions/export")]
    public async Task<IActionResult> ExportSessionReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] string? gateName, [FromQuery] string? licensePlate, [FromQuery] string format = "csv")
    {
        var sessions = await reportService.GenerateSessionReportAsync(startDate, endDate, gateName, licensePlate);

        if (format.Equals("pdf", StringComparison.OrdinalIgnoreCase))
        {
            var pdfBytes = exportService.ExportSessionsToPdf(sessions);
            return File(pdfBytes, "application/pdf", "sessions_report.pdf");
        }

        var csvBytes = exportService.ExportSessionsToCsv(sessions);
        return File(csvBytes, "text/csv", "sessions_report.csv");
    }
}
