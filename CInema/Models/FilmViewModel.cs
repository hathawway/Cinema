using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Models
{
    public class FilmViewModel
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public IdName Studio { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public IdName Country { get; set; }

        [Required]
        public int TimeDuration { get; set; }

        [Required]
        public IdName Genre { get; set; }

        [Required]
        public IdName Rating { get; set; }

        public FilmViewModel() 
        {
            Studio = new();
            Country = new();
            Genre = new();
            Rating = new();
            StartDate = DateTime.Now;
        }

        public string Durations()
        {
            return (TimeDuration / 60) + "ч. " + (TimeDuration % 60) + " м.";
        }
    }

    public class IdName
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
