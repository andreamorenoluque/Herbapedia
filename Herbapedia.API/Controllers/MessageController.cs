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
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        private readonly HerbapediaDBContext _context;

        public MessageController(ILogger<MessageController> logger, HerbapediaDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> SendMessageAsync( MessageModel message)
        {
            try
            {


                message.MessageTransmitter = await _context.Users.FirstAsync(u => u == message.MessageTransmitter);
                message.MessageDate = DateTime.Now.ToUniversalTime();
                message.MessageReceiver = await _context.Users.FirstAsync(u => u== message.MessageReceiver);
                
                _context.Messages.Add(message);
                await _context.SaveChangesAsync();
                return Ok(true);
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, false);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteMessageAsync(int? id = null, int? emisor = null, int? receptor = null, DateTime? date = null)
        {
            try
            {
                if (id != null)
                {
                    List<MessageModel> mensajes = await GetMessageByIdAsync((int)id);
                    if (mensajes != null)
                    {
                        foreach(MessageModel mensaje in mensajes)
                        {
                            _context.Messages.Remove(mensaje);
                        }
                        
                    }
                }
                else if (emisor != null)
                {
                    List<MessageModel> mensajesEmisor = await GetMessagesByTransmitterAsync((int)emisor);
                    if (mensajesEmisor != null)
                    {
                        foreach (MessageModel message in mensajesEmisor)
                        {
                            _context.Messages.Remove(message);
                        }
                    }
                }
                else if (receptor != null)
                {
                    List<MessageModel> mensajesReceptor = await GetMessagesByReceiverAsync((int)receptor);
                    if (mensajesReceptor != null)
                    {
                        foreach (MessageModel message in mensajesReceptor)
                        {
                            _context.Messages.Remove(message);
                        }
                    }
                }
                else if (date != null)
                {
                    List<MessageModel> messagesDate = await GetMessagesByDateAsync((DateTime)date);
                    if (messagesDate != null)
                    {
                        foreach (MessageModel message in messagesDate)
                        {
                            _context.Messages.Remove(message);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ha surgido un error al eliminar el mensaje: {ex.Message}");
                return StatusCode(500, false);
            }
        }

        [HttpGet("Filter")]
        public async Task<ActionResult<List<MessageModel>>> Filter(string filter, string value)
        {
            try
            {
                filter = filter.ToUpper();
                List<MessageModel> messages = new List<MessageModel>();
                switch (filter)
                {
                    case "ID":
                        messages = await GetMessageByIdAsync(int.Parse(value));
                        break;
                    case "TRANSMITTER":
                        messages = await GetMessagesByTransmitterAsync(int.Parse(value));
                        break;
                    case "RECEIVER":
                        messages = await GetMessagesByReceiverAsync(int.Parse(value));
                        break;
                    case "CHAT":
                        string [] values = value.Split(';');
                        int idA = int.Parse(values[0]);
                        int idB = int.Parse(values[1]);
                        messages = await GetMessageByChatAsync(idA, idB);
                        break;
                    case "DATE":
                        messages = await GetMessagesByDateAsync(DateTime.Parse(value));
                        break;
                }

                foreach(MessageModel message in messages)
                {
                    message.MessageTransmitter = _context.Users.Where(u => u.UserId == message.MessageTransmitterUserId).First();
                    message.MessageReceiver = _context.Users.Where(u => u.UserId == message.MessageReceiverUserId).First();
                }
                return messages;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }



        #region Métodos Privados


        private async Task<List<MessageModel>> GetMessageByChatAsync(int idA, int idB)
        {
            try
            {
                List<MessageModel> messages = await _context.Messages
                .Where(m =>
                    (m.MessageTransmitter.UserId == idA && m.MessageReceiver.UserId == idB) ||
                    (m.MessageTransmitter.UserId == idB && m.MessageReceiver.UserId == idA)
                )
                    .ToListAsync();
                return messages;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al encontrar mensajes por chat: {ex.Message}");
                return new List<MessageModel>();
            }
        }


        private async Task<List<MessageModel>> GetMessagesByTransmitterAsync(int transmitterId)
        {
            try
            {
                List<MessageModel> messages = await _context.Messages
                    .Where(m => m.MessageTransmitter.UserId == transmitterId)
                    .ToListAsync();
                return messages;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al encontrar mensajes por emisor: {ex.Message}");
                return new List<MessageModel>();
            }
        }

        private async Task<List<MessageModel>> GetMessagesByReceiverAsync(int receiverId)
        {
            try
            {
                List<MessageModel> messages = await _context.Messages
                    .Where(m => m.MessageReceiver.UserId == receiverId)
                    .ToListAsync();


                return messages;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al encontrar mensajes por receptor: {ex.Message}");
                return new List<MessageModel>();
            }
        }

        private async Task<List<MessageModel>> GetMessagesByDateAsync(DateTime date)
        {
            try
            {
                List<MessageModel> messages = await _context.Messages
                    .Where(m => m.MessageDate.Date == date.Date)
                    .ToListAsync();

                return messages;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al encontrar mensajes por fecha: {ex.Message}");
                return new List<MessageModel>();
            }
        }

        private async Task<List<MessageModel>> GetMessageByIdAsync(int id)
        {
            try
            {
                List<MessageModel> message = await _context.Messages
                    .Where(m => m.MessageId == id).ToListAsync();

                return message;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al encontrar mensaje por ID: {ex.Message}");
                return new List<MessageModel>();
            }
        }
        #endregion

    }
}

