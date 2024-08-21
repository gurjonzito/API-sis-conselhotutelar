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
            return await _context.Cidadaos
                .Include(c => c.Familia)
                .ToListAsync();
        }

        public async Task<List<Cliente>> ObterClientesAtivosComFamilia()
        {
            return await _context.Cidadaos
                                 .Include(c => c.Familia)
                                 .ToListAsync();
        }

        public async Task<int> ObterIdClientePorNome(string nomeCliente)
        {
            var colabId = await _context.Cidadaos
                .Where(c => c.Cli_Nome.ToLower() == nomeCliente.ToLower())
                .Select(c => c.Cli_Id)
                .FirstOrDefaultAsync();

            if (colabId == 0)
            {
                throw new Exception($"Cliente com o nome: {nomeCliente} não foi encontrado.");
            }

            return colabId;
        }

        public async Task<string> ObterNomeFamiliaPorId(int idFamilia)
        {
            var nomeFamilia = await _context.Familias
                .Where(c => c.Fam_Id == idFamilia)
                .Select(c => c.Fam_Sobrenomes)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(nomeFamilia))
            {
                throw new Exception($"Família com o ID: {idFamilia} não foi encontrada.");
            }

            return nomeFamilia;
        }

        public async Task<List<string>> ObterNomesFamiliasClientes()
        {
            return await _context.Familias
                .Select(cliente => cliente.Fam_Sobrenomes)
                .ToListAsync();
        }

        public async Task<int?> ObterIdFamiliaPorNome(string nomeFamilia)
        {
            var familiaId = await _context.Familias
                .Where(c => c.Fam_Sobrenomes.ToLower() == nomeFamilia.ToLower())
                .Select(c => c.Fam_Id)
                .FirstOrDefaultAsync();

            return familiaId == 0 ? (int?)null : familiaId;
        }

        public async Task<bool> VerificarEmailCliente(string email)
        {
            return await _context.Cidadaos.AnyAsync(c => c.Cli_Email == email);
        }

        public async Task<Cliente> AdicionarCliente(ClienteDto clienteDto)
        {
            var cliente = new Cliente
            {
                Cli_Nome = clienteDto.Cli_Nome,
                Cli_CPF = clienteDto.Cli_CPF,
                Cli_DataNasc = clienteDto.Cli_DataNasc,
                Cli_Email = clienteDto.Cli_Email,
                Cli_Telefone = clienteDto.Cli_Telefone,
                Cli_IdFamilia = clienteDto.Cli_IdFamilia,
                Ativo_Inativo = clienteDto.Ativo_Inativo
            };

            _context.Cidadaos.Add(cliente);
            await _context.SaveChangesAsync();

            return cliente;
        }

        public async Task<Cliente> AtualizarCliente(ClienteEdicaoDto clienteDto, int id)
        {
            try
            {
                Cliente clientePorId = await _context.Cidadaos.FirstOrDefaultAsync(x => x.Cli_Id == id);

                if (clientePorId == null)
                {
                    throw new Exception($"Usuário para o Id: {id} não foi encontrado no banco de dados.");
                }

                clientePorId.Cli_Nome = clienteDto.Cli_Nome ?? clientePorId.Cli_Nome;
                clientePorId.Cli_DataNasc = clienteDto.Cli_DataNasc ?? clientePorId.Cli_DataNasc;
                clientePorId.Cli_Email = clienteDto.Cli_Email ?? clientePorId.Cli_Email;
                clientePorId.Cli_Telefone = clienteDto.Cli_Telefone ?? clientePorId.Cli_Telefone;
                clientePorId.Cli_IdFamilia = clienteDto.Cli_IdFamilia ?? clientePorId.Cli_IdFamilia;

                _context.Cidadaos.Update(clientePorId);
                await _context.SaveChangesAsync();

                // Retornar a lista de clientes após a atualização
                return clientePorId;
            }
            catch (Exception ex)
            {
                // Log do erro
                Console.WriteLine($"Erro ao atualizar cliente: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> AtivarCliente(int id)
        {
            var cliente = await _context.Cidadaos.FirstOrDefaultAsync(c => c.Cli_Id == id);

            if (cliente == null)
            {
                throw new Exception($"Cliente com o ID: {id} não foi encontrado.");
            }

            cliente.Ativo_Inativo = 1; // Define como ativo
            _context.Cidadaos.Update(cliente);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> InativarCliente(int id)
        {
            var cliente = await _context.Cidadaos.FirstOrDefaultAsync(c => c.Cli_Id == id);

            if (cliente == null)
            {
                throw new Exception($"Cliente com o ID: {id} não foi encontrado.");
            }

            cliente.Ativo_Inativo = 0; // Define como inativo
            _context.Cidadaos.Update(cliente);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
