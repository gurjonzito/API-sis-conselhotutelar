using API_sis_conselhotutelarv2.Data;
using API_sis_conselhotutelarv2.Enums;
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

        public async Task<Atendimento> BuscarAtendimentoPorCodigoAsync(string codigo)
        {
            return await _context.Atendimentos
                .Where(x => x.Ate_Codigo == codigo)
                .Select(at => new Atendimento
                {
                    Ate_Id = at.Ate_Id,
                    Ate_Codigo = at.Ate_Codigo,
                    Ate_Data = at.Ate_Data,
                    Ate_Status = at.Ate_Status,
                    Ate_Descritivo = at.Ate_Descritivo,
                    Ate_IdCliente = at.Ate_IdCliente,
                    Ate_IdColaborador = at.Ate_IdColaborador,
                    NomeCidadao = at.Cidadao.Cli_Nome,
                    NomeAtendente = at.Colaborador.Col_Nome
                })
                .FirstOrDefaultAsync();
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
                .Where(c => c.Cli_Nome.ToLower() == nomeCliente.ToLower())
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

        public async Task<string> ObterNomeClientePorId(int idCliente)
        {
            var nomeCliente = await _context.Cidadaos
                .Where(c => c.Cli_Id == idCliente)
                .Select(c => c.Cli_Nome)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(nomeCliente))
            {
                throw new Exception($"Cliente com o ID: {idCliente} não foi encontrado.");
            }

            return nomeCliente;
        }

        public async Task<string> ObterNomeColaboradorPorId(int idColab)
        {
            var nomeColab = await _context.Colaboradores
                .Where(c => c.Col_Id == idColab)
                .Select(c => c.Col_Nome)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(nomeColab))
            {
                throw new Exception($"Colaborador com o ID: {idColab} não foi encontrado.");
            }

            return nomeColab;
        }

        public async Task<List<Atendimento>> ObterAtendimentosComNomes()
        {
            var query = from a in _context.Atendimentos
                        join c in _context.Cidadaos on a.Ate_IdCliente equals c.Cli_Id into ac
                        from c in ac.DefaultIfEmpty()
                        join co in _context.Colaboradores on a.Ate_IdColaborador equals co.Col_Id
                        select new Atendimento
                        {
                            Ate_Id = a.Ate_Id,
                            Ate_Codigo = a.Ate_Codigo,
                            Ate_Data = a.Ate_Data,
                            Ate_Status = a.Ate_Status,
                            NomeCidadao = c.Cli_Nome ?? "Desconhecido",
                            NomeAtendente = co.Col_Nome,
                            Ate_Descritivo = a.Ate_Descritivo
                        };

            return await query.ToListAsync();
        }


        public async Task<Atendimento> AdicionarAtendimento(AtendimentoDto atendimentoDto)
        {
            // Convertendo AtendimentoDto para Atendimento
            var atendimento = new Atendimento
            {
                Ate_Codigo = atendimentoDto.Ate_Codigo,
                Ate_Data = atendimentoDto.Ate_Data,
                Ate_Status = (StatusAtendimento)Enum.Parse(typeof(StatusAtendimento), atendimentoDto.Ate_Status),
                Ate_Descritivo = atendimentoDto.Ate_Descritivo,
                Ate_IdCliente = atendimentoDto.Ate_IdCliente,
                Ate_IdColaborador = atendimentoDto.Ate_IdColaborador
            };

            // Adicionar o atendimento ao banco de dados
            _context.Atendimentos.Add(atendimento);
            await _context.SaveChangesAsync();

            // Retornar o Atendimento após a inserção
            return atendimento;
        }


        public async Task<Atendimento> AtualizarAtendimento(AtendimentoEdicaoDto atendimentoDto, int id)
        {
            try
            {
                Atendimento atendimentoPorId = await _context.Atendimentos.FirstOrDefaultAsync(x => x.Ate_Id == id);

                if (atendimentoPorId == null)
                {
                    throw new Exception($"Atendimento para o Id: {id} não foi encontrado no banco de dados.");
                }

                atendimentoPorId.Ate_Codigo = atendimentoDto.Ate_Codigo ?? atendimentoPorId.Ate_Codigo;
                atendimentoPorId.Ate_Data = atendimentoDto.Ate_Data ?? atendimentoPorId.Ate_Data;
                atendimentoPorId.Ate_Status = atendimentoDto.Ate_Status ?? atendimentoPorId.Ate_Status;
                atendimentoPorId.Ate_Descritivo = atendimentoDto.Ate_Descritivo ?? atendimentoPorId.Ate_Descritivo;
                atendimentoPorId.Ate_IdCliente = atendimentoDto.Ate_IdCliente ?? atendimentoPorId.Ate_IdCliente;
                atendimentoPorId.Ate_IdColaborador = atendimentoDto.Ate_IdColaborador ?? atendimentoPorId.Ate_IdColaborador;

                _context.Atendimentos.Update(atendimentoPorId);
                await _context.SaveChangesAsync();

                return atendimentoPorId;
            }
            catch (Exception ex)
            {
                // Log do erro
                Console.WriteLine($"Erro ao atualizar atendimento: {ex.Message}");
                throw;
            }
        }
    }
}
