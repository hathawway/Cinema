using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Models
{
    public class BoxOffice
    {
        public string Film { get; set; }
        public long TotalSum { get; set; }
        public DateTime Date { get; set; }
    }
}
