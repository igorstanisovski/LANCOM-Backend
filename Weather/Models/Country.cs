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
        [StringLength(50, MinimumLength = 1 , ErrorMessage = "{0} must be between {1} and {2}.")]
        [DataType(DataType.Text)]
        public string Name { get; set;  }
    }
}
