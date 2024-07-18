using API_sis_conselhotutelarv2.Enums;
using API_sis_conselhotutelarv2.Models;
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

        [HttpGet("{codigo}")]
        public async Task<ActionResult<Atendimento>> BuscarAtendimentoPorCodigo(string codigo)
        {
            var atendimento = await _atendimentoRepositorio.BuscarAtendimentoPorCodigo(codigo);
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
            var clienteId = await _atendimentoRepositorio.ObterIdClientePorNome(nomeCliente);
            if (clienteId == null)
            {
                return NotFound("Cliente não encontrado");
            }
            return Ok(clienteId);
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

            // Converta o DTO em um modelo de atendimento, se necessário
            var atendimento = new Atendimento
            {
                Ate_Codigo = atendimentoModel.Ate_Codigo,
                Ate_Data = atendimentoModel.Ate_Data,
                Ate_Status = (StatusAtendimento)Enum.Parse(typeof(StatusAtendimento), atendimentoModel.Ate_Status),
                Ate_Descritivo = atendimentoModel.Ate_Descritivo,
                Ate_IdCliente = atendimentoModel.Ate_IdCliente,
                Ate_IdColaborador = atendimentoModel.Ate_IdColaborador
                // Não inclua NomeCidadao e NomeAtendente, pois são opcionais
            };

            // Adicionar o atendimento ao repositório
            atendimento = await _atendimentoRepositorio.AdicionarAtendimento(atendimento);
            return Ok(atendimento);
        }
    }
}
