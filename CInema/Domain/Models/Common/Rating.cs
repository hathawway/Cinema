using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Domain.Models.Common
{
    [Table("RATING")]
    public class Rating
    {
        [Column("KOD_RATING")]
        [Key]
        public long Kod { get; set; }

        [Column("NAME_RATING")]
        public string Name { get; set; }
    }
}
