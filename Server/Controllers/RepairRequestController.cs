using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projServer.Services.Interfaces;
using Shared.DTOs;

namespace projServer.Controllers
{
    /// <summary>
    /// handles repair request operations like adding, updating,
    /// deleting, retrieving, and exporting reports
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RepairRequestController : ControllerBase
    {
        private readonly IRepairRequestService _repairRequestService;

        public RepairRequestController(IRepairRequestService repairRequestService)
        {
            _repairRequestService = repairRequestService;
        }

        /// <summary>
        /// adds a new repair request (user only)
        /// </summary>
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> AddRepairRequest([FromBody] RepairRequestDTO repairRequestDTO)
        {
            if (repairRequestDTO == null)
                return BadRequest("RepairRequestDTO cannot be null");

            var success = await _repairRequestService.AddRepairRequest(repairRequestDTO);
            if (success)
                return Ok(new { message = "added successfully" });

            return StatusCode(500, new { message = "failed to add repair request" });
        }

        /// <summary>
        /// updates an existing repair request
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> UpdateRepairRequest([FromBody] RepairRequestDTO repairRequestDTO)
        {
            if (repairRequestDTO == null)
                return BadRequest("RepairRequestDTO cannot be null");

            var success = await _repairRequestService.UpdateRepairRequest(repairRequestDTO);
            if (success)
                return Ok(new { message = "updated successfully" });

            return NotFound(new { message = "repair request not found" });
        }

        /// <summary>
        /// deletes a repair request by id
        /// </summary>
        [Authorize(Roles = "User,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRepairRequest(int id)
        {
            var success = await _repairRequestService.DeleteRepairRequest(id);
            if (success)
                return Ok(new { message = "deleted successfully" });

            return NotFound(new { message = $"no request found with id {id}" });
        }

        /// <summary>
        /// gets all repair requests with device details
        /// </summary>
        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RepairRequestDTO>>> GetAllRequestsWithDeviceAsync()
        {
            var requests = await _repairRequestService.GetAllRequestsWithDeviceAsync();
            return Ok(requests);
        }

        /// <summary>
        /// gets all repair requests for a specific user
        /// </summary>
        [Authorize(Roles = "User,Admin")]
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<RepairRequestDTO>>> GetRequestsByUserId(int userId)
        {
            var requests = await _repairRequestService.GetRequestByUserId(userId);

            if (!requests.Any())
                return NotFound("no repair requests found for this user");

            return Ok(requests);
        }

        /// <summary>
        /// gets summary of fixed and replaced devices for a month and year
        /// </summary>
        [HttpGet("summary")]
        public async Task<ActionResult<IEnumerable<RepairRequestDTO>>> GetFixedAndReplacedSummary([FromQuery] int month, [FromQuery] int year)
        {
            var summary = await _repairRequestService.GetFixedAndReplacedByMonth(month, year);

            if (!summary.Any())
                return NotFound("no fixed or replaced reports found for this month and year");

            return Ok(summary);
        }

        /// <summary>
        /// exports monthly report as pdf
        /// </summary>
        [HttpGet("export/pdf")]
        public async Task<IActionResult> ExportPdf([FromQuery] int month, [FromQuery] int year)
        {
            var file = await _repairRequestService.GenerateReportPdfAsync(month, year);
            var filename = $"Report_{month}_{year}.pdf";
            return File(file, "application/pdf", filename);
        }

        /// <summary>
        /// exports monthly report as excel
        /// </summary>
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
