using Microsoft.EntityFrameworkCore;
using API_sis_conselhotutelarv2.Data;
using API_sis_conselhotutelarv2.Models;
using API_sis_conselhotutelarv2.Repositórios.Interfaces;
using API_sis_conselhotutelarv2.Enums;
using System.IdentityModel.Tokens.Jwt;

namespace API_sis_conselhotutelarv2.Repositórios
{
    public class ColaboradorRepositorio : IColaboradorRepositorio
    {
        private readonly ApplicationDbContext _context;
        private readonly EmpresaDbContext _empresaContext;
        private readonly TokenRepositorio _tokenRepositorio;

        public ColaboradorRepositorio(ApplicationDbContext applicationDbContext, EmpresaDbContext empresaDbContext, TokenRepositorio tokenRepositorio)
        {
            _context = applicationDbContext;
            _empresaContext = empresaDbContext;
            _tokenRepositorio = tokenRepositorio;
        }

        public async Task<Colaborador> BuscarColaboradorPorId(int id)
        {
            return await _context.Colaboradores
                .Include(c => c.Cargo) // Inclui o cargo, se necessário
                .FirstOrDefaultAsync(c => c.Col_Id == id);
        }
        public async Task<Colaborador> BuscarColaboradorPorUsuario(string usuario)
        {
            return await _context.Colaboradores.FirstOrDefaultAsync(c => c.Col_Username == usuario);
        }

        public async Task<Colaborador> ObterColaboradorPorTokenAsync(string token)
        {
            try
            {
                // Decodifica o token para obter o nome de usuário
                var username = _tokenRepositorio.DecodeTokenToGetUsername(token);

                if (string.IsNullOrEmpty(username))
                {
                    throw new Exception("Token inválido ou usuário não encontrado.");
                }

                // Busca o colaborador pelo nome de usuário
                var colaborador = await _context.Colaboradores
                    .Include(c => c.Cargo)
                    .FirstOrDefaultAsync(c => c.Col_Username == username);

                if (colaborador == null)
                {
                    throw new Exception("Colaborador não encontrado.");
                }

                return colaborador;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter colaborador pelo token: {ex.Message}", ex);
            }
        }


        public async Task<List<Colaborador>> BuscarTodosColaboradores()
        {
            return await _context.Colaboradores
                .Include(c => c.Cargo)
                .ToListAsync();
        }

        public async Task<int> ObterIdColaboradorPorNome(string nomeColab)
        {
            var colabId = await _context.Colaboradores
                .Where(c => c.Col_Nome.ToLower() == nomeColab.ToLower())
                .Select(c => c.Col_Id)
                .FirstOrDefaultAsync();

            if (colabId == 0)
            {
                throw new Exception($"Colaborador com o nome: {nomeColab} não foi encontrado.");
            }

            return colabId;
        }

        public async Task<List<Colaborador>> ObterColaboradoresAtivosComSetor()
        {
            return await _context.Colaboradores
                                 .Include(c => c.Cargo)
                                 .Where(c => c.Ativo_Inativo == 1) // Ajuste conforme a definição de "ativo" no seu contexto
                                 .ToListAsync();
        }

        public async Task<int> ObterIdCargoPorNome(string nomeCargo)
        {
            var cargoId = await _context.Cargos
                .Where(c => c.Car_Nome.ToLower() == nomeCargo.ToLower())
                .Select(c => c.Car_Id)
                .FirstOrDefaultAsync();

            if (cargoId == 0)
            {
                throw new Exception($"Cargo com o nome: {nomeCargo} não foi encontrado.");
            }

            return cargoId;
        }

        public async Task<string> ObterNomeCargoPorId(int idCargo)
        {
            var nomeCargo = await _context.Cargos
                .Where(c => c.Car_Id == idCargo)
                .Select(c => c.Car_Nome)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(nomeCargo))
            {
                throw new Exception($"Cargo com o ID: {idCargo} não foi encontrado.");
            }

            return nomeCargo;
        }

        public async Task<List<string>> ObterNomesCargosColaboradores()
        {
            return await _context.Cargos
                .Select(cargo => cargo.Car_Nome)
                .ToListAsync();
        }

        public async Task<int> ObterIdEmpresaPorNome(string nomeEmpresa)
        {
            var empresaId = await _empresaContext.Empresas
                .Where(c => c.Emp_RazaoSocial.ToLower() == nomeEmpresa.ToLower())
                .Select(c => c.Emp_Id)
                .FirstOrDefaultAsync();

            if (empresaId == 0)
            {
                throw new Exception($"Empresa com o nome: {nomeEmpresa} não foi encontrada.");
            }

            return empresaId;
        }

        public async Task<string> ObterNomeEmpresaPorId(int idEmpresa)
        {
            var nomeEmpresa = await _empresaContext.Empresas
                .Where(c => c.Emp_Id == idEmpresa)
                .Select(c => c.Emp_RazaoSocial)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(nomeEmpresa))
            {
                throw new Exception($"Empresa com o ID: {idEmpresa} não foi encontrada.");
            }

            return nomeEmpresa;
        }

        public async Task<List<string>> ObterNomesEmpresasColaboradores()
        {
            return await _empresaContext.Empresas
                .Select(e => e.Emp_RazaoSocial)
                .ToListAsync();
        }


        public async Task<bool> VerificarEmailColaborador(string email)
        {
            return await _context.Colaboradores.AnyAsync(c => c.Col_Email == email);
        }

        public async Task<bool> AtualizarSenhaColaborador(Colaborador colaborador)
        {
            _context.Colaboradores.Update(colaborador);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Colaborador> VerificarCredenciais(string username, string senha)
        {
            var colaborador = await _context.Colaboradores.SingleOrDefaultAsync(c => c.Col_Username == username);

            if (colaborador == null || !BCrypt.Net.BCrypt.Verify(senha, colaborador.Col_Senha))
            {
                return null;
            }

            return colaborador;
        }

        public async Task<Colaborador> AdicionarColaborador(ColaboradorDto colaboradorDto)
        {
            colaboradorDto.SetSenha(colaboradorDto.Col_Senha); // Hash da senha
            var colaborador = new Colaborador
            {
                Col_Nome = colaboradorDto.Col_Nome,
                Col_Username = colaboradorDto.Col_Username,
                Col_Email = colaboradorDto.Col_Email,
                Col_Telefone = colaboradorDto.Col_Telefone,
                Col_Senha = colaboradorDto.Col_Senha,
                Col_IdCargo = colaboradorDto.Col_IdCargo,
                Col_IdEmpresa = colaboradorDto.Col_IdEmpresa,
                Ativo_Inativo = colaboradorDto.Ativo_Inativo
            };

            _context.Colaboradores.Add(colaborador);
            await _context.SaveChangesAsync();

            return colaborador;
        }

        public async Task<Colaborador> AtualizarColaborador(ColaboradorEdicaoDto colaboradorDto, int id)
        {
            try
            {
                Colaborador colaboradorPorId = await _context.Colaboradores.FirstOrDefaultAsync(x => x.Col_Id == id);

                if (colaboradorPorId == null)
                {
                    throw new Exception($"Usuário para o Id: {id} não foi encontrado no banco de dados.");
                }

                colaboradorPorId.Col_Nome = colaboradorDto.Col_Nome ?? colaboradorPorId.Col_Nome;
                colaboradorPorId.Col_Username = colaboradorDto.Col_Username ?? colaboradorPorId.Col_Username;
                colaboradorPorId.Col_Email = colaboradorDto.Col_Email ?? colaboradorPorId.Col_Email;
                colaboradorPorId.Col_Telefone = colaboradorDto.Col_Telefone ?? colaboradorPorId.Col_Telefone;
                colaboradorPorId.Col_IdCargo = colaboradorDto.Col_IdCargo ?? colaboradorPorId.Col_IdCargo;
                colaboradorPorId.Col_IdEmpresa = colaboradorDto.Col_IdEmpresa ?? colaboradorPorId.Col_IdEmpresa;

                _context.Colaboradores.Update(colaboradorPorId);
                await _context.SaveChangesAsync();

                // Retorne o colaborador atualizado
                return colaboradorPorId;
            }
            catch (Exception ex)
            {
                // Log do erro
                Console.WriteLine($"Erro ao atualizar colaborador: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> AtivarColaborador(int id)
        {
            var colaborador = await _context.Colaboradores.FirstOrDefaultAsync(c => c.Col_Id == id);

            if (colaborador == null)
            {
                throw new Exception($"Colaborador com o ID: {id} não foi encontrado.");
            }

            colaborador.Ativo_Inativo = 1; // Define como ativo
            _context.Colaboradores.Update(colaborador);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> InativarColaborador(int id)
        {
            var colaborador = await _context.Colaboradores.FirstOrDefaultAsync(c => c.Col_Id == id);

            if (colaborador == null)
            {
                throw new Exception($"Colaborador com o ID: {id} não foi encontrado.");
            }

            colaborador.Ativo_Inativo = 0; // Define como inativo
            _context.Colaboradores.Update(colaborador);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
