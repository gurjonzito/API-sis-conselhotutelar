using API_sis_conselhotutelarv2.Enums;
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

        [HttpGet("ObterIdClientePorNome/{nomeCliente}")]
        public async Task<IActionResult> ObterIdClientePorNome(string nomeCliente)
        {
            try
            {
                var clienteId = await _clienteRepositorio.ObterIdClientePorNome(nomeCliente);
                return Ok(clienteId);
            }
            catch (Exception ex)
            {
                return NotFound("Cliente não encontrado: " + ex.Message);
            }
        }

        [HttpGet("familias-nomes")]
        public async Task<ActionResult<List<string>>> ObterNomesFamilias()
        {
            var nomes = await _clienteRepositorio.ObterNomesFamiliasClientes();
            return Ok(nomes);
        }

        [HttpGet("ObterNomeFamiliaPorId/{idFamilia}")]
        public async Task<IActionResult> ObterNomeFamiliaPorId(int idFamilia)
        {
            try
            {
                var nomeFamilia = await _clienteRepositorio.ObterNomeFamiliaPorId(idFamilia);
                return Ok(nomeFamilia);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("ObterIdFamiliaPorNome/{nomeFamilia}")]
        public async Task<IActionResult> ObterIdFamiliaPorNome(string nomeFamilia)
        {
            try
            {
                var familiaId = await _clienteRepositorio.ObterIdFamiliaPorNome(nomeFamilia);
                return Ok(familiaId);
            }
            catch (Exception ex)
            {
                return NotFound("Familia não encontrada: " + ex.Message);
            }
        }

        [HttpGet("verificar-email/{email}")]
        public async Task<ActionResult<bool>> VerificarEmailCliente(string email)
        {
            bool emailExists = await _clienteRepositorio.VerificarEmailCliente(email);
            return Ok(emailExists);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarCliente(int id, [FromBody] ClienteEdicaoDto clienteDto)
        {
            if (id <= 0 || clienteDto == null)
            {
                return BadRequest("Dados inválidos.");
            }

            try
            {
                var clienteAtualizado = await _clienteRepositorio.AtualizarCliente(clienteDto, id);
                if (clienteAtualizado == null)
                {
                    return NotFound("Colaborador não encontrado.");
                }

                var apiResponse = new ApiResponse<Cliente>
                {
                    Data = clienteAtualizado,
                    Success = true,
                    Message = "Cliente atualizado com sucesso."
                };

                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar cliente: {ex.Message}");
                var apiResponse = new ApiResponse<Cliente>
                {
                    Data = null,
                    Success = false,
                    Message = ex.Message
                };

                return StatusCode(StatusCodes.Status500InternalServerError, apiResponse);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> CadastrarCliente([FromBody] ClienteDto clienteModel)
        {
            // Verifique a validade do modelo
            if (!ModelState.IsValid)
            {
                // Remover campos não obrigatórios da validação
                ModelState.Remove("Familia");

                // Retornar erros de validação, se houver
                return BadRequest(ModelState);
            }

            // Adicionar o atendimento ao repositório utilizando o DTO
            var cliente = await _clienteRepositorio.AdicionarCliente(clienteModel);
            if (cliente != null)
            {
                return Ok(new ApiResponse<Cliente>
                {
                    Data = cliente,
                    Success = true,
                    Message = "Cliente cadastrado com sucesso"
                });
            }

            return StatusCode(500, new ApiResponse<bool>
            {
                Data = false,
                Success = false,
                Message = "Erro ao cadastrar cliente"
            });
        }

        [HttpPut("ativar/{id}")]
        public async Task<IActionResult> AtivarCliente(int id)
        {
            try
            {
                bool resultado = await _clienteRepositorio.AtivarCliente(id);
                if (resultado)
                {
                    return Ok(new { Mensagem = "Cliente ativado com sucesso." });
                }
                return BadRequest("Erro ao ativar cliente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpPut("inativar/{id}")]
        public async Task<IActionResult> InativarCliente(int id)
        {
            try
            {
                bool resultado = await _clienteRepositorio.InativarCliente(id);
                if (resultado)
                {
                    return Ok(new { Mensagem = "Cliente inativado com sucesso." });
                }
                return BadRequest("Erro ao inativar cliente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }

    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
