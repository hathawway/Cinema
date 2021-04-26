using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Models
{
    [Table("COUNTRY")]
    public class Country
    {
        [Column("KOD_COUNTRY")]
        [Key]
        public long Kod { get; set; }

        [Column("NAME_COUNTRY")]
        public string Name { get; set; }
    }
}
