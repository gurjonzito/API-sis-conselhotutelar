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
    public class AtendimentoController : ControllerBase
    {
        private readonly IAtendimentoRepositorio _atendimentoRepositorio;

        public AtendimentoController(IAtendimentoRepositorio atendimentoRepositorio)
        {
            _atendimentoRepositorio = atendimentoRepositorio;
        }

        [HttpGet]
        public async Task<ActionResult<List<Atendimento>>> BuscarTodosAtendimentos()
        {
            List<Atendimento> atendimento = await _atendimentoRepositorio.BuscarTodosAtendimentos();
            return Ok(atendimento);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Atendimento>> BuscarAtendimentoPorId(int id)
        {
            Atendimento atendimento = await _atendimentoRepositorio.BuscarAtendimentoPorId(id);
            return Ok(atendimento);
        }

        [HttpGet("codigo/{codigo}")]
        public async Task<ActionResult<Atendimento>> ObterAtendimentoPorCodigo(string codigo)
        {
            var atendimento = await _atendimentoRepositorio.BuscarAtendimentoPorCodigoAsync(codigo);
            if (atendimento == null)
            {
                return NotFound();
            }
            return Ok(atendimento);
        }

        [HttpGet("clientes-nomes")]
        public async Task<ActionResult<List<string>>> ObterNomesClientes()
        {
            var nomes = await _atendimentoRepositorio.ObterNomesClientesAtendimento();
            return Ok(nomes);
        }

        [HttpGet("colaboradores-nomes")]
        public async Task<ActionResult<List<string>>> ObterNomesColaboradores()
        {
            var nomes = await _atendimentoRepositorio.ObterNomesColaboradoresAtendimento();
            return Ok(nomes);
        }

        [HttpGet("ObterIdClientePorNome/{nomeCliente}")]
        public async Task<IActionResult> ObterIdClientePorNome(string nomeCliente)
        {
            try
            {
                var clienteId = await _atendimentoRepositorio.ObterIdClientePorNome(nomeCliente);
                return Ok(clienteId);
            }
            catch (Exception ex)
            {
                return NotFound("Cliente não encontrado: " + ex.Message);
            }
        }

        [HttpGet("ObterIdColaboradorPorNome/{nomeColaborador}")]
        public async Task<IActionResult> ObterIdColaboradorPorNome(string nomeColaborador)
        {
            try
            {
                var colaboradorId = await _atendimentoRepositorio.ObterIdColaboradorPorNome(nomeColaborador);
                return Ok(colaboradorId);
            }
            catch (Exception ex)
            {
                return NotFound("Colaborador não encontrado: " + ex.Message);
            }
        }

        [HttpGet("ObterNomeClientePorId/{idCliente}")]
        public async Task<IActionResult> ObterNomeClientePorId(int idCliente)
        {
            try
            {
                var nomeCliente = await _atendimentoRepositorio.ObterNomeClientePorId(idCliente);
                return Ok(nomeCliente);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("ObterNomeColaboradorPorId/{idColab}")]
        public async Task<IActionResult> ObterNomeColaboradorPorId(int idColab)
        {
            try
            {
                var nomeColab = await _atendimentoRepositorio.ObterNomeColaboradorPorId(idColab);
                return Ok(nomeColab);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("atendimentos-com-nomes")]
        public async Task<IActionResult> ObterAtendimentosComNomes()
        {
            var atendimentos = await _atendimentoRepositorio.ObterAtendimentosComNomes();
            return Ok(atendimentos);
        }

        [HttpPost]
        public async Task<ActionResult<Atendimento>> CadastrarAtendimento([FromBody] AtendimentoDto atendimentoModel)
        {
            // Verifique a validade do modelo
            if (!ModelState.IsValid)
            {
                // Remover campos não obrigatórios da validação
                ModelState.Remove("NomeCidadao");
                ModelState.Remove("NomeAtendente");
                ModelState.Remove("Cidadao");
                ModelState.Remove("Colaborador");

                // Retornar erros de validação, se houver
                return BadRequest(ModelState);
            }

            // Adicionar o atendimento ao repositório utilizando o DTO
            var atendimento = await _atendimentoRepositorio.AdicionarAtendimento(atendimentoModel);
            if (atendimento != null)
            {
                return Ok(new ApiResponse<Atendimento>
                {
                    Data = atendimento,
                    Success = true,
                    Message = "Atendimento cadastrado com sucesso"
                });
            }

            return StatusCode(500, new ApiResponse<bool>
            {
                Data = false,
                Success = false,
                Message = "Erro ao cadastrar atendimento"
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarAtendimento(int id, [FromBody] AtendimentoEdicaoDto atendimentoDto)
        {
            if (id <= 0 || atendimentoDto == null)
            {
                return BadRequest("Dados inválidos.");
            }

            try
            {
                var atendimentoAtualizado = await _atendimentoRepositorio.AtualizarAtendimento(atendimentoDto, id);
                if (atendimentoAtualizado == null)
                {
                    return NotFound("Atendimento não encontrado.");
                }

                var apiResponse = new ApiResponse<Atendimento>
                {
                    Data = atendimentoAtualizado,
                    Success = true,
                    Message = "Atendimento atualizado com sucesso."
                };

                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar atendimento: {ex.Message}");
                var apiResponse = new ApiResponse<Atendimento>
                {
                    Data = null,
                    Success = false,
                    Message = ex.Message
                };

                return StatusCode(StatusCodes.Status500InternalServerError, apiResponse);
            }
        }
    }
}
