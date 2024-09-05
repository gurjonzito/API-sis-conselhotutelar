using Microsoft.AspNetCore.Mvc;
using API_sis_conselhotutelarv2.Repositórios.Interfaces;

namespace API_sis_conselhotutelarv2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public TestController(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        [HttpGet("verify-connection")]
        public IActionResult VerifyConnection()
        {
            try
            {
                // Obtém a string de conexão atual
                var connectionString = _connectionStringProvider.GetConnectionString();

                // Retorna a string de conexão para fins de teste
                return Ok(new { ConnectionString = connectionString });
            }
            catch (Exception ex)
            {
                // Retorna uma mensagem de erro se ocorrer uma exceção
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
