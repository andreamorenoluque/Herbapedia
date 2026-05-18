using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbapedia.Model
{
    /// <summary>
    /// Clase que representa los comentarios qeu los usuarios realizan, estos comentarios pueden estar ligados a las publicaciones o a las plantas 
    /// Hereda atributos de la clase Post
    /// </summary>

    public class CommentModel
    {
        [Key]
        
        /// <summary>
        /// Identificar unico de la clase comentario
        /// </summary>
        public int CommentId { get; set; }
        /// <summary>
        /// Usuario creador del comentario
        /// </summary>
        public int? CommentCreatorUserId { get; set; }
        public UserModel? CommentCreator { get; set; }

        /// <summary>
        /// Atriburo que muestra el contenido del comentario
        /// </summary>
        public string? CommentContent { get; set; }
        /// <summary>
        /// Muestra la pueblicación a la que esta asociada el comentario
        /// Es un atributo opcional
        /// </summary>

        public PostModel? CommentPost { get; set; }

        /// <summary>
        /// Define la planta a la que esta ligada el comentario
        /// Es un atributo opcionals
        /// </summary>
        public PlantModel? CommentPlant { get; set; }
        /// <summary>
        /// Fecha de creación del comentario
        /// </summary>
        public DateTime CommentDate { get; set; }
    }
}
