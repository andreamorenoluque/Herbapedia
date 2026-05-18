using Herbapedia.API.Data;
using Herbapedia.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using System;
using System.Numerics;
using System.Reflection;

namespace Herbapedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly HerbapediaDBContext _context;

        public RoleController(HerbapediaDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<RoleModel>> RoleCreationAsync([FromBody] RoleModel rol)
        {
            try
            {
                _context.Roles.Add(rol);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("Access")]
        public async Task<ActionResult<bool>> GrantAccess(string page, int roleID)
        {
            Util.Cadenas.Capitalizar(ref page);
            if (!page.ToUpper().Contains("PAGE"))
            {
                page += "Page";
            }
            RoleModel? r = await _context.Roles.FirstAsync(r => r.RoleId == roleID);
            PropertyInfo? columna = typeof(RoleModel).GetProperty(page);
            int? acceso = int.Parse(columna.GetValue(r).ToString());

            return (acceso == 1 ? true : false);
        }

        [HttpGet("All")]
        public async Task<ActionResult<List<RoleModel>>> RoleListAsync()
        {
            try
            {
                List<RoleModel> roles = await _context.Roles.ToListAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult<bool>> RoleModificationAsync(int id, string name, string description)
        {
            try
            {
                RoleModel? role = await GetRoleByIdAsync(id);

                if (role == null)
                {
                    return NotFound(false);
                }

                RoleModel roleToModify = role;

                roleToModify.RoleType = name;
                roleToModify.RoleDescription = description;

                await _context.SaveChangesAsync();
                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

      

        [HttpDelete]
        public async Task<ActionResult<bool>?> RoleDeletionAsync(int? id = null, string? name = null)
        {
            try
            {
                if (id != null)
                {
                    RoleModel? role= await GetRoleByIdAsync((int)id);
                    if (role != null)
                    {
                        _context.Roles.Remove(role);
                    }
                }
                else if (name != null)
                {
                    RoleModel? role = await GetRoleByNameAsync((string)name);
                 
                    if (role != null)
                    {
                        _context.Roles.Remove(role);
                    }
                }

                await _context.SaveChangesAsync();
                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("Filter")]
        public async Task<ActionResult<RoleModel>?> Filter(string filter, string value)
        {
            try
            {
                filter = filter.ToUpper();
                RoleModel? role = new RoleModel();
                switch (filter)
                {
                    case "ID":
                        role = await GetRoleByIdAsync(int.Parse(value));
                        break;
                    case "NAME":
                        role = await GetRoleByNameAsync(value);
                        break;
                   
                }

                if (role == null)
                {
                    return NoContent();
                }
                return Ok(role);

            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #region Metodos Privados
        private async Task<RoleModel?> GetRoleByIdAsync(int roleId)
        {
            try
            {
                RoleModel? role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == roleId);


                if (role == null)
                {
                    return null;
                }
                return role;

            }
            catch
            {
                return null;
            }
        }

        
        private async Task<RoleModel?> GetRoleByNameAsync(string roleName)
        {
            try
            {
                RoleModel? role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleType == roleName);

                return role;
               
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
    }
}

