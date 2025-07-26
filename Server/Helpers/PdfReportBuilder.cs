using Shared.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Linq;

public static class PdfReportBuilder
{
    public static byte[] Build(IEnumerable<RepairRequestDTO> reports, string title)
    {
        using var stream = new MemoryStream();
        var fixedReports = reports.Where(r => r.Status.ToString() == "Fixed").ToList();
        var replacedReports = reports.Where(r => r.Status.ToString() == "Replaced").ToList();

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

             
                page.Header().Element(BuildHeader);

              
                page.Content().Element(content => BuildContent(content, fixedReports, replacedReports, title));

             
                page.Footer().Element(BuildFooter);
            });
        }).GeneratePdf(stream);

        return stream.ToArray();
    }

    private static void BuildHeader(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().BorderBottom(2).BorderColor(Colors.Blue.Medium).PaddingBottom(10).Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("COMPLETED REPAIR REQUEST REPORT").FontSize(20).Bold().FontColor(Colors.Blue.Darken2);
                    col.Item().Text("Equipment Management System").FontSize(12).FontColor(Colors.Grey.Darken1);
                });
                row.ConstantItem(100).AlignRight().Text($"Generated: {DateTime.Now:MMM dd, yyyy}").FontSize(9).FontColor(Colors.Grey.Darken1);
            });
        });
    }

    private static void BuildContent(IContainer container, List<RepairRequestDTO> fixedReports, List<RepairRequestDTO> replacedReports, string title)
    {
        container.PaddingVertical(20).Column(column =>
        {
            
            column.Item().Element(c => BuildTitleSection(c, title));

         
            column.Item().Element(c => BuildSummaryCards(c, fixedReports.Count, replacedReports.Count));

            if (fixedReports.Any())
            {
                column.Item().Element(c => BuildReportSection(c, "FIXED REPORTS", fixedReports, Colors.Green.Medium));
            }

            if (replacedReports.Any())
            {
                column.Item().PageBreak();
                column.Item().Element(c => BuildReportSection(c, "REPLACED REPORTS", replacedReports, Colors.Orange.Medium));
            }

            column.Item().ShowOnce().Element(c => BuildFinalSummary(c, fixedReports.Count, replacedReports.Count));
        });
    }

    private static void BuildTitleSection(IContainer container, string title)
    {
        container.PaddingBottom(20).Column(column =>
        {
            column.Item().Background(Colors.Blue.Lighten4).Padding(15).Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text(title).FontSize(16).Bold().FontColor(Colors.Blue.Darken2);
                    col.Item().Text("Comprehensive repair and maintenance report").FontSize(11).FontColor(Colors.Grey.Darken1);
                });
            });
        });
    }

    private static void BuildSummaryCards(IContainer container, int fixedCount, int replacedCount)
    {
        container.PaddingBottom(20).Row(row =>
        {
            row.RelativeItem().Padding(5).Background(Colors.Green.Lighten4).Padding(15).Column(col =>
            {
                col.Item().Text("FIXED").FontSize(12).Bold().FontColor(Colors.Green.Darken2);
                col.Item().Text(fixedCount.ToString()).FontSize(24).Bold().FontColor(Colors.Green.Darken2);
                col.Item().Text("Requests").FontSize(10).FontColor(Colors.Grey.Darken1);
            });

            row.RelativeItem().Padding(5).Background(Colors.Orange.Lighten4).Padding(15).Column(col =>
            {
                col.Item().Text("REPLACED").FontSize(12).Bold().FontColor(Colors.Orange.Darken2);
                col.Item().Text(replacedCount.ToString()).FontSize(24).Bold().FontColor(Colors.Orange.Darken2);
                col.Item().Text("Requests").FontSize(10).FontColor(Colors.Grey.Darken1);
            });

            row.RelativeItem().Padding(5).Background(Colors.Blue.Lighten4).Padding(15).Column(col =>
            {
                col.Item().Text("TOTAL").FontSize(12).Bold().FontColor(Colors.Blue.Darken2);
                col.Item().Text((fixedCount + replacedCount).ToString()).FontSize(24).Bold().FontColor(Colors.Blue.Darken2);
                col.Item().Text("Requests").FontSize(10).FontColor(Colors.Grey.Darken1);
            });
        });
    }

    private static void BuildReportSection(IContainer container, string sectionTitle, List<RepairRequestDTO> reports, string accentColor)
    {
        container.PaddingBottom(20).Column(column =>
        {
            column.Item().Background(accentColor).Padding(12).Row(row =>
            {
                row.RelativeItem().Text(sectionTitle).FontSize(14).Bold().FontColor(Colors.White);
                row.ConstantItem(100).AlignRight().Text($"Count: {reports.Count}").FontSize(12).FontColor(Colors.White);
            });

          
            if (reports.Count > 20) 
            {
                var chunks = reports.Select((r, i) => new { Report = r, Index = i })
                                  .GroupBy(x => x.Index / 20)
                                  .Select(g => g.Select(x => x.Report).ToList())
                                  .ToList();

                foreach (var (chunk, index) in chunks.Select((c, i) => (c, i)))
                {
                    if (index > 0)
                    {
                        column.Item().PageBreak();
                       
                        column.Item().Background(accentColor).Padding(12).Row(row =>
                        {
                            row.RelativeItem().Text($"{sectionTitle} (Continued)").FontSize(14).Bold().FontColor(Colors.White);
                            row.ConstantItem(100).AlignRight().Text($"Page {index + 1}").FontSize(12).FontColor(Colors.White);
                        });
                    }
                    column.Item().Element(c => BuildTable(c, chunk));
                }
            }
            else
            {
                column.Item().Element(c => BuildTable(c, reports));
            }
        });
    }

    private static void BuildTable(IContainer container, List<RepairRequestDTO> reports)
    {
        container.Border(1).BorderColor(Colors.Grey.Medium).Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(40);   
                columns.ConstantColumn(90);   
                columns.ConstantColumn(70);  
                columns.ConstantColumn(55);  
                columns.ConstantColumn(70);   
                columns.ConstantColumn(70);  
                columns.RelativeColumn(2);   
                columns.ConstantColumn(80); 
            });

        
            table.Header(header =>
            {
                header.Cell().Element(c => BuildHeaderCell(c, "ID"));
                header.Cell().Element(c => BuildHeaderCell(c, "Device"));
                header.Cell().Element(c => BuildHeaderCell(c, "Room"));
                header.Cell().Element(c => BuildHeaderCell(c, "Status"));
                header.Cell().Element(c => BuildHeaderCell(c, "Reported"));
                header.Cell().Element(c => BuildHeaderCell(c, "Resolved"));
                header.Cell().Element(c => BuildHeaderCell(c, "Remarks"));
                header.Cell().Element(c => BuildHeaderCell(c, "Reported By"));
            });

            foreach (var (report, index) in reports.Select((r, i) => (r, i)))
            {
                var isEven = index % 2 == 0;
                var backgroundColor = isEven ? Colors.Grey.Lighten5 : Colors.White;

                table.Cell().Element(c => BuildDataCell(c, report.RepairId.ToString(), backgroundColor));
                table.Cell().Element(c => BuildDataCell(c, TruncateText(report.DeviceTag, 12), backgroundColor));
                table.Cell().Element(c => BuildDataCell(c, TruncateText(report.RoomName, 10), backgroundColor));
                table.Cell().Element(c => BuildDataCell(c, report.Status.ToString(), backgroundColor, GetStatusColor(report.Status.ToString())));
                table.Cell().Element(c => BuildDataCell(c, report.ReportedDate.ToString("MM/dd/yy"), backgroundColor));
                table.Cell().Element(c => BuildDataCell(c, report.ResolvedDate?.ToString("MM/dd/yy") ?? "-", backgroundColor));
                table.Cell().Element(c => BuildDataCell(c, TruncateText(report.Remarks, 35), backgroundColor));
                table.Cell().Element(c => BuildDataCell(c, TruncateText(report.ReportedByUserName, 10), backgroundColor));
            }
        });
    }

    private static void BuildHeaderCell(IContainer container, string text)
    {
        container.Background(Colors.Blue.Darken1).Padding(5).Text(text)
            .FontSize(8).Bold().FontColor(Colors.White).LineHeight(1.1f);
    }

    private static void BuildDataCell(IContainer container, string text, string backgroundColor, string? textColor = null)
    {
        container.Background(backgroundColor).Border(1).BorderColor(Colors.Grey.Lighten2).Padding(3)
            .Text(text).FontSize(7).FontColor(textColor ?? Colors.Black).LineHeight(1.1f);
    }

    private static string TruncateText(string? text, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "-";

        if (text.Length <= maxLength)
            return text;

        return text.Substring(0, maxLength - 3) + "...";
    }

    private static string GetStatusColor(string status)
    {
        return status.ToLower() switch
        {
            "fixed" => Colors.Green.Darken1,
            "replaced" => Colors.Orange.Darken1,
            "pending" => Colors.Yellow.Darken1,
            "cancelled" => Colors.Red.Darken1,
            _ => Colors.Black
        };
    }

    private static void BuildFinalSummary(IContainer container, int fixedCount, int replacedCount)
    {
        container.PaddingTop(20).Border(2).BorderColor(Colors.Blue.Medium).Background(Colors.Blue.Lighten5).Padding(15).Row(row =>
        {
            row.RelativeItem().Column(col =>
            {
                col.Item().Text("REPORT SUMMARY").FontSize(14).Bold().FontColor(Colors.Blue.Darken2);
                col.Item().Text($"This report contains {fixedCount + replacedCount} total repair requests").FontSize(10).FontColor(Colors.Grey.Darken1);
            });

            row.ConstantItem(200).AlignRight().Column(col =>
            {
                col.Item().Text($"Fixed: {fixedCount}").FontSize(12).Bold().FontColor(Colors.Green.Darken2);
                col.Item().Text($"Replaced: {replacedCount}").FontSize(12).Bold().FontColor(Colors.Orange.Darken2);
                col.Item().Text($"Total: {fixedCount + replacedCount}").FontSize(12).Bold().FontColor(Colors.Blue.Darken2);
            });
        });
    }

    private static void BuildFooter(IContainer container)
    {
        container.BorderTop(1).BorderColor(Colors.Grey.Medium).PaddingTop(10).Row(row =>
        {
            row.RelativeItem().Text("Equipment Management System - Repair Request Report").FontSize(8).FontColor(Colors.Grey.Darken1);
            row.ConstantItem(100).AlignRight().Text(text =>
            {
                text.Span("Page ").FontSize(8).FontColor(Colors.Grey.Darken1);
                text.CurrentPageNumber().FontSize(8).FontColor(Colors.Grey.Darken1);
                text.Span(" of ").FontSize(8).FontColor(Colors.Grey.Darken1);
                text.TotalPages().FontSize(8).FontColor(Colors.Grey.Darken1);
            });
        });
    }
}