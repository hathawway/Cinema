using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Domain.Models
{
    [Table("USERS")]
    public class User
    {
        [Column("LOGIN")]
        [Key]
        public string Login { get; set; }

        [Column("PASSWORD")]
        public string Password { get; set; }
        [Column("KOD_ROLEUSERS")]
        public string RoleUserKod { get; set; }
    }
}
