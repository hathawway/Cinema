using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Models
{
    public class SessionFilmsViewModel
    {
        public long Kod { get; set; }
        public IdName Film { get; set; }
        public DateTime FilmStart{ get; set; }
        public int ZalNumber { get; set; }
        public int SeatsAmount { get; set; }
        public double TicketPrice { get; set; }
        public int FreeSeatsAmount { get; set; }

        public SessionFilmsViewModel()
        {
            Film = new();
        }

        public string FilmStartTime()
        {
            return FilmStart.ToString("yyyy-MM-dd") + "T" + FilmStart.ToString("hh:mm");
        }

    }
}
