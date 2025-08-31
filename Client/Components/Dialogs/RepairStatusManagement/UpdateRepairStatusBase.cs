using Client.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.Enums;
using Shared.Helpers;

namespace Client.Components.Dialogs.RepairStatusManagement
{
    public class UpdateRepairStatusBase : ComponentBase
    {
        [CascadingParameter] protected IMudDialogInstance MudDialog { get; set; } = default!;
        [Parameter] public RepairRequestViewModel Report { get; set; } = new();

        protected DateTime? displayResolvedDate;
        protected bool isLoading = false;

        protected override void OnInitialized()
        {
            displayResolvedDate = TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.UtcNow,
                TimeZoneInfo.FindSystemTimeZoneById("Asia/Manila")
            );
        }

        protected bool IsFormValid()
        {
            return Report.Status != RepairStatus.Pending &&
                   displayResolvedDate.HasValue &&
                   !string.IsNullOrWhiteSpace(Report.Remarks);
        }

        protected Severity GetSummarySeverity()
        {
            return IsFormValid() ? Severity.Success : Severity.Warning;
        }

        protected string GetSummaryIcon()
        {
            return IsFormValid() ? Icons.Material.Filled.CheckCircle : Icons.Material.Filled.Warning;
        }

        protected string GetSummaryTitle()
        {
            return IsFormValid() ? "Ready to Resolve" : "Action Required";
        }

        protected string GetSummaryMessage()
        {
            if (Report.Status == RepairStatus.Pending)
                return "Please select a resolution status (Fixed or Replaced)";
            if (string.IsNullOrWhiteSpace(Report.Remarks))
                return "Please provide resolution remarks explaining what was done";
            if (!displayResolvedDate.HasValue)
                return "Please set the resolution date and time";

            return $"This repair will be marked as {Report.Status.ToString().ToLower()} on {displayResolvedDate:MMM dd, yyyy 'at' h:mm tt}";
        }

        protected async Task Submit()
        {
            if (!IsFormValid())
                return;

            isLoading = true;
            StateHasChanged();

            try
            {
                if (displayResolvedDate.HasValue)
                {
                    Report.ResolvedDate = TimeHelper.PHToUtc(displayResolvedDate.Value);
                }
                await Task.Delay(2000);
                MudDialog.Close(DialogResult.Ok(Report));
            }
            catch (Exception)
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        protected void Cancel() => MudDialog.Cancel();
    }
}

