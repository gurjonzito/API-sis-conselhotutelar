using API_sis_conselhotutelarv2.Data;
using API_sis_conselhotutelarv2.Repositórios.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;

public class ConnectionStringProvider : IConnectionStringProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly EmpresaDbContext _principalDbContext;
    private readonly IDataProtector _protector;

    public ConnectionStringProvider(
        IHttpContextAccessor httpContextAccessor,
        EmpresaDbContext principalDbContext,
        IDataProtectionProvider dataProtectionProvider)
    {
        _httpContextAccessor = httpContextAccessor;
        _principalDbContext = principalDbContext;
        _protector = dataProtectionProvider.CreateProtector("ConnectionStringProtector");
    }

    public string GetConnectionString()
    {
        // Obter a chave de validade do cabeçalho da requisição
        if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("ChaveValidade", out var chaveValidade))
        {
            var chaveValidadeEntity = _principalDbContext.ChavesValidade
                .Include(c => c.Empresa)
                .FirstOrDefault(c => c.Cha_Chave == chaveValidade && c.Cha_Validade >= DateTime.UtcNow);

            if (chaveValidadeEntity != null && !string.IsNullOrEmpty(chaveValidadeEntity.Empresa.Emp_Connection))
            {
                // Descriptografar a string de conexão antes de retorná-la
                return _protector.Unprotect(chaveValidadeEntity.Empresa.Emp_Connection);
            }
            else
            {
                throw new Exception("Chave de validade inválida, expirada ou empresa não encontrada.");
            }
        }

        throw new Exception("Cabeçalho 'ChaveValidade' não encontrado na requisição.");
    }
}
