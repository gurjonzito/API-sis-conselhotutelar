using API_sis_conselhotutelarv2.Models;

namespace API_sis_conselhotutelarv2.Repositórios.Interfaces
{
    public interface IClienteRepositorio
    {
        Task<List<Cliente>> BuscarTodosClientes();
        Task<List<Cliente>> BuscarClientePorId(int id);
        Task<List<Cliente>> ObterClientesAtivosComFamilia();
        Task<int> ObterIdClientePorNome(string nomeCliente);
        Task<string> ObterNomeFamiliaPorId(int idFamilia);
        Task<List<string>> ObterNomesFamiliasClientes();
        Task<int?> ObterIdFamiliaPorNome(string nomeFamilia);
        Task<bool> VerificarEmailCliente(string email);
        Task<Cliente> AdicionarCliente(ClienteDto cliente);
        Task<Cliente> AtualizarCliente(ClienteEdicaoDto clienteDto, int id);
        Task<bool> AtivarCliente(int id);
        Task<bool> InativarCliente(int id);
    }
}
