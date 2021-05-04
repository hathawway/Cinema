using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Models
{
    public class BoxOfficeViewModel
    {
        public string Film { get; set; }
        public long TotalSum { get; set; }
        public DateTime Date { get; set; }
    }
}
