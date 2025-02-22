using Microsoft.AspNetCore.Mvc;
using TaxInvoiceManagment.Application.Interfaces;
using TaxInvoiceManagment.Application.Models.Dtos;

namespace TaxInvoiceManagment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceManager;

        public InvoiceController(IInvoiceService invoiceManager)
        {
            _invoiceManager = invoiceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _invoiceManager.GetAllInvoices();
            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _invoiceManager.GetInvoiceById(id);
            if (!result.IsSuccess) return NotFound(result.Errors);
            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InvoiceDto invoiceDto)
        {
            var result = await _invoiceManager.CreateInvoice(invoiceDto);
            if (!result.IsSuccess) return BadRequest(result.Errors);
            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] InvoiceDto invoiceDto)
        {
            if (invoiceDto.Id != id) return BadRequest("Mismatched ID.");
            var result = await _invoiceManager.UpdateInvoice(invoiceDto);
            if (!result.IsSuccess) return BadRequest(result.Errors);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _invoiceManager.DeleteInvoice(id);
            if (!result.IsSuccess) return NotFound(result.Errors);
            return NoContent();
        }
    }
}
