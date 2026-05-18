using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbapedia.Model
{
    public class UserModel
    {
        /// <summary>
        /// Identificador único de la clase Usuario
        /// </summary>
        [Key]
        public int UserId { get; set; }

        /// <summary>
        /// Información referente al Nombre del usuario
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Información referente a los Apellidos del usuario
        /// </summary>
        public string? UserSurname { get; set; }

        /// <summary>
        /// Atributo que muestra la fecha de nacimeinto del usuario
        /// </summary>
        public DateTime? UserBirthdate { get; set; }

        /// <summary>
        /// Fecha de creacción del usuario
        /// </summary>
        public DateTime UserCreationDate { get; set; }

        /// <summary>
        /// Fecha de modificación del usuario
        /// </summary>
        public DateTime UserModificationDate { get; set; }


        /// <summary>
        /// Proporciona información de la contraseña que el usuario usa para entrar en el sistema
        /// </summary>
        public string? UserPassword { get; set; }

        /// <summary>
        /// Presenta el nombre de usuario utilizado para iniciar sesión en el sistema
        /// </summary>
        public string? UserLoginName { get; set; }

        /// <summary>
        /// Número de móvil del usuario
        /// </summary>
        public string? UserPhone { get; set; }

        /// <summary>
        /// Correo electrónico del usuario
        /// </summary>
        public string? UserEmail { get; set; }

        /// <summary>
        /// Atributo que representa el tipo de rol que tiene el usuario en el sistema
        /// </summary>
        public RoleModel? UserRole { get; set; }


        [NotMapped]
        public int RoleID { get; set; }

        
    }
}
