using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbapedia.Model
{
    public class RoleModel
    {
        /// <summary>
        /// Identificador único de la clase Role
        /// </summary>
        [Key]
        public int RoleId { get; set; }

        /// <summary>
        /// Muestra los tipos de roles que pueden tener los usuarios, definiendo así los permisos o niveles de accesos con respecto la aplicación (por ejemplo: "Usuario invitado", "Administrador" ...)
        /// </summary>
        public string? RoleType { get; set; }

        /// <summary>
        /// Porporciona una descripción detallada del tipo de rol
        /// </summary>
        public string? RoleDescription { get; set; }


        public int HomePage {  get; set; }
        public int PlantPage { get; set; }
        public int LoginPage { get; set; }
        public int UnregisterPage { get; set; }
        public int UserPage { get; set; }
        public int AdminPage { get; set; }
        public int PostPage { get; set; }
        public int ChatPage { get; set; }
        public int PlantLogPage {  get; set; }
        public int PlantEditionPage { get; set; }

    }
}
