using AppCore.Dtos;

namespace AppCore.Services;

public interface IDataExportService
{
    byte[] ExportSessionsToCsv(IEnumerable<SessionReportDto> sessions);
    byte[] ExportSessionsToPdf(IEnumerable<SessionReportDto> sessions);
}
