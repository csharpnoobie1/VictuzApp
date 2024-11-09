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
    public class AanmeldingenAPIController : ControllerBase
    {
        private readonly VictuzAppContext _context;

        public AanmeldingenAPIController(VictuzAppContext context)
        {
            _context = context;
        }

        // GET: api/Aanmeldingen
        [HttpGet]
        public async Task<IActionResult> GetAanmeldingen()
        {
            var aanmeldingen = await _context.Aanmeldingen
                .Include(a => a.Activiteit) // Voeg gerelateerde activiteit toe
                .Include(a => a.Gebruiker) // Voeg gerelateerde gebruiker toe
                .ToListAsync();

            if (aanmeldingen == null)
            {
                return NotFound("Geen aanmeldingen gevonden.");
            }

            return Ok(aanmeldingen);
        }

        // GET: api/Aanmeldingen/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAanmeldingen(int id)
        {
            var aanmelding = await _context.Aanmeldingen
                .Include(a => a.Activiteit)
                .Include(a => a.Gebruiker)
                .FirstOrDefaultAsync(a => a.AanmeldingId == id);

            if (aanmelding == null)
            {
                return NotFound($"Aanmelding met ID {id} niet gevonden.");
            }

            return Ok(aanmelding);
        }

        // PUT: api/Aanmeldingen/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Bestuurslid")] // Alleen bestuursleden mogen bewerken
        public async Task<IActionResult> PutAanmeldingen(int id, Aanmeldingen aanmeldingen)
        {
            if (id != aanmeldingen.AanmeldingId)
            {
                return BadRequest("De opgegeven AanmeldingId komt niet overeen.");
            }

            _context.Entry(aanmeldingen).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AanmeldingenExists(id))
                {
                    return NotFound($"Aanmelding met ID {id} bestaat niet.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Aanmeldingen
        [HttpPost]
        public async Task<IActionResult> PostAanmeldingen(Aanmeldingen aanmeldingen)
        {
            if (aanmeldingen == null)
            {
                return BadRequest("Aanmelding informatie is ongeldig.");
            }

            // Controleer of de gebruiker al is aangemeld
            if (_context.Aanmeldingen.Any(a => a.GebruikerId == aanmeldingen.GebruikerId && a.ActiviteitId == aanmeldingen.ActiviteitId))
            {
                return BadRequest("De gebruiker is al aangemeld voor deze activiteit.");
            }

            _context.Aanmeldingen.Add(aanmeldingen);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAanmeldingen), new { id = aanmeldingen.AanmeldingId }, aanmeldingen);
        }

        // DELETE: api/Aanmeldingen/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Bestuurslid")] // Alleen bestuursleden mogen verwijderen
        public async Task<IActionResult> DeleteAanmeldingen(int id)
        {
            var aanmelding = await _context.Aanmeldingen.FindAsync(id);
            if (aanmelding == null)
            {
                return NotFound($"Aanmelding met ID {id} niet gevonden.");
            }

            _context.Aanmeldingen.Remove(aanmelding);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Controleer of aanmelding bestaat (hulpmethode)
        private bool AanmeldingenExists(int id)
        {
            return _context.Aanmeldingen.Any(e => e.AanmeldingId == id);
        }
    }
}
