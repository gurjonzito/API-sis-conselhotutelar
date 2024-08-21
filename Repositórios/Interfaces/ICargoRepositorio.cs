using API_sis_conselhotutelarv2.Models;

namespace API_sis_conselhotutelarv2.Repositórios.Interfaces
{
    public interface ICargoRepositorio
    {
        Task<List<Cargo>> BuscarCargoPorId(int id);
        Task<Cargo> AdicionarCargo(Cargo cargo);
        Task<Cargo> AtualizarCargo(Cargo cargo, int id);
    }
}
