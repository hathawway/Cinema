using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Models
{
    public class FilmsEmpViewModel
    {
        public long Kod { get; set; }
        public IdName Film { get; set; }
        public IdName Employee { get; set; }
        public IdName EmployeeType { get; set; }

        public FilmsEmpViewModel()
        {
            Film = new();
            Employee = new();
            EmployeeType = new();
        }
    }
}
