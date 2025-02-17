using Microsoft.AspNetCore.Mvc;
using TaxInvoiceManagment.Application.Dtos;
using TaxInvoiceManagment.Application.Interfaces;

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
            var result = await _taxOrServiceManager.GetAllTaxesOrServices();
            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _taxOrServiceManager.GetTaxOrServiceById(id);
            if (!result.IsSuccess) return NotFound(result.Errors);
            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaxOrServiceDto taxOrServiceDto)
        {
            var result = await _taxOrServiceManager.CreateTaxOrService(taxOrServiceDto);
            if (!result.IsSuccess) return BadRequest(result.Errors);
            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TaxOrServiceDto taxOrServiceDto)
        {
            if (taxOrServiceDto.Id != id) return BadRequest("Mismatched ID.");
            var result = await _taxOrServiceManager.UpdateTaxOrService(taxOrServiceDto);
            if (!result.IsSuccess) return BadRequest(result.Errors);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _taxOrServiceManager.DeleteTaxOrService(id);
            if (!result.IsSuccess) return NotFound(result.Errors);
            return NoContent();
        }
    }
}
