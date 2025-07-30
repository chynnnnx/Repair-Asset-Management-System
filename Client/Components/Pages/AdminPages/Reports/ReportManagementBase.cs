using Microsoft.AspNetCore.Components;
using Shared.DTOs;
using Shared.Enums;
using Client.Services.Interfaces;
using MudBlazor;
using Client.Components.Dialogs.RepairStatusManagement;

namespace Client.Components.Pages.AdminPages.Reports
{
    public class ReportManagementBase : ComponentBase
    {
        [Inject] public ISnackbar Snackbar { get; set; } = default!;
        [Inject] public IRepairRequestService RequestService { get; set; }
        [Inject] protected IDialogService DialogService { get; set; } = default!;
        protected List<RepairRequestDTO> allReports = new();
        protected List<RepairRequestDTO> pending = new();
        protected List<RepairRequestDTO> inProgress = new();
        protected List<RepairRequestDTO> fixedReports = new();
        protected List<RepairRequestDTO> replaced = new();
       


        protected bool hasPending, hasInProgress, hasFixed, hasReplaced;

        protected const int PageSize = 6;

        protected int CurrentPagePending = 1;
        protected int CurrentPageInProgress = 1;
        protected int CurrentPageFixed = 1;
        protected int CurrentPageReplaced = 1;

        protected int TotalPagesPending => (int)Math.Ceiling((double)pending.Count / PageSize);
        protected int TotalPagesInProgress => (int)Math.Ceiling((double)inProgress.Count / PageSize);
        protected int TotalPagesFixed => (int)Math.Ceiling((double)fixedReports.Count / PageSize);
        protected int TotalPagesReplaced => (int)Math.Ceiling((double)replaced.Count / PageSize);

        protected IEnumerable<RepairRequestDTO> PagedPending =>
            pending.Skip((CurrentPagePending - 1) * PageSize).Take(PageSize);

        protected IEnumerable<RepairRequestDTO> PagedInProgress =>
            inProgress.Skip((CurrentPageInProgress - 1) * PageSize).Take(PageSize);

        protected IEnumerable<RepairRequestDTO> PagedFixed =>
            fixedReports.Skip((CurrentPageFixed - 1) * PageSize).Take(PageSize);

        protected IEnumerable<RepairRequestDTO> PagedReplaced =>
            replaced.Skip((CurrentPageReplaced - 1) * PageSize).Take(PageSize);

        protected override async Task OnInitializedAsync()
        {
            allReports = await RequestService.GetAllRepairRequestsAsync();

            pending = allReports
                .Where(r => r.Status == RepairStatus.Pending)
                .OrderByDescending(r => r.ReportedDate)
                .ToList();

            inProgress = allReports
                .Where(r => r.Status == RepairStatus.InProgress)
                .OrderByDescending(r => r.ReportedDate)
                .ToList();

            fixedReports = allReports
                .Where(r => r.Status == RepairStatus.Fixed)
                .OrderByDescending(r => r.ResolvedDate ?? r.ReportedDate)
                .ToList();

            replaced = allReports
                .Where(r => r.Status == RepairStatus.Replaced)
                .OrderByDescending(r => r.ResolvedDate ?? r.ReportedDate)
                .ToList();

            hasPending = pending.Any();
            hasInProgress = inProgress.Any();
            hasFixed = fixedReports.Any();
            hasReplaced = replaced.Any();
        }

        protected async Task MarkAsInProgress(RepairRequestDTO report)
        {
            report.Status = RepairStatus.InProgress;
            await RequestService.UpdateRepairRequestAsync(report);
            await RefreshData();
        }


        protected async Task OpenUpdateDialog(RepairRequestDTO report)
        {
            var parameters = new DialogParameters { ["Report"] = report };
            var options = new DialogOptions { CloseOnEscapeKey = true };

            var dialog = DialogService.Show<UpdateRepairStatusDialog>("Update Status", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled && result.Data is RepairRequestDTO updatedReport)
            {
                Snackbar.Add($"Report marked as {updatedReport.Status}", Severity.Success);
                await RequestService.UpdateRepairRequestAsync(updatedReport);
                await RefreshData();
            }

        }

        private async Task RefreshData()
        {
            allReports = await RequestService.GetAllRepairRequestsAsync();

            pending = allReports.Where(r => r.Status == RepairStatus.Pending).ToList();
            inProgress = allReports.Where(r => r.Status == RepairStatus.InProgress).ToList();
            fixedReports = allReports.Where(r => r.Status == RepairStatus.Fixed).ToList();
            replaced = allReports.Where(r => r.Status == RepairStatus.Replaced).ToList();

            hasPending = pending.Any();
            hasInProgress = inProgress.Any();
            hasFixed = fixedReports.Any();
            hasReplaced = replaced.Any();

            StateHasChanged();
        }
    }

}
