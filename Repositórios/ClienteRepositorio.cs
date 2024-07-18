using API_sis_conselhotutelarv2.Data;
using API_sis_conselhotutelarv2.Models;
using API_sis_conselhotutelarv2.Repositórios.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_sis_conselhotutelarv2.Repositórios
{
    public class ClienteRepositorio : IClienteRepositorio
    {
        private readonly ApplicationDbContext _context;

        public ClienteRepositorio(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<List<Cliente>> BuscarClientePorId(int id)
        {
            var cliente = await _context.Cidadaos.Where(x => x.Cli_Id == id).ToListAsync();
            return cliente;
        }

        public async Task<List<Cliente>> BuscarTodosClientes()
        {
            return await _context.Cidadaos.ToListAsync();
        }

        public async Task<List<Cliente>> ObterClientesAtivosComFamilia()
        {
            return await _context.Cidadaos
                                 .Include(c => c.Familia)
                                 .ToListAsync();
        }

        public async Task<bool> VerificarEmailCliente(string email)
        {
            return await _context.Cidadaos.AnyAsync(c => c.Cli_Email == email);
        }

        public async Task<Cliente> AdicionarCliente(Cliente cliente)
        {
            await _context.Cidadaos.AddAsync(cliente);
            await _context.SaveChangesAsync();

            return cliente;
        }

        public async Task<Cliente> AtualizarCliente(Cliente cliente, int id)
        {
            Cliente clientePorId = await _context.Cidadaos.FirstOrDefaultAsync(x => x.Cli_Id == id);

            if (clientePorId == null)
            {
                throw new Exception($"Cliente para o Id: {id} não foi encontrado no banco de dados.");
            }

            clientePorId.Cli_Nome = cliente.Cli_Nome;
            clientePorId.Cli_DataNasc = cliente.Cli_DataNasc;
            clientePorId.Cli_Telefone = cliente.Cli_Telefone;
            clientePorId.Cli_Email = cliente.Cli_Email;
            clientePorId.Cli_IdFamilia = cliente.Cli_IdFamilia;

            _context.Cidadaos.Update(clientePorId);
            await _context.SaveChangesAsync();

            return clientePorId;
        }

        public async Task<bool> DeletarCliente(int id)
        {
            Cliente clientePorId = await _context.Cidadaos.FirstOrDefaultAsync(x => x.Cli_Id == id);

            if (clientePorId == null)
            {
                throw new Exception($"Cliente para o Id: {id} não foi encontrado no banco de dados.");
            }

            _context.Cidadaos.Remove(clientePorId);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
