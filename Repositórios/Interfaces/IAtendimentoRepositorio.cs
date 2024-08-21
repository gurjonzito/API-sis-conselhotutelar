using API_sis_conselhotutelarv2.Models;

namespace API_sis_conselhotutelarv2.Repositórios.Interfaces
{
    public interface IAtendimentoRepositorio
    {
        Task<List<Atendimento>> BuscarTodosAtendimentos();
        Task<Atendimento> BuscarAtendimentoPorId(int id);
        Task<Atendimento> BuscarAtendimentoPorCodigoAsync(string codigo);
        Task<List<string>> ObterNomesClientesAtendimento();
        Task<List<string>> ObterNomesColaboradoresAtendimento();
        Task<int?> ObterIdClientePorNome(string nomeCliente);
        Task<int> ObterIdColaboradorPorNome(string nomeColaborador);
        Task<string> ObterNomeClientePorId(int idCliente);
        Task<string> ObterNomeColaboradorPorId(int idColab);
        Task<List<Atendimento>> ObterAtendimentosComNomes();
        Task<Atendimento> AdicionarAtendimento(AtendimentoDto atendimentoDto);
        Task<Atendimento> AtualizarAtendimento(AtendimentoEdicaoDto atendimento, int id);
    }
}
