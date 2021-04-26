using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CInema.Domain.Models.User
{
    public class Employe
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        [Key]
        public virtual Guid Guid { get; set; }
        /// <summary>
        /// Фамилия
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [Required]
        public string FirstName { get; set; }
        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get; set; }
        /// <summary>
        /// Дополнительная информация
        /// </summary>
        public string AditionalInfo { get; set; }

        public string FullName()
        {
            return FirstName + " " + LastName;
        }
    }
}
