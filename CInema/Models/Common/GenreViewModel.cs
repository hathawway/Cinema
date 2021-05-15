using System.ComponentModel.DataAnnotations;

namespace Cinema.Models.Common
{
    public class GenreViewModel
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
