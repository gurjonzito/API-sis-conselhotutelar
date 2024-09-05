using API_sis_conselhotutelarv2.Repositórios;
using API_sis_conselhotutelarv2.Repositórios.Interfaces;
using API_sis_conselhotutelarv2.Data;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.DataProtection;

namespace API_sis_conselhotutelarv2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            builder.Services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(@"./keys/"))
                .SetApplicationName("SisConselho");

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configuração do EmpresaDbContext
            builder.Services.AddDbContext<EmpresaDbContext>(options =>
                options.UseMySql(builder.Configuration.GetConnectionString("EmpresaDataBase"),
                new MySqlServerVersion(new Version(8, 0, 21))));

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<ICargoRepositorio, CargoRepositorio>();
            builder.Services.AddScoped<IColaboradorRepositorio, ColaboradorRepositorio>();
            builder.Services.AddScoped<IAtendimentoRepositorio, AtendimentoRepositorio>();
            builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
            builder.Services.AddScoped<IFamiliaRepositorio, FamiliaRepositorio>();
            builder.Services.AddScoped<IEmpresaRepositorio, EmpresaRepositorio>();
            builder.Services.AddScoped<IApplicationDbContextFactory, ApplicationDbContextFactory>();
            builder.Services.AddScoped<IConnectionStringProvider>(provider =>
            {
                var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
                var empresaDbContext = provider.GetRequiredService<EmpresaDbContext>();
                var dataProtectionProvider = provider.GetRequiredService<IDataProtectionProvider>();

                return new ConnectionStringProvider(httpContextAccessor, empresaDbContext, dataProtectionProvider);
            });

            // Registrar TokenRepositorio
            builder.Services.AddScoped<TokenRepositorio>();

            // Configurar JWT Authentication
            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
