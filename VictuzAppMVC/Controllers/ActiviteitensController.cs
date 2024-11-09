using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VictuzAppMVC.Models;
using VictuzAppMVC.Controllers.API; // Zorg ervoor dat je de API-controller importeert
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace VictuzAppMVC.Controllers
{
    public class ActiviteitensController : Controller
    {
        private readonly ActiviteitenAPIController _activiteitenApiController;

        public ActiviteitensController(ActiviteitenAPIController activiteitenApiController)
        {
            _activiteitenApiController = activiteitenApiController;
        }

        public async Task<IActionResult> Index()
        {
            // Roep de API-controller aan en sla het resultaat op
            var result = await _activiteitenApiController.GetActiviteiten();

            // Controleer of het resultaat een OkObjectResult is
            if (result is OkObjectResult okResult)
            {
                // Cast de waarde van het OkObjectResult naar de juiste lijst
                var activiteiten = okResult.Value as List<Activiteiten>;
                if (activiteiten != null)
                {
                    return View(activiteiten); // Geef de activiteiten door aan de view
                }
            }
            else if (result is NotFoundResult)
            {
                // Als er geen activiteiten zijn gevonden, geef een foutpagina terug
                return View("Error");
            }

            // Voor elk ander resultaat geef je een algemene foutpagina terug
            return View("Error");
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Roep de API aan om details op te halen
            var result = await _activiteitenApiController.GetActiviteiten(id.Value);

            if (result is OkObjectResult okResult)
            {
                var activiteit = okResult.Value as Activiteiten;
                if (activiteit != null)
                {
                    return View(activiteit);
                }
            }
            else if (result is NotFoundResult)
            {
                return NotFound();
            }

            return View("Error");
        }


        // GET: Activiteitens/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActiviteitId,Titel,Datum,MaxDeelnemers,Beschrijving")] Activiteiten activiteit)
        {
            if (ModelState.IsValid)
            {
                // Roep de API aan om de activiteit te creëren
                var result = await _activiteitenApiController.PostActiviteiten(activiteit);

                // Controleer of het resultaat een CreatedAtActionResult is
                if (result is CreatedAtActionResult)
                {
                    return RedirectToAction(nameof(Index)); // Succesvol, terug naar index
                }
                else if (result is BadRequestObjectResult badRequestResult)
                {
                    // Als de API een BadRequest retourneert, toon een foutpagina met details
                    ViewBag.ErrorMessage = badRequestResult.Value?.ToString() ?? "Er is een fout opgetreden.";
                    return View("Error");
                }
            }

            // Als het model ongeldig is, geef het formulier opnieuw weer
            return View(activiteit);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _activiteitenApiController.GetActiviteiten(id.Value);

            if (result is OkObjectResult okResult)
            {
                var activiteit = okResult.Value as Activiteiten;
                if (activiteit == null)
                {
                    return NotFound();
                }
                return View(activiteit);
            }

            return View("Error");
        }



        // POST: Activiteitens/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ActiviteitId,Titel,Datum,MaxDeelnemers,Beschrijving")] Activiteiten activiteit)
        {
            if (id != activiteit.ActiviteitId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _activiteitenApiController.PutActiviteiten(id, activiteit);

                if (result is NoContentResult)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View("Error");
                }
            }

            return View(activiteit);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Roep de API aan om activiteitdetails op te halen
            var result = await _activiteitenApiController.GetActiviteiten(id.Value);

            // Controleer of het resultaat een OkObjectResult is
            if (result is OkObjectResult okResult)
            {
                var activiteit = okResult.Value as Activiteiten;
                if (activiteit != null)
                {
                    return View(activiteit); // Geef de activiteit door aan de view
                }
            }
            else if (result is NotFoundResult)
            {
                return NotFound(); // Activiteit niet gevonden
            }

            // Geef een foutpagina weer bij een ander resultaat
            return View("Error");
        }



        // POST: Activiteitens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _activiteitenApiController.DeleteActiviteiten(id);

            if (result is NoContentResult)
            {
                return RedirectToAction(nameof(Index));
            }

            return View("Error");
        }
    }
}
