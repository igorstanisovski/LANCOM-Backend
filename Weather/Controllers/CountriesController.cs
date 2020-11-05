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

        // GET: Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
        {
            return await _context.Countries.ToListAsync();
        }

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

        // POST: Countries
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Country>>> CreateCountries(List<Country> countries)
        {
            for(int i = 0; i < countries.Count; i++)
            {
                var checkIfExists = CountryExistsByName(countries[i].Name);
                if(!checkIfExists)
                {
                    _context.Countries.Add(countries[i]);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    countries.RemoveAt(i);
                    //return NoContent();
                }
            }
            
            return CreatedAtAction(nameof(GetCountries),countries);
        }



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
