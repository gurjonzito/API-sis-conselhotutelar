using API_sis_conselhotutelarv2.Models;

namespace API_sis_conselhotutelarv2.Repositórios.Interfaces
{
    public interface IColaboradorRepositorio
    {
        Task<List<Colaborador>> BuscarTodosColaboradores();
        Task<Colaborador> BuscarColaboradorPorId(int id);
        Task<int> ObterIdColaboradorPorNome(string nomeColaborador);
        Task<List<Colaborador>> ObterColaboradoresAtivosComSetor();
        Task<bool> VerificarEmailColaborador(string email);
        Task<Colaborador> AdicionarColaborador(Colaborador colaborador);
        Task<Colaborador> AtualizarColaborador(Colaborador colaborador, int id);
        Task<bool> DeletarColaborador(int id);
    }
}
