using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Models
{
    public class FilmViewModel
    {
        public long Kod { get; set; }
        public string Name { get; set; }
        public string Studio { get; set; }
        public DateTime StartDate { get; set; }
        public string Country { get; set; }
        public int TimeDuration { get; set; }
        public string Genre{ get; set; }
        public string Rating { get; set; }
    }
}
