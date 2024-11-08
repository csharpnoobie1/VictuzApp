using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<IEnumerable<Activiteiten>>> GetActiviteitens()
        {
            return await _context.Activiteitens.ToListAsync();
        }

        // GET: api/ActiviteitenAPI/5
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

        // PUT: api/ActiviteitenAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActiviteiten(int id, Activiteiten activiteiten)
        {
            if (id != activiteiten.ActiviteitId)
            {
                return BadRequest();
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

        // POST: api/ActiviteitenAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Activiteiten>> PostActiviteiten(Activiteiten activiteiten)
        {
            _context.Activiteitens.Add(activiteiten);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActiviteiten", new { id = activiteiten.ActiviteitId }, activiteiten);
        }

        // DELETE: api/ActiviteitenAPI/5
        [HttpDelete("{id}")]
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

        private bool ActiviteitenExists(int id)
        {
            return _context.Activiteitens.Any(e => e.ActiviteitId == id);
        }
    }
}
