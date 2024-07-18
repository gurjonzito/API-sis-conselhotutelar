using API_sis_conselhotutelarv2.Models;
using API_sis_conselhotutelarv2.Repositórios;
using API_sis_conselhotutelarv2.Repositórios.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_sis_conselhotutelarv2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteRepositorio _clienteRepositorio;

        public ClienteController(IClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cliente>>> BuscarTodosClientes()
        {
            List<Cliente> clientes = await _clienteRepositorio.BuscarTodosClientes();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Cliente>>> BuscarClientePorId(int id)
        {
            List<Cliente> cliente = await _clienteRepositorio.BuscarClientePorId(id);
            return Ok(cliente);
        }

        [HttpGet("ativos-com-familia")]
        public async Task<ActionResult<List<Cliente>>> ObterClientesAtivosComFamilia()
        {
            List<Cliente> clientes = await _clienteRepositorio.ObterClientesAtivosComFamilia();
            return Ok(clientes);
        }

        [HttpGet("verificar-email/{email}")]
        public async Task<ActionResult<bool>> VerificarEmailCliente(string email)
        {
            bool emailExists = await _clienteRepositorio.VerificarEmailCliente(email);
            return Ok(emailExists);
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> CadastrarCliente([FromBody] Cliente clienteModel)
        {
            Cliente cliente = await _clienteRepositorio.AdicionarCliente(clienteModel);
            return Ok(cliente);
        }
    }
}
