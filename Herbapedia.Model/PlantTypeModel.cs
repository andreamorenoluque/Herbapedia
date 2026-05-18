using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbapedia.Model
{
    /// <summary>
    /// Esta clase muestra el tipo de una planta, contiene información sobre la categoria de las plantas
    /// </summary>
    public class PlantTypeModel
    {
        /// <summary>
        /// Identificador único para la clase PlantType
        /// </summary>
        [Key]
        public int PlantTypeId { get; set; }

        /// <summary>
        /// Proporciona el nombre de las distintas categorias de las plantas
        /// </summary>
        public string? PlantTypeName { get; set; }

        /// <summary>
        /// Este atributo es el encargado de proporcionar la descripción del tipo de planta
        /// </summary>
        public string? PlantTypeDescription { get; set; }

        public static implicit operator int(PlantTypeModel v)
        {
            throw new NotImplementedException();
        }
    }
}
