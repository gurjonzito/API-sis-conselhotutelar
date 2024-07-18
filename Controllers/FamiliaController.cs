using API_sis_conselhotutelarv2.Models;
using API_sis_conselhotutelarv2.Repositórios;
using API_sis_conselhotutelarv2.Repositórios.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_sis_conselhotutelarv2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamiliaController : ControllerBase
    {
        private readonly IFamiliaRepositorio _familiaRepositorio;

        public FamiliaController(IFamiliaRepositorio familiaRepositorio)
        {
            _familiaRepositorio = familiaRepositorio;
        }

        [HttpGet]
        public async Task<ActionResult<List<Familia>>> BuscarTodasFamilias()
        {
            List<Familia> familias = await _familiaRepositorio.BuscarTodasFamilias();
            return Ok(familias);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Familia>> BuscarFamiliaPorId(int id)
        {
            Familia familia = await _familiaRepositorio.BuscarFamiliaPorId(id);
            return Ok(familia);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Familia>> AtualizarFamilia(int id, [FromBody] Familia familiaModel)
        {
            try
            {
                Familia familia = await _familiaRepositorio.AtualizarFamilia(familiaModel, id);
                return Ok(familia);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Familia>> CadastrarFamilia([FromBody] Familia familiaModel)
        {
            Familia familia = await _familiaRepositorio.AdicionarFamilia(familiaModel);
            return Ok(familia);
        }
    }
}
