using Microsoft.EntityFrameworkCore;
using API_sis_conselhotutelarv2.Data;
using API_sis_conselhotutelarv2.Models;
using API_sis_conselhotutelarv2.Repositórios.Interfaces;

namespace API_sis_conselhotutelarv2.Repositórios
{
    public class ColaboradorRepositorio : IColaboradorRepositorio
    {
        private readonly ApplicationDbContext _context;

        public ColaboradorRepositorio(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<Colaborador> BuscarColaboradorPorId(int id)
        {
            return await _context.Colaboradores
                .Include(c => c.Cargo) // Inclui o cargo, se necessário
                .FirstOrDefaultAsync(c => c.Col_Id == id);
        }

        public async Task<List<Colaborador>> BuscarTodosColaboradores()
        {
            return await _context.Colaboradores.ToListAsync();
        }

        public async Task<int> ObterIdColaboradorPorNome(string nomeColaborador)
        {
            var colaborador = await _context.Colaboradores.FirstOrDefaultAsync(c => c.Col_Nome == nomeColaborador);
            return colaborador?.Col_Id ?? -1;
        }

        public async Task<List<Colaborador>> ObterColaboradoresAtivosComSetor()
        {
            return await _context.Colaboradores
                                 .Include(c => c.Cargo)
                                 .Where(c => c.Cargo != null) // Ajuste conforme a definição de "ativo" no seu contexto
                                 .ToListAsync();
        }

        public async Task<bool> VerificarEmailColaborador(string email)
        {
            return await _context.Colaboradores.AnyAsync(c => c.Col_Email == email);
        }

        public async Task<Colaborador> AdicionarColaborador(Colaborador colaborador)
        {
            colaborador.SetSenha(colaborador.Col_Senha); // Hash da senha
            await _context.Colaboradores.AddAsync(colaborador);
            await _context.SaveChangesAsync();
            return colaborador;
        }

        public async Task<Colaborador> AtualizarColaborador(Colaborador colaborador, int id)
        {
            Colaborador colaboradorPorId = await _context.Colaboradores.FirstOrDefaultAsync(x => x.Col_Id == id);

            if (colaboradorPorId == null)
            {
                throw new Exception($"Usuário para o Id: {id} não foi encontrado no banco de dados.");
            }

            colaboradorPorId.Col_Nome = colaborador.Col_Nome;
            colaboradorPorId.Col_Username = colaborador.Col_Username;
            colaboradorPorId.Col_Email = colaborador.Col_Email;
            colaboradorPorId.Col_Telefone = colaborador.Col_Telefone;
            colaboradorPorId.Col_Senha = colaborador.Col_Senha;
            colaboradorPorId.Col_IdCargo = colaborador.Col_IdCargo;

            _context.Colaboradores.Update(colaboradorPorId);
            await _context.SaveChangesAsync();

            return colaboradorPorId;
        }

        public async Task<bool> DeletarColaborador(int id)
        {
            Colaborador colaboradorPorId = await _context.Colaboradores.FirstOrDefaultAsync(x => x.Col_Id == id);

            if (colaboradorPorId == null)
            {
                throw new Exception($"Usuário para o Id: {id} não foi encontrado no banco de dados.");
            }

            _context.Colaboradores.Remove(colaboradorPorId);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
