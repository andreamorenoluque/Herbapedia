using Herbapedia.API.Data;
using Herbapedia.Model;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Herbapedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly HerbapediaDBContext _context;

        public UserController(ILogger<UserController> logger, HerbapediaDBContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> Get()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserModel>> CreateUser([FromBody]UserModel user)
        {
            try
            {
                user.UserRole = await _context.Roles.FirstAsync(u => u.RoleId == user.RoleID);
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserModel>> UserModification(int id, [FromBody] UserModel usuarioActualizado)
        {
            try
            {
                UserModel? userToModify = await GetUserById(id);

                if (userToModify != null)
                {
                
                    userToModify.UserName = usuarioActualizado.UserName;
                    userToModify.UserSurname = usuarioActualizado.UserSurname;
                    userToModify.UserPassword = usuarioActualizado.UserPassword;
                    userToModify.UserEmail = usuarioActualizado.UserEmail;
                    userToModify.UserLoginName = usuarioActualizado.UserLoginName;
                    userToModify.UserPhone = usuarioActualizado.UserPhone;
                    userToModify.UserModificationDate = DateTime.Now.ToUniversalTime();

                    _context.Users.Update(userToModify);
                    await _context.SaveChangesAsync();

                    return Ok(userToModify);
                }
                return NotFound(false);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("Login")]
        public async Task<ActionResult<UserModel>> LoginAsync(string loginName, string password)
        {
            try
            {
                UserModel? user = await _context.Users
                    .Include(u => u.UserRole)
                    .FirstOrDefaultAsync(u => u.UserLoginName == loginName && u.UserPassword == password);

                if (user == null)
                {
                    return Unauthorized("Credenciales no válidas");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("FilterByRole/{roleId}")]
        public async Task<ActionResult<IEnumerable<UserModel>>?> UserList(int roleId)
        {
            try
            {
                List<UserModel>? users = await _context.Users
                    .Include(u => u.UserRole)
                    .Where(u => u.UserRole.RoleId == roleId)
                    .ToListAsync();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> UserDeletion( int? id = null, string? surname = null, string? name = null, DateTime? creationDate = null, string? loginName = null)
        {
            try
            {
                if (id != null)
                {
                    UserModel? user = await GetUserById((int)id);
                    if (user != null)
                    {
                        _context.Users.Remove(user);
                    }
                }
                else if (surname != null)
                {
                    List<UserModel> users = await GetUsersBySurname(surname);
                    foreach (UserModel user in users)
                    {
                        _context.Users.Remove(user);
                    }
                }
                else if (name != null)
                {
                    List<UserModel> users = await GetUserByName(name);
                    if (users != null && users.Count > 0)
                    {
                        foreach (UserModel user in users)
                        {
                            _context.Users.Remove(user);
                        }
                    }
                }
                else if (loginName != null)
                {
                    List<UserModel> users = await GetUserByLoginName(loginName);
                    if (users != null && users.Count > 0)
                    {
                        foreach (UserModel user in users)
                        {
                            _context.Users.Remove(user);
                        }
                    }
                }
                else if (creationDate != null)
                {
                    List<UserModel> usersDate = await GetUsersByCreationDate((DateTime)creationDate);
                    foreach (UserModel user in usersDate)
                    {
                        _context.Users.Remove(user);
                    }
                }

                await _context.SaveChangesAsync();
                return Ok(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el usuario: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, false);
            }
        }

        [HttpGet("{id}")]
        private async Task<UserModel?> GetUserById(int id)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet("FilterBySurname")]
        private async Task<List<UserModel>> GetUsersBySurname(string surname)
        {
            try
            {
                return await _context.Users.Where(u => u.UserSurname == surname).ToListAsync();
            }
            catch (Exception)
            {
                return new List<UserModel>();
            }
        }


       
        [HttpGet("Filter")]
        public async Task<ActionResult<List<UserModel>>> Filter(string filter, string value)
        {
            List<UserModel> result = new List<UserModel>();

            try
            {
                switch (filter.ToUpper())
                {
                    case "NAME":
                        result = await GetUserByName(value);
                        break;
                    case "LOGIN_NAME":
                        result = await GetUserByLoginName(value);
                        break;
                    case "CREATION_DATE":
                        if (DateTime.TryParse(value, out DateTime creationDate))
                        {
                            result = await GetUsersByCreationDate(creationDate);
                        }
                        else
                        {
                            return BadRequest("Fecha de creación no válida.");
                        }
                        break;
                    case "ALL":
                        result = await _context.Users.ToListAsync();
                        break;

                    default:
                        return StatusCode(StatusCodes.Status405MethodNotAllowed, "Método de filtrado no soportado.");
                }

                

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #region Métodos Privados
        private async Task<List<UserModel>> GetUserByName(string name)
        {
            try
            {
                return await _context.Users.Where(u => u.UserName == name).ToListAsync();
            }
            catch (Exception)
            {
                return new List<UserModel>();
            }
        }

        private async Task<List<UserModel>> GetUserByLoginName(string loginName)
        {
            try
            {
                return await _context.Users.Where(u => u.UserLoginName == loginName).ToListAsync();
            }
            catch (Exception)
            {
                return new List<UserModel>();
            }
        }

        private async Task<List<UserModel>> GetUsersByCreationDate( DateTime creationDate)
        {
            try
            {
                return await _context.Users.Where(u => u.UserCreationDate.Date == creationDate.Date).ToListAsync();
            }
            catch (Exception)
            {
                return new List<UserModel>();
            }
        }
        #endregion

    }
}


