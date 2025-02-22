using Microsoft.AspNetCore.Mvc;
using TaxInvoiceManagment.Application.Interfaces;
using TaxInvoiceManagment.Application.Models.Dtos;

namespace TaxInvoiceManagment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaxableItemController : ControllerBase
    {
        private readonly ITaxableItemService _taxableItemManager;

        public TaxableItemController(ITaxableItemService taxableItemManager)
        {
            _taxableItemManager = taxableItemManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _taxableItemManager.GetAllTaxableItems();
            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _taxableItemManager.GetTaxableItemById(id);
            if (!result.IsSuccess) return NotFound(result.Errors);
            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaxableItemDto taxableItemDto)
        {
            var result = await _taxableItemManager.CreateTaxableItem(taxableItemDto);
            if (!result.IsSuccess) return BadRequest(result.Errors);
            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TaxableItemDto taxableItemDto)
        {
            if (taxableItemDto.Id != id) return BadRequest("Mismatched ID.");
            var result = await _taxableItemManager.UpdateTaxableItem(taxableItemDto);
            if (!result.IsSuccess) return BadRequest(result.Errors);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _taxableItemManager.DeleteTaxableItem(id);
            if (!result.IsSuccess) return NotFound(result.Errors);
            return NoContent();
        }
    }
}
