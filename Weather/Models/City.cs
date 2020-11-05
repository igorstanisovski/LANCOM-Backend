using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [ForeignKey("Country")]
        public int CountryId { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(100 , MinimumLength = 1, ErrorMessage = "{0} must be between {1} and {2} characters")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        //[Required(ErrorMessage = "{0} is required")]
        public int TemperatureInCelsius { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Timestamp { get; set; }
    }
}
