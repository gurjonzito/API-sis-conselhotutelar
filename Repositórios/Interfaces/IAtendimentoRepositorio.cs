using API_sis_conselhotutelarv2.Models;

namespace API_sis_conselhotutelarv2.Repositórios.Interfaces
{
    public interface IAtendimentoRepositorio
    {
        Task<List<Atendimento>> BuscarTodosAtendimentos();
        Task<Atendimento> BuscarAtendimentoPorId(int id);
        Task<Atendimento> BuscarAtendimentoPorCodigo(string codigo);
        Task<List<string>> ObterNomesClientesAtendimento();
        Task<List<string>> ObterNomesColaboradoresAtendimento();
        Task<int?> ObterIdClientePorNome(string nomeCliente);
        Task<int> ObterIdColaboradorPorNome(string nomeColaborador);
        Task<List<Atendimento>> ObterAtendimentosComNomes();
        Task<Atendimento> AdicionarAtendimento(Atendimento atendimento);
        Task<Atendimento> AtualizarAtendimento(Atendimento atendimento, int id);
        Task<bool> DeletarAtendimento(int id);
    }
}
