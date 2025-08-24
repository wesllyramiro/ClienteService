using Microsoft.AspNetCore.Mvc;
using ClienteService.Models;
using ClienteService.Services;

namespace ClienteService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController(IClienteService service) : ControllerBase
    {
        private readonly IClienteService _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("count")]
        public async Task<IActionResult> Count() => Ok(await _service.CountAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var c = await _service.GetByIdAsync(id);
            if (c == null) return NotFound();
            return Ok(c);
        }

        [HttpGet("find")]
        public async Task<IActionResult> FindByName([FromQuery] string nome) =>
            Ok(await _service.FindByNameAsync(nome));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cliente cliente)
        {
            var created = await _service.CreateAsync(cliente);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Cliente cliente)
        {
            if (id != cliente.Id) return BadRequest();
            var ok = await _service.UpdateAsync(cliente);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}