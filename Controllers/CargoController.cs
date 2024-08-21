using API_sis_conselhotutelarv2.Enums;
using API_sis_conselhotutelarv2.Models;
using API_sis_conselhotutelarv2.Repositórios;
using API_sis_conselhotutelarv2.Repositórios.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_sis_conselhotutelarv2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CargoController : ControllerBase
    {
        private readonly ICargoRepositorio _cargoRepositorio;

        public CargoController(ICargoRepositorio cargoRepositorio)
        {
            _cargoRepositorio = cargoRepositorio;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Cargo>>> BuscarCargoPorId(int id)
        {
            List<Cargo> cargo = await _cargoRepositorio.BuscarCargoPorId(id);
            return Ok(cargo);
        }

        [HttpPost]
        public async Task<ActionResult<Cargo>> CadastrarCargo([FromBody] CargoDto cargoModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cargo = new Cargo
            {
                Car_Nome = cargoModel.Car_Nome
            };

            cargo = await _cargoRepositorio.AdicionarCargo(cargo);

            if (cargo != null)
            {
                return Ok(new ApiResponse<Cargo>
                {
                    Data = cargo,
                    Success = true,
                    Message = "Cargo cadastrado com sucesso"
                });
            }

            return StatusCode(500, new ApiResponse<bool>
            {
                Data = false,
                Success = false,
                Message = "Erro ao cadastrar cargo"
            });
        }
    }
}
