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
    public class CountriesController : ControllerBase
    {
        private readonly WeatherContext _context;

        public CountriesController(WeatherContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Gets all countries.
        /// </summary>
        /// <returns>All available countries stored in database.</returns>
        // GET: Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
        {
            return await _context.Countries.ToListAsync();
        }
        /// <summary>
        /// Gets a specific country by Id.
        /// </summary>
        /// <remarks>
        /// /// Sample request:
        ///
        ///     GET /Countries/1
        /// </remarks>
        /// <param name="id"></param>   
        /// <returns>Specific country</returns>
        // GET: Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> GetCountry(int id)
        {
            var country = await _context.Countries.FindAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            return country;
        }
        /// <summary>
        /// Edit a specific country by Id.
        /// </summary>
        /// <remarks>
        /// /// Sample request:
        ///
        ///     PUT /Countries/1
        ///     {
        ///         "name": "Slovenia"
        ///     }
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="country"></param>
        /// <returns>Edited country</returns>
        // PUT: Countries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, Country country)
        {
            if (id != country.Id)
            {
                return BadRequest();
            }

            _context.Entry(country).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
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
        /// Creates countries.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Countries
        ///     [
        ///         {
        ///             "name": "Slovenia"
        ///         },
        ///         {
        ///             "name": "Macedonia"
        ///         },
        ///         {
        ///             "name": "Germany"
        ///         }
        ///     ]
        ///     
        ///
        /// </remarks>
        /// <param name="countries"></param>
        /// <returns>A newly created Countries</returns>
        // POST: Countries
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Country>>> CreateCountries(List<Country> countries)
        {
            List<Country> addedContries = new List<Country>();
            for(int i = 0; i < countries.Count; i++)
            {
                var checkIfExists = CountryExistsByName(countries[i].Name.ToLower());
                if(!checkIfExists)
                {
                    addedContries.Add(countries[i]);
                    _context.Countries.Add(countries[i]);
                    await _context.SaveChangesAsync();
                }
            }
            
            return CreatedAtAction(nameof(GetCountries),addedContries);
        }


        /// <summary>
        /// Deletes a specific country by Id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /Countries/1
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>Deleted country.</returns>
        // DELETE: Countries/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Country>> DeleteCountry(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();

            return country;
        }

        private bool CountryExists(int id)
        {
            return _context.Countries.Any(e => e.Id == id);
        }
        
        private bool CountryExistsByName(string name)
        {
            return _context.Countries.Any(e => e.Name == name);
        }
    }
}
