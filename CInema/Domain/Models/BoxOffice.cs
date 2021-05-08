using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Domain.Models
{
    [Table("BOXOFFICE")]
    [Keyless]
    public class BoxOffice
    {
        [Column("KOD_FILM")]
        public long KodFilm{ get; set; }
        [Column("TOTAL_SUM")]
        public long TotalSum { get; set; }
        [Column("DATE_BOXOFFICE")]
        public DateTime DateBoxOffice { get; set; }
    }
}
