using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_sis_conselhotutelarv2.Models;
using API_sis_conselhotutelarv2.Repositórios.Interfaces;
using API_sis_conselhotutelarv2.Repositórios;

namespace API_sis_conselhotutelarv2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColaboradorController : ControllerBase
    {
        private readonly IColaboradorRepositorio _colaboradorRepositorio;
        private readonly ICargoRepositorio _cargoRepositorio;

        public ColaboradorController(IColaboradorRepositorio colaboradorRepositorio, ICargoRepositorio cargoRepositorio) 
        {
            _colaboradorRepositorio = colaboradorRepositorio;
            _cargoRepositorio = cargoRepositorio;
        }

        [HttpGet]
        public async Task<ActionResult<List<Colaborador>>> BuscarTodosColaboradores()
        {
            List<Colaborador> colaboradores = await _colaboradorRepositorio.BuscarTodosColaboradores();
            return Ok(colaboradores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Colaborador>> BuscarColaboradorPorId(int id)
        {
            Colaborador colaborador = await _colaboradorRepositorio.BuscarColaboradorPorId(id);
            return Ok(colaborador);
        }

        [HttpGet("colaborador/{id}")]
        public async Task<ActionResult<Colaborador>> BuscarColaboradorComCargo(int id)
        {
            var colaborador = await _colaboradorRepositorio.BuscarColaboradorPorId(id);
            if (colaborador == null)
            {
                return NotFound("Colaborador não encontrado.");
            }

            // O cargo já deve estar incluído se você usar Include no repositório
            return Ok(colaborador);
        }


        [HttpGet("obter-id/{nomeColaborador}")]
        public async Task<ActionResult<int>> ObterIdColaboradorPorNome(string nomeColaborador)
        {
            int idColaborador = await _colaboradorRepositorio.ObterIdColaboradorPorNome(nomeColaborador);
            if (idColaborador == -1)
            {
                return NotFound("Colaborador não encontrado.");
            }
            return Ok(idColaborador);
        }

        [HttpGet("ativos-com-setor")]
        public async Task<ActionResult<List<Colaborador>>> ObterColaboradoresAtivosComSetor()
        {
            List<Colaborador> colaboradores = await _colaboradorRepositorio.ObterColaboradoresAtivosComSetor();
            return Ok(colaboradores);
        }

        [HttpGet("verificar-email/{email}")]
        public async Task<ActionResult<bool>> VerificarEmailColaborador(string email)
        {
            bool emailExists = await _colaboradorRepositorio.VerificarEmailColaborador(email);
            return Ok(emailExists);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Colaborador>> AtualizarColaborador(int id, [FromBody] Colaborador colaboradorModel)
        {
            try
            {
                Colaborador colaborador = await _colaboradorRepositorio.AtualizarColaborador(colaboradorModel, id);
                return Ok(colaborador);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Colaborador>> CadastrarColaborador([FromBody] Colaborador colaboradorModel)
        {
            Colaborador colaborador = await _colaboradorRepositorio.AdicionarColaborador(colaboradorModel);
            return Ok(colaborador);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Colaborador>> ValidarCredenciais([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.Col_Username) || string.IsNullOrWhiteSpace(loginRequest.Col_Senha))
            {
                return BadRequest("Usuário ou senha inválidos.");
            }

            var usuario = await _colaboradorRepositorio.BuscarTodosColaboradores();

            var colaborador = usuario.FirstOrDefault(c => c.Col_Username == loginRequest.Col_Username);

            if (colaborador == null || !colaborador.VerificarSenha(loginRequest.Col_Senha))
            {
                return Unauthorized("Usuário ou senha incorretos.");
            }

            return Ok(colaborador); // Retorna o colaborador se as credenciais forem válidas
        }
    }
}
