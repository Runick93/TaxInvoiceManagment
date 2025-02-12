using Microsoft.AspNetCore.Mvc;
using TaxInvoiceManagment.Application.Interfaces;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaxableItemController : ControllerBase
    {
        private readonly ITaxableItemManager _taxableItemManager;

        public TaxableItemController(ITaxableItemManager taxableItemManager)
        {
            _taxableItemManager = taxableItemManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var taxableItems = await _taxableItemManager.GetAllAssets();
            return Ok(taxableItems);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var taxableItem = await _taxableItemManager.GetAssetById(id);
                return Ok(taxableItem);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaxableItem asset)
        {
            if (asset == null)
            {
                return BadRequest("TaxableItem data is required.");
            }

            bool created = await _taxableItemManager.CreateAsset(asset);
            if (!created)
            {
                return StatusCode(500, "TaxableItem could not be created.");
            }
            return CreatedAtAction(nameof(GetById), new { id = asset.Id }, asset);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TaxableItem asset)
        {
            if (asset == null || asset.Id == 0)
            {
                return BadRequest("Valid TaxableItem data is required.");
            }

            bool updated = await _taxableItemManager.UpdateAsset(asset);
            if (!updated)
            {
                return StatusCode(500, "TaxableItem could not be updated.");
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _taxableItemManager.DeleteAsset(id);
            if (!deleted)
            {
                return NotFound(new { message = $"TaxableItem with ID {id} was not found or could not be deleted." });
            }
            return NoContent();
        }
    }
}