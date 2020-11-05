using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Weather.Models;

namespace Weather.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly WeatherContext _context;

        public CitiesController(WeatherContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Gets all cities.
        /// </summary>
        /// <returns>All available cities stored in database.</returns>
        // GET: Cities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityDTO>>> GetCities()
        {
            return await _context.Cities.Select(x => CityToDTO(x)).ToListAsync();
        }
        /// <summary>
        /// Gets a specific city.
        /// </summary>
        /// /// <remarks>
        /// /// Sample request:
        ///
        ///     GET /Cities/1
        ///     
        /// </remarks>
        /// <returns>A specific city.</returns>
        // GET: Cities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CityDTO>> GetCity(int id)
        {
            var city = await _context.Cities.FindAsync(id);

            if (city == null)
            {
                return NotFound();
            }

            return CityToDTO(city);
        }
        /// <summary>
        /// Edits a specific city.
        /// </summary>
        /// /// <remarks>
        /// /// Sample request:
        ///
        ///     PUT /Cities/1
        ///     {
        ///         "name":"Ljubljana",
        ///         "countryId": 1,
        ///         "temperatureInCelsius": 1,
        ///         "date": "28-01-2020"
        ///     }
        ///     
        /// </remarks>
        /// <returns>Edited specific city.</returns>
        // PUT: Cities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCity(int id, CityDTO cityDTO)
        {
            if (id != cityDTO.Id)
            {
                return BadRequest();
            }

            _context.Entry(cityDTO).State = EntityState.Modified;
            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            city.Name = cityDTO.Name;
            city.TemperatureInCelsius = cityDTO.TemperatureInCelsius;
            city.Date = cityDTO.Date;
            city.CountryId = cityDTO.CountryId;
            city.Timestamp = DateTime.Now;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        /// <summary>
        /// Creates a city.
        /// </summary>
        /// /// <remarks>
        /// /// Sample request:
        ///
        ///     POST /Cities
        ///     {
        ///         "name": "Ljubljana",
        ///         "countryId": 1
        ///     }
        ///     
        /// </remarks>
        /// <returns>Created city.</returns>
        // POST: Cities
        [HttpPost]
        public async Task<ActionResult<CityDTO>> CreateCity(CityDTO cityDTO)
        {
            var checkIfCityExists = CityExistsByName(cityDTO.Name);
            if (!checkIfCityExists)
            {
                var city = new City
                {
                    Name = cityDTO.Name,
                    TemperatureInCelsius = cityDTO.TemperatureInCelsius,
                    CountryId = cityDTO.CountryId,
                    Date = cityDTO.Date,
                    Timestamp = DateTime.Now
                };
                _context.Cities.Add(city);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCity), new { id = city.Id }, CityToDTO(city));
            }
            return NoContent();
        }
        /// <summary>
        /// Deletes a specific city.
        /// </summary>
        /// /// <remarks>
        /// /// Sample request:
        ///
        ///     DELETE /Cities/1
        ///     
        /// </remarks>
        /// <returns>Deleted city.</returns>
        // DELETE: Cities/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<City>> DeleteCity(int id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();

            return city;
        }

        private bool CityExists(int id)
        {
            return _context.Cities.Any(e => e.Id == id);
        }
        private bool CityExistsByName(string cityName)
        {
            return _context.Cities.Any(e => e.Name == cityName);
        }

        private static CityDTO CityToDTO(City city)
        {
            return new CityDTO
            {
                Id = city.Id,
                Name = city.Name,
                CountryId = city.CountryId,
                TemperatureInCelsius = city.TemperatureInCelsius,
                Date = city.Date
            };
        }
    }
}
