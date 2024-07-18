using API_sis_conselhotutelarv2.Models;

namespace API_sis_conselhotutelarv2.Repositórios.Interfaces
{
    public interface IFamiliaRepositorio
    {
        Task<List<Familia>> BuscarTodasFamilias();
        Task<Familia> BuscarFamiliaPorId(int id);
        Task<Familia> AdicionarFamilia(Familia familia);
        Task<Familia> AtualizarFamilia(Familia familia, int id);
        Task<bool> DeletarFamilia(int id);
    }
}
