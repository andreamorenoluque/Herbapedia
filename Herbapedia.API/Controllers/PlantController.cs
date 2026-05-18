using Herbapedia.API.Data;
using Herbapedia.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Herbapedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantController : ControllerBase
    {
        private readonly ILogger<PlantController> _logger;
        private readonly HerbapediaDBContext _context;

        public PlantController(ILogger<PlantController> logger, HerbapediaDBContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<PlantModel>> PlantCreationAsync(PlantModel newPlant)
        {
            try
            {
                newPlant.PlantCreator = _context.Users.FirstOrDefault(u => u == newPlant.PlantCreator);
                newPlant.PlantEditor = _context.Users.FirstOrDefault(u => u == newPlant.PlantEditor);
                _context.Add(newPlant);
                await _context.SaveChangesAsync();
                return newPlant;
            }
            catch(Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, error);
            }
        }

        [HttpPut("Modify")]
        public async Task<ActionResult<PlantModel>> PlantModificationAsync(PlantModel plant)
        {
            try
            {
                    _context.Update(plant);
                    await _context.SaveChangesAsync();
                    return Ok(plant);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, error);
            }
            
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<bool>> PlantDeletionAsync(int? id = null, int? idCreator = null, string? name = null, DateTime? creationDate = null)
        {
            try
            {
                List<PlantModel> plantsToDelete = new List<PlantModel>();

                if (id != null)
                {
                    List<PlantModel> plants = await GetPlantByIdAsync((int)id);
                    if (plants != null)
                    {

                        plantsToDelete.AddRange(plants);
                    }
                }
                else if (idCreator != null)
                {
                    List<PlantModel>? plants = await GetPlantsByCreatorAsync((int)idCreator);
                    if (plants != null)
                    {
                        plantsToDelete.AddRange(plants);
                    }
                }
                else if (name != null)
                {
                    List<PlantModel>? plants = await GetPlantByNameAsync(name);
                    if (plants != null)
                    {
                        plantsToDelete.AddRange(plants);
                    }
                }
                else if (creationDate != null)
                {
                    List<PlantModel>? plants = await GetPlantsByDateCreationAsync((DateTime)creationDate);
                    if (plants != null)
                    {
                        plantsToDelete.AddRange(plants);
                    }
                }

                foreach (var plant in plantsToDelete)
                {
                    PlantLogModel plantLog = new PlantLogModel
                    {
                        PlantName = plant.PlantName,
                        PlantDescription = plant.PlantDescription,
                        PlantTips = plant.PlantTips,
                        PlantCreator = plant.PlantCreator,
                        PlantEditor = plant.PlantEditor,
                        PlantTypeId = plant.PlantTypeId,
                        PlantCreationDate = plant.PlantCreationDate,
                        PlantModificationDate = plant.PlantModificationDate,
                        DeletionDate = DateTime.Now.ToUniversalTime()
                    };
                    _context.PlantLogs.Add(plantLog);
                    _context.Plants.Remove(plant);
                }

                await _context.SaveChangesAsync();
                return Ok(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar la planta: {ex.Message}");
                return StatusCode(500, false);
            }
        }

        [HttpGet("Filter")]
        public async Task<ActionResult<List<PlantModel>>> Filter(string filter, string value = null)
        {
            try
            {
                filter = filter.ToUpper();
                List<PlantModel>? plants = new List<PlantModel>();
                switch (filter)
                {
                    case "ID":
                        plants = await GetPlantByIdAsync(int.Parse(value));
                        break;
                    case "NAME":
                        plants = await GetPlantByNameAsync(value);
                        break;
                    case "TYPE":
                        plants = await GetPlantsByCreatorAsync(int.Parse(value));
                        break;
                    case "DATE":
                        plants = await GetPlantsByDateCreationAsync(DateTime.Parse(value));
                        break;
                    case "PATTERN":
                        plants = await GetByPatternAsync(value);
                        break;
                    case "ALL":
                        plants = await _context.Plants.ToListAsync();
                        foreach(PlantModel plant in plants)
                        {
                            plant.PlantType = _context.PlantTypes.Where(pt => pt.PlantTypeId == plant.PlantTypeId).First();
                        }
                        break;
                }
                if(plants == null)
                {
                    return NoContent();
                }
                return plants;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #region Metodos Privados

        private async Task<List<PlantModel>> GetPlantsByTypeAsync(int plantTypeId)
        {
            List<PlantModel> plants = await _context.Plants.Where(p => p.PlantTypeId == plantTypeId).ToListAsync();
           
            return plants;
        }

        
        private async Task<List<PlantModel>?> GetPlantsByCreatorAsync(int creatorId)
        {
            List<PlantModel>? plants = await _context.Plants.Where(p => p.PlantCreator.UserId == creatorId).ToListAsync();
           

            return plants;
        }

       
        private async Task<List<PlantModel>> GetPlantsByDateCreationAsync(DateTime dateCreation)
        {
            List<PlantModel> plants = await _context.Plants.Where(p => p.PlantCreationDate.Date == dateCreation.Date).ToListAsync();
           
            return plants;
        }

        
        private async Task<List<PlantModel>> GetPlantByIdAsync(int id)
        {
            List<PlantModel> plants = await _context.Plants.Where(p => p.PlantId == id).ToListAsync();
            foreach (PlantModel plant in plants)
            {
                plant.PlantType = _context.PlantTypes.Where(pt => pt.PlantTypeId == plant.PlantTypeId).First();
            }
            return plants;
        }

        
        private async Task<List<PlantModel>?> GetPlantByNameAsync(string name)
        {
            try
            {
                List<PlantModel> plants = await _context.Plants.Where(p => p.PlantName == name).ToListAsync();

                return plants;
            }
            catch
            {
                return null;
            }
        }

        
        private async Task<List<PlantModel>> GetByPatternAsync(string pattern)
        {
            List<PlantModel>? plants = await _context.Plants.Where(p => p.PlantName.Contains(pattern.ToUpper().Trim())).ToListAsync();
            
            return plants;
        }
        #endregion
    }
}