using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Domain.Models.Film
{
    [Table("FILMSEMP")]
    [Keyless]
    public class FilmSemp
    {
        [Column("KOD_FILM")]
        public long FilmKod { get; set; }

        [Column("KOD_EMPLOYEE")]
        public long EmployeeKod { get; set; }

        [Column("KOD_TYPEEMP")]
        public long TypeEmployeeKod { get; set; }
    }
}
