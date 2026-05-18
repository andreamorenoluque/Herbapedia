using Herbapedia.API.Data;
using Herbapedia.Model;
using Microsoft.AspNetCore.Mvc;
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
    public class PostController : ControllerBase
    {
        private readonly ILogger<PostController> _logger;
        private readonly HerbapediaDBContext _context;

        public PostController(ILogger<PostController> logger, HerbapediaDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> SendPostAsync(PostModel post)
        {
            try
            {
                post.PostCreator= _context.Users.FirstOrDefault(u => u == post.PostCreator);
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
                return Ok(true);
            }
            catch 
            {
               
                return StatusCode(500, false);
            }
        }
        [HttpPut]
        public async Task<ActionResult<PostModel>> PostModificationAsync(PostModel post)
        {
            try
            {
                _context.Update(post);
                await _context.SaveChangesAsync();
                return Ok(post);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, error);
            }

        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeletePostsAsync(int? id = null, int? creador = null, DateTime? date = null)
        {
            try
            {
                if (id != null)
                {
                    List<PostModel> publicacionesId = await GetPostByIdAsync((int)id);
                    if (publicacionesId!= null)
                    {
                        foreach(PostModel post in publicacionesId)
                        {
                            _context.Posts.Remove(post);
                        }
                        
                    }
                }
                else if (creador != null)
                {
                    List<PostModel>? publicacionesCreador = (List<PostModel>?)await GetPostsByCreatorAsync((int)creador);
                    if (publicacionesCreador!= null)
                    {
                        foreach (PostModel post in publicacionesCreador)
                        {
                            _context.Posts.Remove(post);
                        }
                    }
                }
                else if (date != null)
                {
                    List <PostModel> postDate = await GetPostsByDateAsync((DateTime)date);
                    if (postDate != null)
                    {
                        foreach (var post in postDate)
                        {
                            _context.Posts.Remove(post);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar los posts: {ex.Message}");
                return StatusCode(500, false);
            }
        }
        [HttpGet("Filter")]
        public async Task<ActionResult<List<PostModel>>> Filter(string filter, string value = null)
        {
            try
            {
                filter = filter.ToUpper();
                List<PostModel>? posts = new List<PostModel>();
                switch (filter)
                {
                    case "ID":
                        posts = await GetPostByIdAsync(int.Parse(value));
                        break;
                    case "CREATOR":
                        posts = (List<PostModel>)await GetPostsByCreatorAsync(int.Parse(value));
                        break;
                    case "DATE":
                        posts = await GetPostsByDateAsync(DateTime.Parse(value));
                        break;
                    case "ALL":
                        posts = await _context.Posts.ToListAsync();
                        break;
                        
                }
                foreach (PostModel post in posts)
                {
                    post.PostCreator = _context.Users.Where(u => u.UserId == post.PostCreatorUserId).FirstOrDefault();
                }

                if (posts == null)
                {
                    return NoContent();
                }

                return posts;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #region Metodos Privados


        private async Task<IActionResult> GetPostsByCreatorAsync(int creatorId)
        {
            try
            {
                List<PostModel>? posts = await _context.Posts
                                                       .Where(p => p.PostCreator.UserId == creatorId)
                                                       .ToListAsync();

                if (posts == null || posts.Count == 0)
                {
                    return NoContent();
                }

               
                return Ok(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al encontrar posts por creador: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }


        private async Task<List<PostModel>> GetPostsByDateAsync(DateTime date)
        {
            try
            {
                List<PostModel> posts = await _context.Posts.Where(p => p.PostDate.Date == date.Date).ToListAsync();


                return posts;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al encontrar posts por fecha: {ex.Message}");
                return new List<PostModel>();
            }
        }

        
        private async Task<List<PostModel>> GetPostByIdAsync(int id)
        {
            try
            {
                List<PostModel> posts = await _context.Posts.Where(p => p.PostId == id).ToListAsync();

                return posts;
            }
            catch (Exception ex)
            {
                
                return new List<PostModel>();
            }
        }
        #endregion
    }
}

