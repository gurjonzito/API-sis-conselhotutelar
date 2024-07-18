using API_sis_conselhotutelarv2.Data;
using API_sis_conselhotutelarv2.Models;
using API_sis_conselhotutelarv2.Repositórios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_sis_conselhotutelarv2.Repositórios
{
    public class FamiliaRepositorio : IFamiliaRepositorio
    {

        private readonly ApplicationDbContext _context;

        public FamiliaRepositorio(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<Familia> BuscarFamiliaPorId(int id)
        {
            return await _context.Familias.FirstOrDefaultAsync(f => f.Fam_Id == id);
        }

        public async Task<List<Familia>> BuscarTodasFamilias()
        {
            return await _context.Familias.ToListAsync();
        }

        public async Task<Familia> AdicionarFamilia(Familia familia)
        {
            await _context.Familias.AddAsync(familia);
            await _context.SaveChangesAsync();

            return familia;
        }

        public async Task<Familia> AtualizarFamilia(Familia familia, int id)
        {
            Familia familiaPorId = await _context.Familias.FirstOrDefaultAsync(x => x.Fam_Id == id);

            if (familiaPorId == null)
            {
                throw new Exception($"Família para o Id: {id} não foi encontrada no banco de dados.");
            }

            familiaPorId.Fam_Sobrenomes = familia.Fam_Sobrenomes;
            familiaPorId.Fam_Responsavel = familia.Fam_Responsavel;
            familiaPorId.Fam_Participantes = familia.Fam_Participantes;

            _context.Familias.Update(familiaPorId);
            await _context.SaveChangesAsync();

            return familiaPorId;
        }

        public async Task<bool> DeletarFamilia(int id)
        {
            Familia familiaPorId = await _context.Familias.FirstOrDefaultAsync(x => x.Fam_Id == id);

            if (familiaPorId == null)
            {
                throw new Exception($"Família para o Id: {id} não foi encontrada no banco de dados.");
            }

            _context.Familias.Remove(familiaPorId);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
