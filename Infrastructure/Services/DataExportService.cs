using System.Globalization;
using AppCore.Dtos;
using AppCore.Services;
using CsvHelper;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Infrastructure.Services;

public class DataExportService : IDataExportService
{
    public byte[] ExportSessionsToCsv(IEnumerable<SessionReportDto> sessions)
    {
        using var memoryStream = new MemoryStream();
        using var streamWriter = new StreamWriter(memoryStream);
        using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
        
        csvWriter.WriteRecords(sessions);
        streamWriter.Flush();
        
        return memoryStream.ToArray();
    }

    public byte[] ExportSessionsToPdf(IEnumerable<SessionReportDto> sessions)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10));
                
                page.Header().Text("Parking Sessions Report")
                    .SemiBold().FontSize(18).FontColor(Colors.Black);
                
                page.Content().PaddingVertical(1, Unit.Centimetre).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });
                    
                    table.Header(header =>
                    {
                        header.Cell().BorderBottom(1).PaddingBottom(5).Text("License Plate").SemiBold();
                        header.Cell().BorderBottom(1).PaddingBottom(5).Text("Gate Name").SemiBold();
                        header.Cell().BorderBottom(1).PaddingBottom(5).Text("Entry Time").SemiBold();
                        header.Cell().BorderBottom(1).PaddingBottom(5).Text("Duration").SemiBold();
                        header.Cell().BorderBottom(1).PaddingBottom(5).Text("Fee").SemiBold();
                    });
                    
                    foreach (var session in sessions)
                    {
                        table.Cell().PaddingVertical(2).Text(session.LicensePlate);
                        table.Cell().PaddingVertical(2).Text(session.GateName);
                        table.Cell().PaddingVertical(2).Text(session.EntryTime.ToString("g"));
                        table.Cell().PaddingVertical(2).Text(session.Duration?.ToString(@"hh\:mm\:ss") ?? "-");
                        table.Cell().PaddingVertical(2).Text(session.ParkingFee?.ToString("C") ?? "-");
                    }
                });
                
                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Page ");
                    x.CurrentPageNumber();
                    x.Span(" of ");
                    x.TotalPages();
                });
            });
        });
        
        return document.GeneratePdf();
    }
}
