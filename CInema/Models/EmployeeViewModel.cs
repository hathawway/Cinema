using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Models
{
    /*
     * Съёмочная группа
     */
    public class EmployeeViewModel
    {
        public long Kod { get; set; }
        /*
         * Имя 
         */
        [Required]
        public string FirstName { get; set; }
        /*
         * Фамилия
         */

        [Required]
        public string SecondName { get; set; }
        /*
         * Отчество
         */
        public string ThirdName { get; set; }
        /*
         * Дата рождения
         */

        [Required]
        public DateTime BirthDay{ get; set; }

        public int Age()
        {
            return DateTime.Now.Year - BirthDay.Year;
        }
        public string Initials()
        {
            return FirstName + " " + SecondName.Substring(0, 1) + ". " + ThirdName.Substring(0,1)+"." ;
        }
    }
}
