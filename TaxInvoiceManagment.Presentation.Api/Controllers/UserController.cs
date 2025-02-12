using Microsoft.AspNetCore.Mvc;
using TaxInvoiceManagment.Application.Interfaces;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public UserController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userManager.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var user = await _userManager.GetUserById(id);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User data is required.");
            }

            bool created = await _userManager.CreateUser(user);
            if (!created)
            {
                return StatusCode(500, "User could not be created.");
            }
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] User user)
        {
            if (user == null || user.Id == 0)
            {
                return BadRequest("Valid user data is required.");
            }

            bool updated = await _userManager.UpdateUser(user);
            if (!updated)
            {
                return StatusCode(500, "User could not be updated.");
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _userManager.DeleteUser(id);
            if (!deleted)
            {
                return NotFound(new { message = $"User with ID {id} was not found or could not be deleted." });
            }
            return NoContent();
        }
    }
}
