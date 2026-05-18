using Herbapedia.API.Data;
using Herbapedia.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Herbapedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ILogger<CommentController> _logger;
        private readonly HerbapediaDBContext _context;

        public CommentController(ILogger<CommentController> logger, HerbapediaDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> SendCommentAsync(CommentModel comment)
        {
            try
            {
                    comment.CommentCreator =  _context.Users.FirstOrDefault(u => u== comment.CommentCreator);
                    comment.CommentDate = DateTime.Now.ToUniversalTime();
                    if (comment.CommentPost != null)
                    {
                        comment.CommentPost = _context.Posts.FirstOrDefault(p => p.PostId == comment.CommentPost.PostId);
                    }
                    else if (comment.CommentPlant != null)
                    {
                        comment.CommentPlant = _context.Plants.FirstOrDefault(pl => pl.PlantId == comment.CommentPlant.PlantId);
                    }
                    else
                    {
                    return BadRequest();
                    }
                    _context.Comments.Add(comment);
                    await _context.SaveChangesAsync();
                    return Ok(true);
                
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al enviar el comentario: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteCommentAsync(int? id = null, int? creator = null, DateTime? date = null)
        {
            try
            {
                if (id != null)
                {
                    List<CommentModel> comentariosId = await GetCommentByIdAsync((int)id);
                    if (comentariosId != null)
                    {
                        foreach (CommentModel comment in comentariosId)
                        {

                            _context.Comments.Remove(comment);
                        }
                    }
                }
                else if (creator != null)
                {
                    List <CommentModel> comentariosCreador = await GetCommentsByCreatorAsync((int)creator);
                    if (comentariosCreador!= null)
                    {
                        foreach (CommentModel comment in comentariosCreador)
                        {
                            _context.Comments.Remove(comment);
                        }
                    }
                }
                else if (date != null)
                {
                    List<CommentModel>? commentsDate = await GetCommentsByDateAsync((DateTime)date);
                    if (commentsDate!= null)
                    {
                        foreach ( CommentModel comment in commentsDate)
                        {
                            _context.Comments.Remove(comment);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ha surgido un error al eliminar el comentario: {ex.Message}");
                return StatusCode(500, false);
            }
        }
        [HttpGet("Filter")]
        public async Task<ActionResult<List<CommentModel>>> Filter(string filter, string value)
        {
            try
            {
                filter = filter.ToUpper();
                List<CommentModel> comments = new List<CommentModel>();
                switch (filter)
                {
                    case "ID":
                        comments = await GetCommentByIdAsync(int.Parse(value));
                        break;
                    case "CREATOR":
                        comments = await GetCommentsByCreatorAsync(int.Parse(value));
                        break;
                    case "DATE":
                        comments = await GetCommentsByDateAsync(DateTime.Parse(value));
                        break;
                    case "POST":
                        comments = await GetCommentByPostIdAsync(int.Parse(value));
                        break;
                    case "PLANT":
                        comments = await GetCommentByPlantIdAsync(int.Parse(value));
                        break;

                }
                if(comments.Count <= 0)
                {
                    return NoContent();
                }
                foreach(CommentModel comment in comments)
                {
                    comment.CommentCreator = _context.Users.Where(u => u.UserId == comment.CommentCreatorUserId).First();
                }
                return comments;
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        #region Metodos Privados

        private async Task<List<CommentModel>> GetCommentsByCreatorAsync(int creatorId)
        {
            try
            {
                List<CommentModel> comments = await _context.Comments
                    .Where(c => c.CommentCreator.UserId == creatorId)
                    .ToListAsync();

     
                return comments;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al encontrar comentarios por creador: {ex.Message}");
                return new List<CommentModel>();
            }
        }

        
        private async Task<List<CommentModel>> GetCommentsByDateAsync(DateTime date)
        {
            try
            {
                List<CommentModel> comments = await _context.Comments
                    .Where(c => c.CommentDate.Date == date.Date)
                    .ToListAsync();


                return comments;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al encontrar comentarios por fecha: {ex.Message}");
                return new List<CommentModel>();
            }
        }

        
        private async Task<List<CommentModel>> GetCommentByIdAsync(int id)
        {
            try
            {
                List<CommentModel> comments = await _context.Comments
                    .Where(c => c.CommentId == id).ToListAsync();

                return comments;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al encontrar comentario por ID: {ex.Message}");
                return new List<CommentModel>();
            }
        }

        private async Task<List<CommentModel>> GetCommentByPostIdAsync(int id)
        {
            try
            {
                List<CommentModel> comments = await _context.Comments
                    .Where(c => c.CommentPost.PostId == id).ToListAsync();

                return comments;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al encontrar comentario por ID: {ex.Message}");
                return new List<CommentModel>();
            }
        }
        private async Task<List<CommentModel>> GetCommentByPlantIdAsync(int id)
        {
            try
            {
                List<CommentModel> comments = await _context.Comments
                    .Where(c => c.CommentPlant.PlantId == id).ToListAsync();

                return comments;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al encontrar comentario por ID: {ex.Message}");
                return new List<CommentModel>();
            }
        }
        #endregion
    }
}
