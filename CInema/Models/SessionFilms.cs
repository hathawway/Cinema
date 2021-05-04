using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Models
{
    public class SessionFilms
    {
        public string FilmName { get; set; }
        public DateTime FilmStart{ get; set; }
        public int ZalNumber { get; set; }
        public int SeatsAmount { get; set; }
        public double TicketPrice { get; set; }
        public int FreeSeatsAmount { get; set; }

    }
}
