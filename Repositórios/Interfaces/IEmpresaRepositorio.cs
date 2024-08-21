using API_sis_conselhotutelarv2.Models;

namespace API_sis_conselhotutelarv2.Repositórios.Interfaces
{
    public interface IEmpresaRepositorio
    {
        Task<Empresa> BuscarEmpresaPorId(int id);
        Task<Empresa> BuscarEmpresaPorChave(string chave);
        Task<Empresa> AdicionarEmpresa(Empresa empresa);
        //Task<Empresa> AtualizarEmpresa(Empresa empresa, int id);
        Task<bool> AtivarEmpresa(int id);
        Task<bool> InativarEmpresa(int id);
    }
}
