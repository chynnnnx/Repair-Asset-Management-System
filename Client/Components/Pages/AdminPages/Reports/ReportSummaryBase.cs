using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.DTOs;
using Client.Services.Interfaces;
using Microsoft.JSInterop;
using Shared.Helpers;
using Shared.Enums;

namespace Client.Components.Pages.AdminPages.Reports
{
    public class ReportSummaryBase : ComponentBase
    {
        [Inject] protected IRepairRequestService RequestService { get; set; }
        [Inject] protected ISnackbar Snackbar { get; set; }
        [Inject] protected IJSRuntime JS { get; set; }

        public int _selectedMonth = DateTime.Now.Month;
        public int _selectedYear = DateTime.Now.Year;
        public DateTime? _month = DateTime.Today;

        protected int SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                if (_selectedMonth != value)
                {
                    _selectedMonth = value;
                    _ = LoadReports();
                }
            }
        }
        protected string GetStatusLabel(RepairStatus status) => status switch
        {
            RepairStatus.Fixed => "Fixed",
            RepairStatus.Replaced => "Replaced",
         
            _ => "Unknown"
        };

        protected Color GetStatusColor(RepairStatus status) => status switch
        {
            RepairStatus.Fixed => Color.Success,
            RepairStatus.Replaced => Color.Error,
          
            _ => Color.Default
        };


        protected int SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    _ = LoadReports();
                }
            }
        }

        protected List<RepairRequestDTO> Reports { get; set; } = new();
        protected DateTime SelectedDate
        {
            get => new(_selectedYear, _selectedMonth, 1);
            set
            {
                _selectedMonth = value.Month;
                _selectedYear = value.Year;
            }
        }

        protected async Task OnDateChanged(DateTime? newDate)
        {
            if (newDate.HasValue)
            {
                _month = newDate;
                SelectedDate = newDate.Value;
                await LoadReports();
            }
        }



        protected override async Task OnInitializedAsync()
        {
            await LoadReports();
        }

      

        protected async Task LoadReports()
        {
            if (_month.HasValue)
            {
                var selectedMonth = _month.Value.Month;
                var selectedYear = _month.Value.Year;

                Reports = await RequestService.GetSummaryByMonthAsync(selectedMonth, selectedYear);

              
            }
        }
        protected async Task DownloadPdf()
        {
            try
            {
                var fileBytes = await RequestService.DownloadPdfReportAsync(SelectedMonth, SelectedYear);
                await JS.InvokeVoidAsync("BlazorDownloadFile", $"Report_{SelectedMonth}_{SelectedYear}.pdf", "application/pdf", fileBytes);
            }
            catch (Exception ex)
            {
                Snackbar.Add("Error downloading PDF report.", Severity.Error);
            }
        }

        protected async Task DownloadExcel()
        {
            try
            {
                var fileBytes = await RequestService.DownloadExcelReportAsync(SelectedMonth, SelectedYear);
                await JS.InvokeVoidAsync("BlazorDownloadFile", $"Report_{SelectedMonth}_{SelectedYear}.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileBytes);
            }
            catch (Exception ex)
            {
                Snackbar.Add("Error downloading Excel report.", Severity.Error);
            }
        }

    }
}
