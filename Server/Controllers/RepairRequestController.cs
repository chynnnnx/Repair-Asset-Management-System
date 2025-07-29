using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projServer.Services.Interfaces;
using Shared.DTOs;

namespace projServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepairRequestController : ControllerBase
    {
        private readonly IRepairRequestService _repairRequestService;
        public RepairRequestController(IRepairRequestService repairRequestService)
        {
            _repairRequestService = repairRequestService;
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> AddRepairRequest([FromBody] RepairRequestDTO repairRequestDTO)
        {
            if (repairRequestDTO == null)
                return BadRequest("RepairRequestDTO cannot be null");

            var success = await _repairRequestService.AddRepairRequest(repairRequestDTO);
            if (success)
                return Ok(new { message = "Added successfully" });

            return StatusCode(500, new { message = "Failed to add repair request" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateRepairRequest([FromBody] RepairRequestDTO repairRequestDTO)
        {
            if (repairRequestDTO == null)
                return BadRequest("RepairRequestDTO cannot be null");

            var success = await _repairRequestService.UpdateRepairRequest(repairRequestDTO);
            if (success)
                return Ok(new { message = "Updated successfully" });

            return NotFound(new { message = "Repair request not found" });
        }

        [Authorize(Roles = "User,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRepairRequest(int id)
        {
            var success = await _repairRequestService.DeleteRepairRequest(id);
            if (success)
                return Ok(new { message = "Deleted successfully" });

            return NotFound(new { message = $"No request found with ID {id}" });
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RepairRequestDTO>>> GetAllRequestsWithDeviceAsync()
        {
            var requests = await _repairRequestService.GetAllRequestsWithDeviceAsync();
            return Ok(requests);

        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<RepairRequestDTO>>> GetRequestsByUserId(int userId)
        {
            var requests = await _repairRequestService.GetRequestByUserId(userId);

            if (!requests.Any())
                return NotFound("No repair requests found for this user.");

            return Ok(requests);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("summary")]
        public async Task<ActionResult<IEnumerable<RepairRequestDTO>>> GetFixedAndReplacedSummary([FromQuery] int month, [FromQuery] int year)
        {
            var summary = await _repairRequestService.GetFixedAndReplacedByMonth(month, year);

            if (!summary.Any())
                return NotFound("No fixed or replaced reports found for this month and year.");

            return Ok(summary);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("export/pdf")]
        public async Task<IActionResult> ExportPdf([FromQuery] int month, [FromQuery] int year)
        {
            var file = await _repairRequestService.GenerateReportPdfAsync(month, year);
            var filename = $"Report_{month}_{year}.pdf";
            return File(file, "application/pdf", filename);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("export/excel")]
        public async Task<IActionResult> ExportExcel([FromQuery] int month, [FromQuery] int year)
        {
            var file = await _repairRequestService.GenerateReportExcelAsync(month, year);
            var filename = $"Report_{month}_{year}.xlsx";
            return File(file,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                filename);
        }


    }
}
