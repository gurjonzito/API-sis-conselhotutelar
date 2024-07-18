using API_sis_conselhotutelarv2.Data;
using API_sis_conselhotutelarv2.Models;
using API_sis_conselhotutelarv2.Repositórios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_sis_conselhotutelarv2.Repositórios
{
    public class AtendimentoRepositorio : IAtendimentoRepositorio 
    {

        private readonly ApplicationDbContext _context;

        public AtendimentoRepositorio(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<Atendimento> BuscarAtendimentoPorId(int id)
        {
            return await _context.Atendimentos.FirstOrDefaultAsync(x => x.Ate_Id == id);

        }

        public async Task<List<Atendimento>> BuscarTodosAtendimentos()
        {
            return await _context.Atendimentos.ToListAsync();
        }

        public async Task<Atendimento> BuscarAtendimentoPorCodigo(string codigo)
        {
            return await _context.Atendimentos.FirstOrDefaultAsync(x => x.Ate_Codigo == codigo);
        }

        public async Task<List<string>> ObterNomesClientesAtendimento()
        {
            return await _context.Cidadaos
                .Select(cliente => cliente.Cli_Nome)
                .ToListAsync();
        }

        public async Task<List<string>> ObterNomesColaboradoresAtendimento()
        {
            return await _context.Colaboradores
                .Select(colaborador => colaborador.Col_Nome)
                .ToListAsync();
        }

        public async Task<int?> ObterIdClientePorNome(string nomeCliente)
        {
            var clienteId = await _context.Cidadaos
                .Where(c => c.Cli_Nome.Equals(nomeCliente, StringComparison.OrdinalIgnoreCase))
                .Select(c => c.Cli_Id)
                .FirstOrDefaultAsync();

            return clienteId == 0 ? (int?)null : clienteId;
        }

        public async Task<int> ObterIdColaboradorPorNome(string nomeColaborador)
        {
            var colaboradorId = await _context.Colaboradores
                .Where(c => c.Col_Nome.ToLower() == nomeColaborador.ToLower())
                .Select(c => c.Col_Id)
                .FirstOrDefaultAsync();

            if (colaboradorId == 0)
            {
                throw new Exception($"Colaborador com o nome: {nomeColaborador} não foi encontrado.");
            }

            return colaboradorId;
        }

        public async Task<List<Atendimento>> ObterAtendimentosComNomes()
        {
            var query = from a in _context.Atendimentos
                        join c in _context.Cidadaos on a.Ate_IdCliente equals c.Cli_Id into ac
                        from c in ac.DefaultIfEmpty()
                        join co in _context.Colaboradores on a.Ate_IdColaborador equals co.Col_Id into aco
                        from co in aco.DefaultIfEmpty()
                        select new Atendimento
                        {
                            Ate_Id = a.Ate_Id,
                            Ate_Codigo = a.Ate_Codigo,
                            Ate_Data = a.Ate_Data,
                            Ate_Status = a.Ate_Status,
                            NomeCidadao = c.Cli_Nome,
                            NomeAtendente = co.Col_Nome,
                            Ate_Descritivo = a.Ate_Descritivo
                        };

            return await query.ToListAsync();
        }


        public async Task<Atendimento> AdicionarAtendimento(Atendimento atendimento)
        {
            await _context.Atendimentos.AddAsync(atendimento);
            await _context.SaveChangesAsync();

            return atendimento;
        }

        public async Task<Atendimento> AtualizarAtendimento(Atendimento atendimento, int id)
        {
            Atendimento atendimentoPorId = await _context.Atendimentos.FirstOrDefaultAsync(x => x.Ate_Id == id);

            if (atendimentoPorId == null)
            {
                throw new Exception($"Atendimento para o Id: {id} não foi encontrado no banco de dados.");
            }

            atendimentoPorId.Ate_Codigo = atendimento.Ate_Codigo;
            atendimentoPorId.Ate_Data = atendimento.Ate_Data;
            atendimentoPorId.Ate_Status = atendimento.Ate_Status;
            atendimentoPorId.Ate_Descritivo = atendimento.Ate_Descritivo;
            atendimentoPorId.Ate_IdCliente = atendimento.Ate_IdCliente;
            atendimentoPorId.Ate_IdColaborador = atendimento.Ate_IdColaborador;

            _context.Atendimentos.Update(atendimentoPorId);
            await _context.SaveChangesAsync();

            return atendimentoPorId;
        }

        public async Task<bool> DeletarAtendimento(int id)
        {
            Atendimento atendimentoPorId = await _context.Atendimentos.FirstOrDefaultAsync(x => x.Ate_Id == id);

            if (atendimentoPorId == null)
            {
                throw new Exception($"Atendimento para o Id: {id} não foi encontrado no banco de dados.");
            }

            _context.Atendimentos.Remove(atendimentoPorId);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
