using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Models
{
    [Table("FILMSTUDIO")]
    public class FilmStudio
    {
        [Column("KOD_FILMSTUDIO")]
        [Key]
        public long Kod { get; set; }

        [Column("NAME_FILMSTUDIO")]
        public string Name { get; set; }
    }
}
