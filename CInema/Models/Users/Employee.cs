using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Models
{
    [Table("EMPLOYEE")]
    public class Employee
    {
        [Column("KOD_EMPLOYEE")]
        [Key]
        public long Kod { get; set; }
        [Column("SECOND_NAME")]
        public string SecondName { get; set; }
        [Column("FIRST_NAME")] 
        public string FirstName { get; set; }
        [Column("THIRD_NAME")] 
        public string ThirdName { get; set; }
        [Column("DATE_BIRTH")] 
        public DateTime BirthDay { get; set; }
    }
}
