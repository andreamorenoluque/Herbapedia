using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbapedia.Model
{
    /// <summary>
    /// Clase que describe los mensajes enviados en tre los distintos usuarios de la aplicación
    /// </summary>
    public class MessageModel
    {
        /// <summary>
        /// Identificador único de la clase Mensaje
        /// </summary>
        [Key]
        public int MessageId { get; set; }

        /// <summary>
        /// Usuario encargado de emitir el mensaje 
        /// </summary>
        public int ? MessageTransmitterUserId { get; set; }
        public UserModel? MessageTransmitter { get; set; }

        /// <summary>
        /// Usuario que recibe el mensaje que ha enviado el MessageTransmitter
        /// </summary>
        public int ? MessageReceiverUserId { get; set; }
        public UserModel? MessageReceiver { get; set; }

        /// <summary>
        /// Este atributo está formado por el contenido del mensaje en si
        /// </summary>
        public string? MessageContent { get; set; }

        /// <summary>
        /// Fecha de emisión del mensaje
        /// </summary>
        public DateTime MessageDate { get; set; }

        [NotMapped]
        public bool IsTransmitter { get; set; }
        [NotMapped]
        public bool IsReceiver { get; set; }
    }
}
