using API_sis_conselhotutelarv2.Data;
using API_sis_conselhotutelarv2.Models;
using API_sis_conselhotutelarv2.Repositórios.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection;

namespace API_sis_conselhotutelarv2.Repositórios
{
    public class EmpresaRepositorio : IEmpresaRepositorio
    {
        private readonly EmpresaDbContext _context;
        private readonly TokenRepositorio _tokenRepositorio;
        private readonly IDataProtector _protector;

        public EmpresaRepositorio(EmpresaDbContext empresaDbContext, TokenRepositorio tokenRepositorio, IDataProtectionProvider protectionProvider)
        {
            _context = empresaDbContext;
            _tokenRepositorio = tokenRepositorio;
            _protector = protectionProvider.CreateProtector("EmpConnectionProtector");
        }

        public async Task<Empresa> BuscarEmpresaPorId(int id)
        {
            return await _context.Empresas.FindAsync(id);
        }

        public async Task<Empresa> BuscarEmpresaPorChave(string chave)
        {
            var chaveValidade = await _context.ChavesValidade
                .Include(c => c.Empresa)
                .FirstOrDefaultAsync(c => c.Cha_Chave == chave);

            if (chaveValidade == null || chaveValidade.Cha_Validade < DateTime.UtcNow)
            {
                return null;
            }

            // Descriptografar a string de conexão antes de retornar
            chaveValidade.Empresa.Emp_Connection = _protector.Unprotect(chaveValidade.Empresa.Emp_Connection);

            return chaveValidade.Empresa;
        }

        public async Task<Empresa> AdicionarEmpresa(Empresa empresaDto)
        {
            if (empresaDto == null)
            {
                throw new ArgumentNullException(nameof(empresaDto));
            }

            var empresa = new Empresa
            {
                Emp_RazaoSocial = empresaDto.Emp_RazaoSocial,
                Emp_CNPJ = empresaDto.Emp_CNPJ,
                Emp_Telefone = empresaDto.Emp_Telefone,
                Ativo_Inativo = empresaDto.Ativo_Inativo,
                Emp_Connection = _protector.Protect(empresaDto.Emp_Connection) // Criptografar a string de conexão
            };

            _context.Empresas.Add(empresa);
            await _context.SaveChangesAsync();

            // Gerar chave de validade para a nova empresa
            var chave = _tokenRepositorio.GenerateEmpresaKey();

            var chaveValidade = new ChaveValidade
            {
                Cha_IdEmpresa = empresa.Emp_Id,
                Cha_Chave = chave, // Criptografar a chave de validade
                Cha_Validade = DateTime.UtcNow.AddYears(1) // Validade de 1 ano
            };

            _context.ChavesValidade.Add(chaveValidade);
            await _context.SaveChangesAsync();

            return empresa;
        }

        public async Task<bool> AtivarEmpresa(int id)
        {
            var empresa = await _context.Empresas.FirstOrDefaultAsync(c => c.Emp_Id == id);

            if (empresa == null)
            {
                throw new Exception($"Empresa com o ID: {id} não foi encontrada.");
            }

            empresa.Ativo_Inativo = 1; // Define como ativo
            _context.Empresas.Update(empresa);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> InativarEmpresa(int id)
        {
            var empresa = await _context.Empresas.FirstOrDefaultAsync(c => c.Emp_Id == id);

            if (empresa == null)
            {
                throw new Exception($"Empresa com o ID: {id} não foi encontrada.");
            }

            empresa.Ativo_Inativo = 0; // Define como inativo
            _context.Empresas.Update(empresa);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
