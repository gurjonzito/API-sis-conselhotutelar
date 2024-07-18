using API_sis_conselhotutelarv2.Data;
using API_sis_conselhotutelarv2.Models;
using API_sis_conselhotutelarv2.Repositórios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_sis_conselhotutelarv2.Repositórios
{
    public class CargoRepositorio : ICargoRepositorio
    {
        private readonly ApplicationDbContext _context;

        public CargoRepositorio(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<List<Cargo>> BuscarCargoPorId(int id)
        {
            var cargo = await _context.Cargos.Where(x => x.Car_Id == id).ToListAsync();
            return cargo;
        }

        public async Task<Cargo> AdicionarCargo(Cargo cargo)
        {
            await _context.Cargos.AddAsync(cargo);
            await _context.SaveChangesAsync();

            return cargo;
        }

        public async Task<Cargo> AtualizarCargo(Cargo cargo, int id)
        {
            Cargo cargoPorId = await _context.Cargos.FirstOrDefaultAsync(x => x.Car_Id == id);

            if (cargoPorId == null)
            {
                throw new Exception($"Cargo para o Id: {id} não foi encontrado no banco de dados.");
            }

            cargoPorId.Car_Nome = cargo.Car_Nome;

            _context.Cargos.Update(cargoPorId);
            await _context.SaveChangesAsync();

            return cargoPorId;
        }

        public async Task<bool> DeletarCargo(int id)
        {
            Cargo cargoPorId = await _context.Cargos.FirstOrDefaultAsync(x => x.Car_Id == id);

            if (cargoPorId == null)
            {
                throw new Exception($"Cargo para o Id: {id} não foi encontrado no banco de dados.");
            }

            _context.Cargos.Remove(cargoPorId);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
