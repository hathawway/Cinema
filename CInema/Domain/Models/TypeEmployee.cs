using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Domain.Models
{
    [Table("TYPEEMP")]
    public class TypeEmployee
    {
        [Column("KOD_TYPEEMP")]
        [Key]
        public long Kod { get; set; }

        [Column("NAME_EMP")]
        public string Name { get; set; }
    }
}
