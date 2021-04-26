using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Models.Film
{
    [Table("FILMS")]
    public class Film
    {
        [Column("KOD_FILM")]
        [Key]
        public long Kod { get; set; }
        [Column("NAME_FILM")]
        public string Name { get; set; }
        [Column("KOD_FILMSTUDIO")]
        public long StudioKod{ get; set; }
        [Column("DATE_START")]
        public DateTime StartDate { get; set; }
        [Column("KOD_COUNTRY")]
        public long CountryKod{ get; set; }
        [Column("TIMEFILM")]
        public int TimeDuration{ get; set; }
        [Column("KOD_GENRE")]
        public long GenreKod{ get; set; }
        [Column("KOD_RATING")]
        public long RatingKod { get; set; }
    }
}
