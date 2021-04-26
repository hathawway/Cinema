using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Models
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
