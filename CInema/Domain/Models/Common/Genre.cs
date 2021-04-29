using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Domain.Models.Common
{
    [Table("GENRE")]
    public class Genre
    {
        [Column("KOD_GENRE")]
        [Key]
        public long Kod { get; set; }

        [Column("NAME_GENRE")]
        public string Name { get; set; }
    }
}
