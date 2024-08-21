using API_sis_conselhotutelarv2.Models;

namespace API_sis_conselhotutelarv2.Repositórios.Interfaces
{
    public interface IFamiliaRepositorio
    {
        Task<List<Familia>> BuscarTodasFamilias();
        Task<Familia> BuscarFamiliaPorId(int id);
        Task<int> ObterIdFamiliaPorNome(string nomeFamilia);
        Task<Familia> AdicionarFamilia(Familia familia);
        Task<Familia> AtualizarFamilia(FamiliaEdicaoDto familiaDto, int id);
        Task<bool> AtivarFamilia(int id);
        Task<bool> InativarFamilia(int id);
    }
}
