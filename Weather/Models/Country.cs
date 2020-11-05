using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "{0} cannot be longer than 50 characters.")]
        public string Name { get; set;  }
    }
}
