using Microsoft.AspNetCore.Mvc;
using System.Data;
using Dapper;

namespace ClienteService.Controllers
{
    [ApiController]
    public class HealthController(IDbConnection db) : ControllerBase
    {
        private readonly IDbConnection _db = db;

        [HttpGet("/health")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _db.QueryFirstOrDefaultAsync<int>("SELECT 1");
                var healthy = result == 1;
                return Ok(new { status = "Healthy", db = healthy });
            }
            catch (Exception ex)
            {
                return StatusCode(503, new { status = "Unhealthy", error = ex.Message });
            }
        }
    }
}