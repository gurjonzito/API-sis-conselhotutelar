using API_sis_conselhotutelarv2.Data;
using API_sis_conselhotutelarv2.Repositórios;
using API_sis_conselhotutelarv2.Repositórios.Interfaces;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace API_sis_conselhotutelarv2
{
    public class Program
    {
        public static void Main(string[] args)
        {

            string connectionString = "Server=;Database=;User=;Password=;";
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Conexão bem-sucedida!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao conectar: {ex.Message}");
                }
            }

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(builder.Configuration.GetConnectionString("DataBase"),
                new MySqlServerVersion(new Version(8, 0, 21))));

            builder.Services.AddScoped<ICargoRepositorio, CargoRepositorio>();
            builder.Services.AddScoped<IColaboradorRepositorio, ColaboradorRepositorio>();
            builder.Services.AddScoped<IAtendimentoRepositorio, AtendimentoRepositorio>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
