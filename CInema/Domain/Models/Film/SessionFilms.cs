using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Domain.Models.Film
{
    [Table("SESSIONFILMS")]
    public class SessionFilms
    {
        [Column("KOD_SESSION")]
        [Key]
        public long Kod { get; set; }
        [Column("KOD_FILM")]
        public long FilmKod { get; set; }
        [Column("DATE_FILM_START")]
        public DateTime FilmStartDate { get; set; }
        [Column("HALL")]
        public int Hall { get; set; }
        [Column("COUNT_PLACES")]
        public double Places { get; set; }
        [Column("PRICE_TICKET")]
        public double TicketPrice { get; set; }
        [Column("FREE_PLACES")]
        public double FreePlaces { get; set; }
    }
}
