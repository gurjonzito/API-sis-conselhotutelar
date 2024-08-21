using API_sis_conselhotutelarv2.Data;
using API_sis_conselhotutelarv2.Models;
using API_sis_conselhotutelarv2.Repositórios;
using API_sis_conselhotutelarv2.Repositórios.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_sis_conselhotutelarv2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly IEmpresaRepositorio _empresaRepositorio;
        private readonly TokenRepositorio _tokenRepositorio;
        private readonly EmpresaDbContext _context;

        public EmpresaController(IEmpresaRepositorio empresaRepositorio, TokenRepositorio tokenRepositorio, EmpresaDbContext empresaDbContext)
        {
            _empresaRepositorio = empresaRepositorio;
            _tokenRepositorio = tokenRepositorio;
            _context = empresaDbContext;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Empresa>>> BuscarEmpresaPorId(int id)
        {
            var empresa = await _empresaRepositorio.BuscarEmpresaPorId(id);
            if (empresa == null)
            {
                return NotFound();
            }
            return Ok(empresa);
        }

        [HttpPost]
        public async Task<ActionResult<EmpresaResponseDto<Empresa>>> AdicionarEmpresa([FromBody] Empresa empresaModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new EmpresaResponseDto<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "Dados inválidos",
                    ChaveDeValidade = null
                });
            }

            var empresa = await _empresaRepositorio.AdicionarEmpresa(empresaModel);

            if (empresa != null)
            {
                // Recuperar a chave de validade gerada após o cadastro da empresa
                var chaveValidade = await _context.ChavesValidade
                                                  .FirstOrDefaultAsync(c => c.Cha_IdEmpresa == empresa.Emp_Id);

                return Ok(new EmpresaResponseDto<Empresa>
                {
                    Data = empresa,
                    Success = true,
                    Message = "Empresa cadastrada com sucesso",
                    ChaveDeValidade = chaveValidade?.Cha_Chave
                });
            }

            return StatusCode(500, new EmpresaResponseDto<bool>
            {
                Data = false,
                Success = false,
                Message = "Erro ao cadastrar empresa",
                ChaveDeValidade = null
            });
        }



        [HttpGet("chave/{chave}")]
        public async Task<IActionResult> BuscarEmpresaPorChave(string chave)
        {
            var empresa = await _empresaRepositorio.BuscarEmpresaPorChave(chave);
            if (empresa == null)
            {
                return NotFound("Chave de validade inválida.");
            }
            return Ok(empresa);
        }

        [HttpPut("ativar/{id}")]
        public async Task<IActionResult> AtivarEmpresa(int id)
        {
            try
            {
                bool resultado = await _empresaRepositorio.AtivarEmpresa(id);
                if (resultado)
                {
                    return Ok(new { Mensagem = "Empresa ativada com sucesso." });
                }
                return BadRequest("Erro ao ativar empresa.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpPut("inativar/{id}")]
        public async Task<IActionResult> InativarEmpresa(int id)
        {
            try
            {
                bool resultado = await _empresaRepositorio.InativarEmpresa(id);
                if (resultado)
                {
                    return Ok(new { Mensagem = "Empresa inativada com sucesso." });
                }
                return BadRequest("Erro ao inativar empresa.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
}
