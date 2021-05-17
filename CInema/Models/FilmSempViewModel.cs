using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Models
{
    public class FilmsEmpViewModel
    {
        public long Kod { get; set; }

        [Required]
        public IdName Film { get; set; }

        [Required]
        public IdName Employee { get; set; }

        [Required]
        public IdName EmployeeType { get; set; }

        public FilmsEmpViewModel()
        {
            Film = new();
            Employee = new();
            EmployeeType = new();
        }
    }
}
