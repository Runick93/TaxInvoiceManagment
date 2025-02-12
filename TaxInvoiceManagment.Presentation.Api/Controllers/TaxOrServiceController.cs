using Microsoft.AspNetCore.Mvc;
using TaxInvoiceManagment.Application.Interfaces;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaxOrServiceController : ControllerBase
    {
        private readonly ITaxOrServiceManager _taxOrServiceManager;

        public TaxOrServiceController(ITaxOrServiceManager taxOrServiceManager)
        {
            _taxOrServiceManager = taxOrServiceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var taxesOrServices = await _taxOrServiceManager.GetAllTaxesOrServices();
            return Ok(taxesOrServices);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var taxOrService = await _taxOrServiceManager.GetTaxOrServiceById(id);
                return Ok(taxOrService);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaxOrService taxOrService)
        {
            if (taxOrService == null)
            {
                return BadRequest("TaxOrService data is required.");
            }

            bool created = await _taxOrServiceManager.CreateTaxOrService(taxOrService);
            if (!created)
            {
                return StatusCode(500, "TaxOrService could not be created.");
            }
            return CreatedAtAction(nameof(GetById), new { id = taxOrService.Id }, taxOrService);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TaxOrService taxOrService)
        {
            if (taxOrService == null || taxOrService.Id == 0)
            {
                return BadRequest("Valid TaxOrService data is required.");
            }

            bool updated = await _taxOrServiceManager.UpdateTaxOrService(taxOrService);
            if (!updated)
            {
                return StatusCode(500, "TaxOrService could not be updated.");
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _taxOrServiceManager.DeleteTaxOrService(id);
            if (!deleted)
            {
                return NotFound(new { message = $"TaxOrService with ID {id} was not found or could not be deleted." });
            }
            return NoContent();
        }
    }
}
