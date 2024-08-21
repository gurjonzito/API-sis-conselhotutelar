using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_sis_conselhotutelarv2.Models;
using API_sis_conselhotutelarv2.Repositórios.Interfaces;
using API_sis_conselhotutelarv2.Repositórios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace API_sis_conselhotutelarv2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColaboradorController : ControllerBase
    {
        private readonly IColaboradorRepositorio _colaboradorRepositorio;
        private readonly ICargoRepositorio _cargoRepositorio;
        private readonly IConfiguration _configuration;
        private readonly TokenRepositorio _tokenRepositorio;

        public ColaboradorController(IColaboradorRepositorio colaboradorRepositorio, ICargoRepositorio cargoRepositorio, IConfiguration configuration, TokenRepositorio tokenRepositorio)
        {
            _colaboradorRepositorio = colaboradorRepositorio;
            _cargoRepositorio = cargoRepositorio;
            _configuration = configuration;
            _tokenRepositorio = tokenRepositorio;
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

        [HttpGet("usuario")]
        public async Task<ActionResult<Colaborador>> BuscarColaboradorPorUsuario(string usuario)
        {
            Colaborador colaborador = await _colaboradorRepositorio.BuscarColaboradorPorUsuario(usuario);
            if (colaborador == null)
            {
                return NotFound("Colaborador não encontrado.");
            }
            return Ok(colaborador);
        }

        [HttpGet("por-token")]
        public async Task<IActionResult> ObterColaboradorPorToken([FromQuery] string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token não fornecido.");
            }

            try
            {
                var colaborador = await _colaboradorRepositorio.ObterColaboradorPorTokenAsync(token);
                if (colaborador == null)
                {
                    return NotFound("Colaborador não encontrado.");
                }

                return Ok(colaborador);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
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


        [HttpGet("ObterIdColaboradorPorNome/{nomeColab}")]
        public async Task<IActionResult> ObterIdColaboradorPorNome(string nomeColab)
        {
            try
            {
                var colabId = await _colaboradorRepositorio.ObterIdColaboradorPorNome(nomeColab);
                return Ok(colabId);
            }
            catch (Exception ex)
            {
                return NotFound("Colaborador não encontrado: " + ex.Message);
            }
        }

        [HttpGet("ObterIdCargoPorNome/{nomeCargo}")]
        public async Task<IActionResult> ObterIdCargoPorNome(string nomeCargo)
        {
            try
            {
                var cargoId = await _colaboradorRepositorio.ObterIdCargoPorNome(nomeCargo);
                return Ok(cargoId);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("ObterNomeCargoPorId/{idCargo}")]
        public async Task<IActionResult> ObterNomeCargoPorId(int idCargo)
        {
            try
            {
                var nomeCargo = await _colaboradorRepositorio.ObterNomeCargoPorId(idCargo);
                return Ok(nomeCargo);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("cargos-nomes")]
        public async Task<ActionResult<List<string>>> ObterNomesCargos()
        {
            var nomes = await _colaboradorRepositorio.ObterNomesCargosColaboradores();
            return Ok(nomes);
        }

        [HttpGet("ObterIdEmpresaPorNome/{nomeEmpresa}")]
        public async Task<IActionResult> ObterIdEmpresaPorNome(string nomeEmpresa)
        {
            try
            {
                var empresaId = await _colaboradorRepositorio.ObterIdEmpresaPorNome(nomeEmpresa);
                return Ok(empresaId);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("ObterNomeEmpresaPorId/{idEmpresa}")]
        public async Task<IActionResult> ObterNomeEmpresaPorId(int idEmpresa)
        {
            try
            {
                var nomeEmpresa = await _colaboradorRepositorio.ObterNomeEmpresaPorId(idEmpresa);
                return Ok(nomeEmpresa);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("empresas-nomes")]
        public async Task<ActionResult<List<string>>> ObterNomesEmpresas()
        {
            var nomes = await _colaboradorRepositorio.ObterNomesEmpresasColaboradores();
            return Ok(nomes);
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

        [HttpPut("atualizar-senha/{id}")]
        public async Task<IActionResult> AtualizarSenhaColaborador(int id, [FromBody] AtualizarSenhaDto atualizarSenhaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var colaborador = await _colaboradorRepositorio.BuscarColaboradorPorId(id);
                if (colaborador == null)
                {
                    return NotFound("Colaborador não encontrado.");
                }

                // Verifique se a senha antiga está correta
                if (!BCrypt.Net.BCrypt.Verify(atualizarSenhaDto.SenhaAntiga, colaborador.Col_Senha))
                {
                    return Unauthorized("Senha antiga incorreta.");
                }

                // Criptografe a nova senha
                colaborador.Col_Senha = BCrypt.Net.BCrypt.HashPassword(atualizarSenhaDto.NovaSenha);

                // Atualize a senha no banco de dados
                await _colaboradorRepositorio.AtualizarSenhaColaborador(colaborador);

                return Ok("Senha alterada com sucesso!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar a senha: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarColaborador(int id, [FromBody] ColaboradorEdicaoDto colaboradorDto)
        {
            if (id <= 0 || colaboradorDto == null)
            {
                return BadRequest("Dados inválidos.");
            }

            try
            {
                var colaboradorAtualizado = await _colaboradorRepositorio.AtualizarColaborador(colaboradorDto, id);
                if (colaboradorAtualizado == null)
                {
                    return NotFound("Colaborador não encontrado.");
                }

                var apiResponse = new ApiResponse<Colaborador>
                {
                    Data = colaboradorAtualizado,
                    Success = true,
                    Message = "Colaborador atualizado com sucesso."
                };

                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar colaborador: {ex.Message}");
                var apiResponse = new ApiResponse<Colaborador>
                {
                    Data = null,
                    Success = false,
                    Message = ex.Message
                };

                return StatusCode(StatusCodes.Status500InternalServerError, apiResponse);
            }
        }


        [HttpPost]
        public async Task<ActionResult<Colaborador>> CadastrarColaborador([FromBody] ColaboradorDto colaboradorModel)
        {
            // Verifique a validade do modelo
            if (!ModelState.IsValid)
            {
                // Remover campos não obrigatórios da validação
                ModelState.Remove("Cargo");

                // Retornar erros de validação, se houver
                return BadRequest(ModelState);
            }

            // Adicionar o atendimento ao repositório utilizando o DTO
            var colaborador = await _colaboradorRepositorio.AdicionarColaborador(colaboradorModel);
            if (colaborador != null)
            {
                return Ok(new ApiResponse<Colaborador>
                {
                    Data = colaborador,
                    Success = true,
                    Message = "Colaborador cadastrado com sucesso"
                });
            }

            return StatusCode(500, new ApiResponse<bool>
            {
                Data = false,
                Success = false,
                Message = "Erro ao cadastrar colaborador"
            });
        }

        [HttpPut("ativar/{id}")]
        public async Task<IActionResult> AtivarColaborador(int id)
        {
            try
            {
                bool resultado = await _colaboradorRepositorio.AtivarColaborador(id);
                if (resultado)
                {
                    return Ok(new { Mensagem = "Colaborador ativado com sucesso." });
                }
                return BadRequest("Erro ao ativar colaborador.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpPut("inativar/{id}")]
        public async Task<IActionResult> InativarColaborador(int id)
        {
            try
            {
                bool resultado = await _colaboradorRepositorio.InativarColaborador(id);
                if (resultado)
                {
                    return Ok(new { Mensagem = "Colaborador inativado com sucesso." });
                }
                return BadRequest("Erro ao inativar colaborador.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }


        /*private async Task<Colaborador> BuscarColaboradorPorEmail(string email)
        {
            return await _colaboradorRepositorio.BuscarColaboradorPorEmail(email);
        }

        [HttpPost("enviar-token")]
        public async Task<IActionResult> EnviarTokenParaEmailCadastrado([FromBody] EmailRequest request)
        {
            var colaborador = await BuscarColaboradorPorEmail(request.Email);
            if (colaborador == null)
            {
                return NotFound("Colaborador não encontrado.");
            }

            // Gere uma senha temporária ou token
            var token = GerarTokenTemporario();

            // Envie o token para o e-mail do colaborador
            var subject = "Seu token temporário";
            var body = $"Seu token temporário é: {token}";

            try
            {
                await _emailService.SendEmailAsync(colaborador.Col_Email, subject, body);
                return Ok("Token enviado com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao enviar o e-mail: {ex.Message}");
            }
        }

        private string GerarTokenTemporario()
        {
            // Gere um token temporário simples (aqui estamos usando um GUID, mas você pode usar outra lógica)
            return Guid.NewGuid().ToString();
        }
    }

    public class EmailRequest
    {
        public string Email { get; set; }
    }*/

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginDto)
        {
            var colaborador = await _colaboradorRepositorio.BuscarColaboradorPorUsuario(loginDto.Col_Username);

            if (colaborador == null)
            {
                Console.WriteLine($"Login falhou: usuário {loginDto.Col_Username} não encontrado.");
                return Unauthorized("Usuário ou senha incorretos.");
            }

            // Verificar a senha
            bool senhaValida = BCrypt.Net.BCrypt.Verify(loginDto.Col_Senha, colaborador.Col_Senha);
            if (!senhaValida)
            {
                Console.WriteLine($"Login falhou: senha incorreta para usuário {loginDto.Col_Username}.");
                return Unauthorized("Usuário ou senha incorretos.");
            }

            // Gerar token JWT
            var token = _tokenRepositorio.GenerateToken(colaborador);

            Console.WriteLine($"Login bem-sucedido para usuário {loginDto.Col_Username}.");
            return Ok(new { Token = token });
        }
    }
}

