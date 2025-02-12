using Microsoft.AspNetCore.Mvc;
using TaxInvoiceManagment.Application.Interfaces;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceManager _invoiceManager;

        public InvoiceController(IInvoiceManager invoiceManager)
        {
            _invoiceManager = invoiceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var invoices = await _invoiceManager.GetAllInvoices();
            return Ok(invoices);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var invoice = await _invoiceManager.GetInvoiceById(id);
                return Ok(invoice);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Invoice invoice)
        {
            if (invoice == null)
            {
                return BadRequest("Invoice data is required.");
            }

            bool created = await _invoiceManager.CreateInvoice(invoice);
            if (!created)
            {
                return StatusCode(500, "Invoice could not be created.");
            }
            return CreatedAtAction(nameof(GetById), new { id = invoice.Id }, invoice);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Invoice invoice)
        {
            if (invoice == null || invoice.Id == 0)
            {
                return BadRequest("Valid Invoice data is required.");
            }

            bool updated = await _invoiceManager.UpdateInvoice(invoice);
            if (!updated)
            {
                return StatusCode(500, "Invoice could not be updated.");
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _invoiceManager.DeleteInvoice(id);
            if (!deleted)
            {
                return NotFound(new { message = $"Invoice with ID {id} was not found or could not be deleted." });
            }
            return NoContent();
        }
    }
}
