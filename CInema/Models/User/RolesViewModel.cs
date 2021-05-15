using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Models.User
{
    public class RolesViewModel
    {
        public long Kod { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
