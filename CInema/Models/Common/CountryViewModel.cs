using System.ComponentModel.DataAnnotations;

namespace Cinema.Models.Common
{
    public class CountryViewModel
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
