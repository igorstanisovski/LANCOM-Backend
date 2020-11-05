using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Weather.Models
{
    public class CityDTO
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [ForeignKey("Country")]
        public int CountryId { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "{0} must be between {1} and {2} characters")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public int TemperatureInCelsius { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
