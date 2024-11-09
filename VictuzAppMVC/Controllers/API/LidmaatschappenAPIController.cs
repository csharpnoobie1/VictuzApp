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
    public class LidmaatschappenAPIController : ControllerBase
    {
        private readonly VictuzAppContext _context;

        public LidmaatschappenAPIController(VictuzAppContext context)
        {
            _context = context;
        }

        // GET: api/LidmaatschappenAPI
        [HttpGet]
        public async Task<IActionResult> GetLidmaatschappens()
        {
            var lidmaatschappens = await _context.Lidmaatschappens.ToListAsync();
            if (lidmaatschappens == null)
            {
                return NotFound("Geen lidmaatschappen gevonden.");
            }
            return Ok(lidmaatschappens);
        }

        // GET: api/LidmaatschappenAPI/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLidmaatschappen(int id)
        {
            var lidmaatschappen = await _context.Lidmaatschappens.FindAsync(id);

            if (lidmaatschappen == null)
            {
                return NotFound($"Geen lidmaatschap gevonden met ID {id}.");
            }

            return Ok(lidmaatschappen);
        }

        // PUT: api/LidmaatschappenAPI/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Bestuurslid")] // Autorisatie toegevoegd
        public async Task<IActionResult> PutLidmaatschappen(int id, Lidmaatschappen lidmaatschappen)
        {
            if (id != lidmaatschappen.LidmaatschapId)
            {
                return BadRequest("Lidmaatschap ID komt niet overeen.");
            }

            _context.Entry(lidmaatschappen).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LidmaatschappenExists(id))
                {
                    return NotFound($"Lidmaatschap met ID {id} bestaat niet.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/LidmaatschappenAPI
        [HttpPost]
        [Authorize(Roles = "Bestuurslid")]
        public async Task<IActionResult> PostLidmaatschappen(Lidmaatschappen lidmaatschappen)
        {
            if (lidmaatschappen == null)
            {
                return BadRequest("Lidmaatschap informatie is ongeldig.");
            }

            _context.Lidmaatschappens.Add(lidmaatschappen);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLidmaatschappen), new { id = lidmaatschappen.LidmaatschapId }, lidmaatschappen);
        }

        // DELETE: api/LidmaatschappenAPI/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Bestuurslid")]
        public async Task<IActionResult> DeleteLidmaatschappen(int id)
        {
            var lidmaatschappen = await _context.Lidmaatschappens.FindAsync(id);
            if (lidmaatschappen == null)
            {
                return NotFound($"Lidmaatschap met ID {id} is niet gevonden.");
            }

            _context.Lidmaatschappens.Remove(lidmaatschappen);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // FILTERS: api/LidmaatschappenAPI/filter
        [HttpGet("filter")]
        public async Task<IActionResult> FilterLidmaatschappens([FromQuery] string status)
        {
            var query = _context.Lidmaatschappens.AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(l => l.Status.Contains(status));
            }

            var filteredLidmaatschappens = await query.ToListAsync();
            if (!filteredLidmaatschappens.Any())
            {
                return NotFound("Geen lidmaatschappen gevonden die aan de filtercriteria voldoen.");
            }

            return Ok(filteredLidmaatschappens);
        }

        // Controleer of lidmaatschap bestaat (hulpmethode)
        private bool LidmaatschappenExists(int id)
        {
            return _context.Lidmaatschappens.Any(e => e.LidmaatschapId == id);
        }
    }
}
