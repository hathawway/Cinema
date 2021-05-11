using System;
using System.Collections.Generic;
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
        public string FirstName { get; set; }
        /*
         * Фамилия
         */
        public string SecondName { get; set; }
        /*
         * Отчество
         */
        public string ThirdName { get; set; }
        /*
         * Дата рождения
         */
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
