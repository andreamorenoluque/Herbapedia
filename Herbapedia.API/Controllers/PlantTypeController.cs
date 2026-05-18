using Herbapedia.API.Data;
using Herbapedia.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Herbapedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantTypeController : ControllerBase
    {
        private readonly ILogger<PlantTypeController> _logger;
        private readonly HerbapediaDBContext _context;

        public PlantTypeController(ILogger<PlantTypeController> logger, HerbapediaDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<PlantTypeModel>?> PlantTypeCreationAsync( string name, string description)
        {
            try
            {
                PlantTypeModel newPlantType = new PlantTypeModel
                {
                    PlantTypeName = name,
                    PlantTypeDescription = description
                };
                _context.PlantTypes.Add(newPlantType);
                await _context.SaveChangesAsync();
                return newPlantType;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Se ha encontrado un error al crear el nuevo tipo de plantas: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPut]
        public async Task<ActionResult<bool>> PlantTypeModificationAsync([FromHeader] int id, string name, string description)
        {
            try
            {
                PlantTypeModel? plantType = await GetPlantTypeByIdAsync(id);
                if (plantType != null)
                {
                    plantType.PlantTypeName = name;
                    plantType.PlantTypeDescription = description;
                    await _context.SaveChangesAsync();
                    return Ok(true);
                }
                return NotFound(false);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Se ha encontrado un error al modificar el tipo de plantas: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> PlantTypeDeletionAsync(int? id = null, string? name = null, string? description = null)
        {
            try
            {
                if (id != null)
                {
                    PlantTypeModel? plantType = await GetPlantTypeByIdAsync((int)id);
                    if (plantType!= null)
                    {
                        _context.PlantTypes.Remove(plantType);
                    }
                }
                else if (name != null)
                {
                    PlantTypeModel? plantType = await GetPlantTypeByNameAsync(name);
                    if (plantType != null)
                    {
                        _context.PlantTypes.Remove(plantType);
                    }
                }
                else if (description != null)
                {
                    PlantTypeModel? plantType = await GetPlantTypeByDescriptionAsync(description);
                    if (plantType != null)
                    {
                        _context.PlantTypes.Remove(plantType);
                    }
                }

                await _context.SaveChangesAsync();
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Se ha encontrado un error al eliminar el tipo de plantas: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<PlantTypeModel>>> GetAllPlantTypeAsync()
        {
            try
            {
                List<PlantTypeModel> plantTypes = await _context.PlantTypes.ToListAsync();
                
                return Ok(plantTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al proporcionar la lista completa de los tipos de plantas existentes: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }
        [HttpGet("Filter")]
        public async Task<ActionResult<PlantTypeModel>?> Filter(string filter, string value)
        {
            try
            {
                filter = filter.ToUpper();
                PlantTypeModel? plantType = new PlantTypeModel();
                switch (filter)
                {
                    case "ID":
                        plantType = await GetPlantTypeByIdAsync(int.Parse(value));
                        break;
                    case "NAME":
                        plantType = await GetPlantTypeByNameAsync(value);
                        break;
                    case "RECEIVER":
                        plantType = await GetPlantTypeByDescriptionAsync(value);
                            break;
                    
                }
                if (plantType == null)
                {
                    return NoContent();
                }
                return plantType;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        #region Metodos Privados

        private async Task<PlantTypeModel?> GetPlantTypeByIdAsync(int plantTypeId)
        {
            try
            {
                PlantTypeModel? plantType = await _context.PlantTypes.FirstOrDefaultAsync(pt => pt.PlantTypeId == plantTypeId);
                
                return plantType;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener el tipo de planta por id: {ex.Message}");
                return null;
            }
        }

        
        private async Task<PlantTypeModel?> GetPlantTypeByNameAsync(string name)
        {
            try
            {
                PlantTypeModel? plantType = await _context.PlantTypes.FirstOrDefaultAsync(pt => pt.PlantTypeName == name);
              
                return plantType;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener el tipo de planta por nombre: {ex.Message}");
                return null;
            }
        }
        private async Task<PlantTypeModel?> GetPlantTypeByDescriptionAsync(string description)
        {
            try
            {
                PlantTypeModel? plantType = await _context.PlantTypes.FirstOrDefaultAsync(pt => pt.PlantTypeDescription == description);
               
                return plantType;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener el tipo de planta por descripción: {ex.Message}");
                return null;
            }
        }
        #endregion
    }
}

