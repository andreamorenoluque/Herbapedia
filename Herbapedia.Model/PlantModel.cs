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
    /// Clase que describe la información de una planta, incluyendo las asociaciones xon otras clases (User, PlantType, y Comment)
    /// </summary>
    public class PlantModel
    {
        /// <summary>
        /// Identificador único de la clase Planta
        /// </summary>
        [Key]
        public int PlantId { get; set; }

        /// <summary>
        /// Muestra el nombre de la planta correspondiente
        /// </summary>
        public string? PlantName { get; set; }

        /// <summary>
        /// Atributo que contiene la descripción de la planta
        /// </summary>
        public string? PlantDescription { get; set; }

        /// <summary>
        /// Atributo compuesto por consejos que el creador o editor de la planta aporta, para así poder ayudar a los usuarios que los necesiten
        /// </summary>
        public string? PlantTips { get; set; }

        /// <summary>
        /// Usuario que crea la planta en la aplicación
        /// </summary>
        public UserModel? PlantCreator { get; set; }


        /// <summary>
        /// Usuario que modifica la planta en la aplicación
        /// </summary>
        public UserModel? PlantEditor { get; set; }

        public int PlantTypeId { get; set; }
        /// <summary>
        /// Específica el tipo de planta que es, es decir a la categoria que pertenece dicha planta
        /// </summary>
        public PlantTypeModel? PlantType { get; set; }

        /// <summary>
        /// Lista de comentarios vinculados a cada planta
        /// </summary>
        public List<CommentModel>? Comments { get; set; }

        /// <summary>
        /// Atributo que corresponde a la fecha de creacción de una planta en el sistema
        /// </summary>
        public DateTime PlantCreationDate { get; set; }

        /// <summary>
        /// Contiene la fecha de edicción de una planta ya existente en la aplicación
        /// </summary>
        public DateTime? PlantModificationDate { get; set; }
        public string? Log { get; set; }

        public DateTime? DeletionDate { get; set; }
        


    }
}
