using API_sis_conselhotutelarv2.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_sis_conselhotutelarv2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HealthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Check()
        {
            var healthCheckResult = new HealthCheckResult();

            // Verificação de conexão com o banco de dados
            try
            {
                await _context.Database.CanConnectAsync();
                healthCheckResult.DatabaseStatus = "Healthy";
            }
            catch (Exception ex)
            {
                healthCheckResult.DatabaseStatus = "Unhealthy";
                healthCheckResult.DatabaseError = ex.Message;
            }

            // Retornando o resultado da verificação
            healthCheckResult.Status = healthCheckResult.DatabaseStatus == "Healthy" ? "Healthy" : "Unhealthy";

            return Ok(healthCheckResult);
        }
    }

    public class HealthCheckResult
    {
        public string Status { get; set; }
        public string DatabaseStatus { get; set; }
        public string DatabaseError { get; set; }
    }
}
