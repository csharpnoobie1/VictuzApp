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
    public class LidmaatschappensController : Controller
    {
        private readonly VictuzAppContext _context;

        public LidmaatschappensController(VictuzAppContext context)
        {
            _context = context;
        }

        // GET: Lidmaatschappens
        public async Task<IActionResult> Index()
        {
            return View(await _context.Lidmaatschappens.ToListAsync());
        }

        // GET: Lidmaatschappens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lidmaatschappen = await _context.Lidmaatschappens
                .FirstOrDefaultAsync(m => m.LidmaatschapId == id);
            if (lidmaatschappen == null)
            {
                return NotFound();
            }

            return View(lidmaatschappen);
        }

        // GET: Lidmaatschappens/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lidmaatschappens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LidmaatschapId,Status,Beschrijving")] Lidmaatschappen lidmaatschappen)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lidmaatschappen);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lidmaatschappen);
        }

        // GET: Lidmaatschappens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lidmaatschappen = await _context.Lidmaatschappens.FindAsync(id);
            if (lidmaatschappen == null)
            {
                return NotFound();
            }
            return View(lidmaatschappen);
        }

        // POST: Lidmaatschappens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LidmaatschapId,Status,Beschrijving")] Lidmaatschappen lidmaatschappen)
        {
            if (id != lidmaatschappen.LidmaatschapId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lidmaatschappen);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LidmaatschappenExists(lidmaatschappen.LidmaatschapId))
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
            return View(lidmaatschappen);
        }

        // GET: Lidmaatschappens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lidmaatschappen = await _context.Lidmaatschappens
                .FirstOrDefaultAsync(m => m.LidmaatschapId == id);
            if (lidmaatschappen == null)
            {
                return NotFound();
            }

            return View(lidmaatschappen);
        }

        // POST: Lidmaatschappens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lidmaatschappen = await _context.Lidmaatschappens.FindAsync(id);
            if (lidmaatschappen != null)
            {
                _context.Lidmaatschappens.Remove(lidmaatschappen);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LidmaatschappenExists(int id)
        {
            return _context.Lidmaatschappens.Any(e => e.LidmaatschapId == id);
        }
    }
}
