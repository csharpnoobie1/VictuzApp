using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VictuzAppMVC.Models;

namespace VictuzAppMVC.Controllers
{
    public class ActiviteitensController : Controller
    {
        private readonly VictuzAppContext _context;

        public ActiviteitensController(VictuzAppContext context)
        {
            _context = context;
        }

        // GET: Activiteitens
        public async Task<IActionResult> Index()
        {
            return View(await _context.Activiteitens.ToListAsync());
        }

        // GET: Activiteitens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activiteiten = await _context.Activiteitens
                .FirstOrDefaultAsync(m => m.ActiviteitId == id);
            if (activiteiten == null)
            {
                return NotFound();
            }

            return View(activiteiten);
        }

        // GET: Activiteitens/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Activiteitens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActiviteitId,Titel,Datum,MaxDeelnemers,Beschrijving")] Activiteiten activiteiten)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activiteiten);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(activiteiten);
        }

        // GET: Activiteitens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activiteiten = await _context.Activiteitens.FindAsync(id);
            if (activiteiten == null)
            {
                return NotFound();
            }
            return View(activiteiten);
        }

        // POST: Activiteitens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ActiviteitId,Titel,Datum,MaxDeelnemers,Beschrijving")] Activiteiten activiteiten)
        {
            if (id != activiteiten.ActiviteitId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activiteiten);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActiviteitenExists(activiteiten.ActiviteitId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(activiteiten);
        }

        // GET: Activiteitens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activiteiten = await _context.Activiteitens
                .FirstOrDefaultAsync(m => m.ActiviteitId == id);
            if (activiteiten == null)
            {
                return NotFound();
            }

            return View(activiteiten);
        }

        // POST: Activiteitens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activiteiten = await _context.Activiteitens.FindAsync(id);
            if (activiteiten != null)
            {
                _context.Activiteitens.Remove(activiteiten);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActiviteitenExists(int id)
        {
            return _context.Activiteitens.Any(e => e.ActiviteitId == id);
        }
    }
}
