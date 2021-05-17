using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Models
{
    public class SessionFilmsViewModel
    {
        public long Kod { get; set; }

        [Required]
        public IdName Film { get; set; }

        [Required]
        public DateTime FilmStart{ get; set; }

        [Required]
        public int ZalNumber { get; set; }

        [Required]
        public int SeatsAmount { get; set; }

        [Required]
        public double TicketPrice { get; set; }

        [Required]
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
