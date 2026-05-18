using Herbapedia.API.Data;
using Herbapedia.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Herbapedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantLogController : ControllerBase
    {
        private readonly ILogger<PlantLogController> _logger;
        private readonly HerbapediaDBContext _context;

        public PlantLogController(ILogger<PlantLogController> logger, HerbapediaDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Método para consultar los registros guardados de plantas borradas
        [HttpGet]
        public async Task<ActionResult<List<PlantLogModel>>> GetDeletedPlantLogsAsync()
        {
            try
            {
                List <PlantLogModel> deletedPlantLogs = await _context.PlantLogs
                    .Where(pl => pl.DeletionDate != null)
                    .ToListAsync();

                if (deletedPlantLogs == null || deletedPlantLogs.Count == 0)
                {
                    return NoContent();
                }

                return Ok(deletedPlantLogs);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener los registros de plantas borradas: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // Método para crear un registro en la base de datos
        [HttpPost]
        public async Task<ActionResult> CreatePlantLogAsync(PlantLogModel plantLog)
        {
            try
            {
                _context.PlantLogs.Add(plantLog);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el registro de planta: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // Método para borrar un registro en la base de datos
        [HttpDelete]
        public async Task<ActionResult> DeletePlantLogAsync(int id)
        {
            try
            {
                PlantLogModel? plantLog = await _context.PlantLogs.FindAsync(id);
                if(plantLog != null)
                {

                    _context.PlantLogs.Remove(plantLog);
                    await _context.SaveChangesAsync();
                }
               
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al borrar el registro de planta: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

       
    }
}

