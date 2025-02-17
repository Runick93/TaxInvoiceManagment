using Microsoft.AspNetCore.Mvc;
using TaxInvoiceManagment.Application.Dtos;
using TaxInvoiceManagment.Application.Interfaces;

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
            var result = await _userManager.GetAllUsers();
            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _userManager.GetUserById(id);
            if (!result.IsSuccess) return NotFound(result.Errors);
            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserDto userDto)
        {
            var result = await _userManager.CreateUser(userDto);
            if (!result.IsSuccess) return BadRequest(result.Errors);
            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserDto userDto)
        {
            if (userDto.Id != id) return BadRequest("Mismatched ID.");
            var result = await _userManager.UpdateUser(userDto);
            if (!result.IsSuccess) return BadRequest(result.Errors);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userManager.DeleteUser(id);
            if (!result.IsSuccess) return NotFound(result.Errors);
            return NoContent();
        }
    }
}