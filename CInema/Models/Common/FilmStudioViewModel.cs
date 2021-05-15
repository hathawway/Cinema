using System.ComponentModel.DataAnnotations;

namespace Cinema.Models.Common
{
    public class FilmStudioViewModel
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
