using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VictuzAppMVC.Models;

namespace VictuzAppMVC.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GebruikersAPIController : ControllerBase
    {
        private readonly VictuzAppContext _context;

        public GebruikersAPIController(VictuzAppContext context)
        {
            _context = context;
        }

        // GET: api/GebruikersAPI
        [HttpGet]
        public async Task<IActionResult> GetGebruikers()
        {
            var gebruikers = await _context.Gebruikers.ToListAsync();
            if (gebruikers == null)
            {
                return NotFound();
            }
            return Ok(gebruikers); // Gebruik Ok() om een 200-response terug te geven
        }

        // GET: api/GebruikersAPI/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGebruiker(int id)
        {
            var gebruiker = await _context.Gebruikers.FindAsync(id);

            if (gebruiker == null)
            {
                return NotFound();
            }

            return Ok(gebruiker); // Gebruik Ok() voor een succesvolle respons
        }

        // PUT: api/GebruikersAPI/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Bestuurslid")] // Voeg rol-gebaseerde autorisatie toe
        public async Task<IActionResult> PutGebruiker(int id, Gebruiker gebruiker)
        {
            if (id != gebruiker.GebruikerId)
            {
                return BadRequest("Gebruiker ID komt niet overeen.");
            }

            _context.Entry(gebruiker).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GebruikerExists(id))
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

        // POST: api/GebruikersAPI
        [HttpPost]
        [Authorize(Roles = "Bestuurslid")]
        public async Task<IActionResult> PostGebruiker(Gebruiker gebruiker)
        {
            if (gebruiker == null)
            {
                return BadRequest("Gebruiker is ongeldig.");
            }

            _context.Gebruikers.Add(gebruiker);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGebruiker), new { id = gebruiker.GebruikerId }, gebruiker);
        }

        // DELETE: api/GebruikersAPI/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Bestuurslid")]
        public async Task<IActionResult> DeleteGebruiker(int id)
        {
            var gebruiker = await _context.Gebruikers.FindAsync(id);
            if (gebruiker == null)
            {
                return NotFound();
            }

            _context.Gebruikers.Remove(gebruiker);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // FILTERS: api/GebruikersAPI/filter
        [HttpGet("filter")]
        public async Task<IActionResult> FilterGebruikers([FromQuery] string naam, [FromQuery] int? lidmaatschapId)
        {
            var query = _context.Gebruikers.AsQueryable();

            if (!string.IsNullOrEmpty(naam))
            {
                query = query.Where(g => g.Naam.Contains(naam));
            }

            if (lidmaatschapId.HasValue)
            {
                query = query.Where(g => g.LidmaatschapId == lidmaatschapId.Value);
            }

            var gebruikers = await query.ToListAsync();

            return Ok(gebruikers);
        }

        // Controleer of gebruiker bestaat (hulpmethode)
        private bool GebruikerExists(int id)
        {
            return _context.Gebruikers.Any(e => e.GebruikerId == id);
        }
    }
}
