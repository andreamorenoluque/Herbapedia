using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbapedia.Model
{
    /// <summary>
    /// Clase que muestra las publicaciones que se realizan en el sistema, ya sean publicaciones en el foro, comentarios es estas mismas publicaciones, comentarios en las plantas
    /// </summary>
    public class PostModel
    {
        /// <summary>
        /// Identificador único de la clase Post
        /// </summary>
        [Key]
        public int PostId { get; set; }

        public int PostCreatorUserId { get; set; }
        /// <summary>
        /// Usuario creador de la publicación
        /// </summary>
        public UserModel? PostCreator { get; set; }

        /// <summary>
        /// Atriburo que muestra el contenido de la publicación
        /// </summary>
        public string? PostContent { get; set; }

        /// <summary>
        /// Fecha de creación de la publicación
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// Lista de comentarios asociados a las pueblicaciones
        /// </summary>
        public List<CommentModel>? Comments { get; set; }
       
    }
}