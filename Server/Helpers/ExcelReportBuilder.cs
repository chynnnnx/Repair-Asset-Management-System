using ClosedXML.Excel;
using Shared.DTOs;

namespace projServer.Helpers
{
    public static class ExcelReportBuilder
    {
        public static byte[] Build(IEnumerable<RepairRequestDTO> reports)
        {
            using var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add("Report Summary");

            int currentRow = 1;

            void WriteSection(string title, List<RepairRequestDTO> sectionReports)
            {
                sheet.Cell(currentRow, 1).Value = $"{title} Reports";
                sheet.Range(currentRow, 1, currentRow, 8).Merge().Style
                    .Font.SetBold()
                    .Font.FontSize = 14;
                currentRow += 1;
                sheet.Cell(currentRow, 1).Value = "Report ID";
                sheet.Cell(currentRow, 2).Value = "Device";
                sheet.Cell(currentRow, 3).Value = "Room";
                sheet.Cell(currentRow, 4).Value = "Status";
                sheet.Cell(currentRow, 5).Value = "Date Reported";
                sheet.Cell(currentRow, 6).Value = "Resolved Date";
                sheet.Cell(currentRow, 7).Value = "Remarks";
                sheet.Cell(currentRow, 8).Value = "Reported By";
                sheet.Range(currentRow, 1, currentRow, 8).Style.Font.Bold = true;

                currentRow++;

                foreach (var r in sectionReports)
                {
                    sheet.Cell(currentRow, 1).Value = r.RepairId;
                    sheet.Cell(currentRow, 2).Value = r.DeviceTag;
                    sheet.Cell(currentRow, 3).Value = r.RoomName;
                    sheet.Cell(currentRow, 4).Value = r.Status.ToString();
                    sheet.Cell(currentRow, 5).Value = r.ReportedDate.ToString("MMM dd, yyyy");
                    sheet.Cell(currentRow, 6).Value = r.ResolvedDate?.ToString("MMM dd, yyyy") ?? "-";
                    sheet.Cell(currentRow, 7).Value = string.IsNullOrWhiteSpace(r.Remarks) ? "-" : r.Remarks;
                    sheet.Cell(currentRow, 8).Value = r.ReportedByUserName;
                    currentRow++;
                }

                sheet.Cell(currentRow, 1).Value = $"Total {title}: {sectionReports.Count}";
                sheet.Range(currentRow, 1, currentRow, 8).Merge().Style.Font.SetBold();
                currentRow += 2; 
            }

            var fixedReports = reports.Where(r => r.Status.ToString() == "Fixed").ToList();
            var replacedReports = reports.Where(r => r.Status.ToString() == "Replaced").ToList();

            if (fixedReports.Any())
                WriteSection("Fixed", fixedReports);

            if (replacedReports.Any())
                WriteSection("Replaced", replacedReports);

            sheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
