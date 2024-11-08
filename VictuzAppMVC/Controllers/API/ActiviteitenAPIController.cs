using System;
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
    public class ActiviteitenAPIController : ControllerBase
    {
        private readonly VictuzAppContext _context;

        public ActiviteitenAPIController(VictuzAppContext context)
        {
            _context = context;
        }

        // GET: api/ActiviteitenAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Activiteiten>>> GetActiviteiten()
        {
            return await _context.Activiteitens.ToListAsync();
        }

        // GET: api/ActiviteitenAPI/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Activiteiten>> GetActiviteiten(int id)
        {
            var activiteiten = await _context.Activiteitens.FindAsync(id);

            if (activiteiten == null)
            {
                return NotFound();
            }

            return activiteiten;
        }

        // POST: api/ActiviteitenAPI
        [HttpPost]
        [Authorize(Roles = "Bestuurslid")]
        public async Task<ActionResult<Activiteiten>> PostActiviteiten(Activiteiten activiteiten)
        {
            if (activiteiten == null)
            {
                return BadRequest("Activiteit is ongeldig.");
            }

            _context.Activiteitens.Add(activiteiten);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActiviteiten", new { id = activiteiten.ActiviteitId }, activiteiten);
        }

        // PUT: api/ActiviteitenAPI/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Bestuurslid")]
        public async Task<IActionResult> PutActiviteiten(int id, Activiteiten activiteiten)
        {
            if (id != activiteiten.ActiviteitId)
            {
                return BadRequest("Activiteit ID komt niet overeen.");
            }

            _context.Entry(activiteiten).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActiviteitenExists(id))
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

        // DELETE: api/ActiviteitenAPI/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Bestuurslid")]
        public async Task<IActionResult> DeleteActiviteiten(int id)
        {
            var activiteiten = await _context.Activiteitens.FindAsync(id);

            if (activiteiten == null)
            {
                return NotFound();
            }

            _context.Activiteitens.Remove(activiteiten);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // AANMELDEN VOOR ACTIVITEITEN: api/ActiviteitenAPI/{id}/aanmelden
        [HttpPost("{id}/aanmelden")]
        [Authorize(Roles = "Lid")]
        public async Task<IActionResult> AanmeldenVoorActiviteit(int id)
        {
            var activiteit = await _context.Activiteitens.FindAsync(id);

            if (activiteit == null)
            {
                return NotFound("Activiteit niet gevonden.");
            }

            // Controleer of er nog plek is
            if (_context.Aanmeldingen.Count(a => a.ActiviteitId == id) >= activiteit.MaxDeelnemers)
            {
                return BadRequest("Activiteit is vol.");
            }

            var gebruikerId = int.Parse(User.FindFirst("GebruikerId").Value); // Haal gebruiker ID uit claims

            // Controleer of de gebruiker al is aangemeld
            if (_context.Aanmeldingen.Any(a => a.GebruikerId == gebruikerId && a.ActiviteitId == id))
            {
                return BadRequest("Je bent al aangemeld voor deze activiteit.");
            }

            var aanmelding = new Aanmeldingen
            {
                GebruikerId = gebruikerId,
                ActiviteitId = id,
                AanmeldDatum = DateTime.Now
            };

            _context.Aanmeldingen.Add(aanmelding);
            await _context.SaveChangesAsync();

            return Ok("Succesvol aangemeld.");
        }

        // FILTERS: api/ActiviteitenAPI/filter
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Activiteiten>>> FilterActiviteiten(
            [FromQuery] string type,
            [FromQuery] DateTime? startDatum,
            [FromQuery] string locatie)
        {
            var query = _context.Activiteitens.AsQueryable();

            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(a => a.Type == type);
            }

            if (startDatum.HasValue)
            {
                query = query.Where(a => a.Datum >= startDatum.Value);
            }

            if (!string.IsNullOrEmpty(locatie))
            {
                query = query.Where(a => a.Locatie.Contains(locatie));
            }

            return await query.ToListAsync();
        }

        // VOORSTELLEN: api/ActiviteitenAPI/voorstel
        [HttpPost("voorstel")]
        [Authorize(Roles = "Lid")]
        public async Task<IActionResult> VoorstelActiviteit([FromBody] Activiteiten voorstel)
        {
            if (voorstel == null)
            {
                return BadRequest("Voorstel is ongeldig.");
            }

            voorstel.IsVoorstel = true; // Markeer dit als een voorstel
            _context.Activiteitens.Add(voorstel);
            await _context.SaveChangesAsync();

            return Ok("Voorstel ingediend. Het bestuur zal dit beoordelen.");
        }

        // Controleer of activiteit bestaat (hulpmethode)
        private bool ActiviteitenExists(int id)
        {
            return _context.Activiteitens.Any(e => e.ActiviteitId == id);
        }
    }
}
