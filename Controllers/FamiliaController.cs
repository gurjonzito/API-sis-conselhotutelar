using API_sis_conselhotutelarv2.Models;
using API_sis_conselhotutelarv2.Repositórios;
using API_sis_conselhotutelarv2.Repositórios.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_sis_conselhotutelarv2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamiliaController : ControllerBase
    {
        private readonly IFamiliaRepositorio _familiaRepositorio;

        public FamiliaController(IFamiliaRepositorio familiaRepositorio)
        {
            _familiaRepositorio = familiaRepositorio;
        }

        [HttpGet]
        public async Task<ActionResult<List<Familia>>> BuscarTodasFamilias()
        {
            List<Familia> familias = await _familiaRepositorio.BuscarTodasFamilias();
            return Ok(familias);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Familia>> BuscarFamiliaPorId(int id)
        {
            Familia familia = await _familiaRepositorio.BuscarFamiliaPorId(id);
            return Ok(familia);
        }

        [HttpGet("ObterIdFamiliaPorNome/{nomeFamilia}")]
        public async Task<IActionResult> ObterIdFamiliaPorNome(string nomeFamilia)
        {
            try
            {
                var familiaId = await _familiaRepositorio.ObterIdFamiliaPorNome(nomeFamilia);
                return Ok(familiaId);
            }
            catch (Exception ex)
            {
                return NotFound("Família não encontrada: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarFamilia(int id, [FromBody] FamiliaEdicaoDto familiaDto)
        {
            if (id <= 0 || familiaDto == null)
            {
                return BadRequest("Dados inválidos.");
            }

            try
            {
                var familiaAtualizado = await _familiaRepositorio.AtualizarFamilia(familiaDto, id);
                if (familiaAtualizado == null)
                {
                    return NotFound("Família não encontrada.");
                }

                var apiResponse = new ApiResponse<Familia>
                {
                    Data = familiaAtualizado,
                    Success = true,
                    Message = "Família atualizada com sucesso."
                };

                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar família: {ex.Message}");
                var apiResponse = new ApiResponse<Familia>
                {
                    Data = null,
                    Success = false,
                    Message = ex.Message
                };

                return StatusCode(StatusCodes.Status500InternalServerError, apiResponse);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Familia>> CadastrarFamilia([FromBody] Familia familiaModel)
        {

            var familia = new Familia
            {
                Fam_Sobrenomes = familiaModel.Fam_Sobrenomes,
                Fam_Responsavel = familiaModel.Fam_Responsavel,
                Fam_Participantes = familiaModel.Fam_Participantes,
            };

            familia = await _familiaRepositorio.AdicionarFamilia(familia);

            if (familia != null)
            {
                return Ok(new ApiResponse<Familia>
                {
                    Data = familia,
                    Success = true,
                    Message = "Família cadastrada com sucesso"
                });
            }

            return StatusCode(500, new ApiResponse<bool>
            {
                Data = false,
                Success = false,
                Message = "Erro ao cadastrar família"
            });
        }

        [HttpPut("ativar/{id}")]
        public async Task<IActionResult> AtivarFamilia(int id)
        {
            try
            {
                bool resultado = await _familiaRepositorio.AtivarFamilia(id);
                if (resultado)
                {
                    return Ok(new { Mensagem = "Família ativada com sucesso." });
                }
                return BadRequest("Erro ao ativar família.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpPut("inativar/{id}")]
        public async Task<IActionResult> InativarFamilia(int id)
        {
            try
            {
                bool resultado = await _familiaRepositorio.InativarFamilia(id);
                if (resultado)
                {
                    return Ok(new { Mensagem = "Família inativada com sucesso." });
                }
                return BadRequest("Erro ao inativar colaborador.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
}
