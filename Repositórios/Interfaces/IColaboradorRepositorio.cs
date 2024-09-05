using API_sis_conselhotutelarv2.Models;

namespace API_sis_conselhotutelarv2.Repositórios.Interfaces
{
    public interface IColaboradorRepositorio
    {
        Task<List<Colaborador>> BuscarTodosColaboradores();
        Task<Colaborador> BuscarColaboradorPorId(int id);
        Task<Colaborador> BuscarColaboradorPorUsuario(string usuario);
        Task<Colaborador> ObterColaboradorPorTokenAsync(string token);
        Task<int> ObterIdColaboradorPorNome(string nomeColaborador);
        Task<List<Colaborador>> ObterColaboradoresAtivosComSetor();
        Task<List<string>> ObterNomesCargosColaboradores();
        Task<int> ObterIdCargoPorNome(string nomeCargo);
        Task<string> ObterNomeCargoPorId(int idCargo);
        Task<int> ObterIdEmpresaPorNome(string nomeEmpresa);
        Task<List<string>> ObterNomesEmpresasColaboradores();
        Task<string> ObterNomeEmpresaPorId(int idEmpresa);
        Task<bool> VerificarEmailColaborador(string email);
        Task<LoginResponse> VerificarCredenciais(string username, string senha, string chaveValidade);
        Task<bool> AtualizarSenhaColaborador(Colaborador colaborador);
        Task<Colaborador> AdicionarColaborador(ColaboradorDto colaboradorDto);
        Task<Colaborador> AtualizarColaborador(ColaboradorEdicaoDto colaboradorDto, int id);
        Task<bool> AtivarColaborador(int id);
        Task<bool> InativarColaborador(int id);
    }
}
