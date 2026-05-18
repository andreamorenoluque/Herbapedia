using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbapedia.Model
{
    /// <summary>
    /// Clase utilixada como un registro histórico de las plantas del sistema 
    /// Hereda todos los atributos  de la clase Plant
    /// </summary>
    public class PlantLogModel : PlantModel
    {
        /// <summary>
        /// Identifidor único del historial de plantas
        /// </summary>
       
        public DateTime FechaLog { get; set; }

       
       
    }
}
