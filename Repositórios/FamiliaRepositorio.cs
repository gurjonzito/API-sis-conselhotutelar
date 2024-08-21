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

        public async Task<int> ObterIdFamiliaPorNome(string nomeFamilia)
        {
            var familiaId = await _context.Familias
                .Where(c => c.Fam_Sobrenomes.ToLower() == nomeFamilia.ToLower())
                .Select(c => c.Fam_Id)
                .FirstOrDefaultAsync();

            if (familiaId == 0)
            {
                throw new Exception($"Família com o sobrenome: {nomeFamilia} não foi encontrado.");
            }

            return familiaId;
        }

        public async Task<Familia> AdicionarFamilia(Familia familia)
        {
            await _context.Familias.AddAsync(familia);
            await _context.SaveChangesAsync();

            return familia;
        }

        public async Task<Familia> AtualizarFamilia(FamiliaEdicaoDto familiaDto, int id)
        {
            try
            {
                Familia familiaPorId = await _context.Familias.FirstOrDefaultAsync(x => x.Fam_Id == id);

                if (familiaPorId == null)
                {
                    throw new Exception($"Família para o ID: {id} não foi encontrado no banco de dados.");
                }

                familiaPorId.Fam_Sobrenomes = familiaDto.Fam_Sobrenomes ?? familiaPorId.Fam_Sobrenomes;
                familiaPorId.Fam_Responsavel = familiaDto.Fam_Responsavel ?? familiaPorId.Fam_Responsavel;
                familiaPorId.Fam_Participantes = familiaDto.Fam_Participantes ?? familiaPorId.Fam_Participantes;

                _context.Familias.Update(familiaPorId);
                await _context.SaveChangesAsync();

                return familiaPorId;
            }
            catch (Exception ex)
            {
                // Log do erro
                Console.WriteLine($"Erro ao atualizar família: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> AtivarFamilia(int id)
        {
            var familia = await _context.Familias.FirstOrDefaultAsync(c => c.Fam_Id == id);

            if (familia == null)
            {
                throw new Exception($"Família com o ID: {id} não foi encontrada.");
            }

            familia.Ativo_Inativo = 1; // Define como ativo
            _context.Familias.Update(familia);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> InativarFamilia(int id)
        {
            var familia = await _context.Familias.FirstOrDefaultAsync(c => c.Fam_Id == id);

            if (familia == null)
            {
                throw new Exception($"Família com o ID: {id} não foi encontrada.");
            }

            familia.Ativo_Inativo = 0; // Define como inativo
            _context.Familias.Update(familia);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
