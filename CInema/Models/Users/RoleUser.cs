
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Models.Users
{
    [Table("ROLEUSERS")]
    public class RoleUser
    {
        [Column("KOD_ROLEUSERS")]
        [Key]
        public long Kod { get; set; }

        [Column("NAME_ROLEUSERS")]
        public string Name { get; set; }
    }
}
