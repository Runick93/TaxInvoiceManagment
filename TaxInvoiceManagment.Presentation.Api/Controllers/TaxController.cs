using Microsoft.AspNetCore.Mvc;
using TaxInvoiceManagment.Application.Interfaces;
using TaxInvoiceManagment.Application.Models.Dtos;

namespace TaxInvoiceManagment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaxController : ControllerBase
    {
        private readonly ITaxService _taxService;

        public TaxController(ITaxService taxService)
        {
            _taxService = taxService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _taxService.GetAllTaxes();
            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _taxService.GetTaxById(id);
            if (!result.IsSuccess) return NotFound(result.Errors);
            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaxDto taxDto)
        {
            var result = await _taxService.CreateTax(taxDto);
            if (!result.IsSuccess) return BadRequest(result.Errors);
            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TaxDto taxDto)
        {
            if (taxDto.Id != id) return BadRequest("Mismatched ID.");
            var result = await _taxService.UpdateTax(taxDto);
            if (!result.IsSuccess) return BadRequest(result.Errors);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _taxService.DeleteTax(id);
            if (!result.IsSuccess) return NotFound(result.Errors);
            return NoContent();
        }
    }
}
