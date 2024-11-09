using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VictuzAppMVC.Controllers.API;
using VictuzAppMVC.Models;

namespace VictuzAppMVC.Controllers
{
    public class AanmeldingenController : Controller
    {
        private readonly AanmeldingenAPIController _aanmeldingenApiController;
        private readonly ActiviteitenAPIController _activiteitenApiController;
        private readonly GebruikersAPIController _gebruikersApiController;

        public AanmeldingenController(
            AanmeldingenAPIController aanmeldingenApiController,
            ActiviteitenAPIController activiteitenApiController,
            GebruikersAPIController gebruikersApiController)
        {
            _aanmeldingenApiController = aanmeldingenApiController;
            _activiteitenApiController = activiteitenApiController;
            _gebruikersApiController = gebruikersApiController;
        }

        // GET: Aanmeldingen
        public async Task<IActionResult> Index()
        {
            var result = await _aanmeldingenApiController.GetAanmeldingen();
            if (result is OkObjectResult okResult)
            {
                var aanmeldingen = okResult.Value as List<Aanmeldingen>;
                return View(aanmeldingen);
            }

            return View("Error");
        }

        // GET: Aanmeldingen/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var result = await _aanmeldingenApiController.GetAanmeldingen(id.Value);
            if (result is OkObjectResult okResult)
            {
                var aanmelding = okResult.Value as Aanmeldingen;
                if (aanmelding != null) return View(aanmelding);
            }

            return View("Error");
        }

        public async Task<IActionResult> Create()
        {
            await SetViewBagForCreateOrEdit(); // Stel ViewBag in
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GebruikerId,ActiviteitId")] Aanmeldingen aanmelding)
        {
            if (ModelState.IsValid)
            {
                var result = await _aanmeldingenApiController.PostAanmeldingen(aanmelding);

                if (result is CreatedAtActionResult)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            // Haal dropdown-data opnieuw op als validatie faalt
            await SetViewBagForCreateOrEdit(aanmelding);

            return View(aanmelding);
        }


        private async Task SetViewBagForCreateOrEdit(Aanmeldingen aanmelding = null)
        {
            // Haal activiteiten op via de API
            var activiteitenResult = await _activiteitenApiController.GetActiviteiten();
            if (activiteitenResult is OkObjectResult activiteitenOkResult)
            {
                var activiteiten = activiteitenOkResult.Value as List<Activiteiten>;
                ViewBag.ActiviteitId = new SelectList(activiteiten, "ActiviteitId", "Titel", aanmelding?.ActiviteitId);
            }

            // Haal gebruikers op via de API
            var gebruikersResult = await _gebruikersApiController.GetGebruikers();
            if (gebruikersResult is OkObjectResult gebruikersOkResult)
            {
                var gebruikers = gebruikersOkResult.Value as List<Gebruiker>;
                ViewBag.GebruikerId = new SelectList(gebruikers, "GebruikerId", "Naam", aanmelding?.GebruikerId);
            }
        }


        // GET: Aanmeldingen/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var result = await _aanmeldingenApiController.GetAanmeldingen(id.Value);
            if (result is OkObjectResult okResult)
            {
                var aanmelding = okResult.Value as Aanmeldingen;
                if (aanmelding != null)
                {
                    await SetViewDataForCreateOrEdit(aanmelding);
                    return View(aanmelding);
                }
            }

            return View("Error");
        }

        // POST: Aanmeldingen/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AanmeldingId,GebruikerId,ActiviteitId,AanmeldDatum")] Aanmeldingen aanmeldingen)
        {
            if (id != aanmeldingen.AanmeldingId) return NotFound();

            if (ModelState.IsValid)
            {
                var result = await _aanmeldingenApiController.PutAanmeldingen(id, aanmeldingen);
                if (result is NoContentResult)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            // Haal opnieuw data op voor dropdowns
            await SetViewDataForCreateOrEdit(aanmeldingen);

            return View(aanmeldingen);
        }

        // GET: Aanmeldingen/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var result = await _aanmeldingenApiController.GetAanmeldingen(id.Value);
            if (result is OkObjectResult okResult)
            {
                var aanmelding = okResult.Value as Aanmeldingen;
                if (aanmelding != null) return View(aanmelding);
            }

            return View("Error");
        }

        // POST: Aanmeldingen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _aanmeldingenApiController.DeleteAanmeldingen(id);
            if (result is NoContentResult)
            {
                return RedirectToAction(nameof(Index));
            }

            return View("Error");
        }

        // Hulpmethode om ViewData in te stellen
        private async Task SetViewDataForCreateOrEdit(Aanmeldingen aanmelding)
        {
            var activiteitenResult = await _activiteitenApiController.GetActiviteiten();
            if (activiteitenResult is OkObjectResult activiteitenOkResult)
            {
                var activiteiten = activiteitenOkResult.Value as List<Activiteiten>;
                ViewData["ActiviteitId"] = new SelectList(activiteiten, "ActiviteitId", "Titel", aanmelding.ActiviteitId);
            }

            var gebruikersResult = await _gebruikersApiController.GetGebruikers();
            if (gebruikersResult is OkObjectResult gebruikersOkResult)
            {
                var gebruikers = gebruikersOkResult.Value as List<Gebruiker>;
                ViewData["GebruikerId"] = new SelectList(gebruikers, "GebruikerId", "Naam", aanmelding.GebruikerId);
            }
        }
    }
}
