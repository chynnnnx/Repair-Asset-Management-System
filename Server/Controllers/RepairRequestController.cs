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

        [HttpPost]
        public async Task<IActionResult> AddRepairRequest([FromBody] RepairRequestDTO repairRequestDTO)
        {
            if (repairRequestDTO == null)
                return BadRequest();

            await _repairRequestService.AddRepairRequest(repairRequestDTO);
            return Ok(new { message = "added successfully" });

        }
        [HttpPut]
        public async Task<IActionResult> UpdateRepairRequest([FromBody] RepairRequestDTO repairRequestDTO)
        {
            if (repairRequestDTO == null)
                return BadRequest();

            await _repairRequestService.UpdateRepairRequest(repairRequestDTO);
            return Ok(new { message = "updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRepairRequest(int id)
        {
            try
            {
                await _repairRequestService.DeleteRepairRequest(id);
                return Ok(new { message = "deleted successfully" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RepairRequestDTO>>> GetAllRequestsWithDeviceAsync()
        {
            var requests = await _repairRequestService.GetAllRequestsWithDeviceAsync();
            return Ok(requests);

        }
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<RepairRequestDTO>>> GetRequestsByUserId(int userId)
        {
            var requests = await _repairRequestService.GetRequestByUserId(userId);

            if (!requests.Any())
                return NotFound("No repair requests found for this user.");

            return Ok(requests);
        }
        [HttpGet("summary")]
        public async Task<ActionResult<IEnumerable<RepairRequestDTO>>> GetFixedAndReplacedSummary([FromQuery] int month, [FromQuery] int year)
        {
            var summary = await _repairRequestService.GetFixedAndReplacedByMonth(month, year);

            if (!summary.Any())
                return NotFound("No fixed or replaced reports found for this month and year.");

            return Ok(summary);
        }

        [HttpGet("export/pdf")]
        public async Task<IActionResult> ExportPdf([FromQuery] int month, [FromQuery] int year)
        {
            var file = await _repairRequestService.GenerateReportPdfAsync(month, year);
            var filename = $"Report_{month}_{year}.pdf";
            return File(file, "application/pdf", filename);
        }

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