using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Domain.Models.Users
{
    [Table("WEB_USER_INFO")]
    [Keyless]
    public class UserInfo
    {
        [Column("KOD_USER")]
        public long KodUser { get; set; }
        [Column("FIRST_NAME")]
        public string FirstName { get; set; }
        [Column("SECOND_NAME")]
        public string SecondName { get; set; }
        [Column("LAST_NAME")]
        public string ThirdName { get; set; }
        [Column("BIRTHDAY")]
        public DateTime Birthday{ get; set; }
    }
}
