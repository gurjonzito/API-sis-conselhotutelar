using API_sis_conselhotutelarv2.Models;

namespace API_sis_conselhotutelarv2.Repositórios.Interfaces
{
    public interface IClienteRepositorio
    {
        Task<List<Cliente>> BuscarTodosClientes();
        Task<List<Cliente>> BuscarClientePorId(int id);
        Task<List<Cliente>> ObterClientesAtivosComFamilia();
        Task<bool> VerificarEmailCliente(string email);
        Task<Cliente> AdicionarCliente(Cliente cliente);
        Task<Cliente> AtualizarCliente(Cliente cliente, int id);
        Task<bool> DeletarCliente(int id);
    }
}
