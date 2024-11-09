using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VictuzAppMVC.Controllers.API;
using VictuzAppMVC.Models;

namespace VictuzAppMVC.Controllers
{
    public class GebruikersController : Controller
    {
        private readonly GebruikersAPIController _gebruikersApiController;
        private readonly LidmaatschappenAPIController _lidmaatschappenApiController;


        public GebruikersController(
            GebruikersAPIController gebruikersApiController,
            LidmaatschappenAPIController lidmaatschappenApiController)
        {
            _gebruikersApiController = gebruikersApiController;
            _lidmaatschappenApiController = lidmaatschappenApiController;
        }

        // GET: Gebruikers
        public async Task<IActionResult> Index()
        {
            var result = await _gebruikersApiController.GetGebruikers();

            if (result is OkObjectResult okResult)
            {
                var gebruikers = okResult.Value as List<Gebruiker>;
                if (gebruikers != null)
                {
                    return View(gebruikers);
                }
            }

            return View("Error");
        }

        // GET: Gebruikers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _gebruikersApiController.GetGebruiker(id.Value);

            if (result is OkObjectResult okResult)
            {
                var gebruiker = okResult.Value as Gebruiker;
                if (gebruiker != null)
                {
                    return View(gebruiker);
                }
            }

            return View("Error");
        }

        // GET: Gebruikers/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                // Haal de lidmaatschappen op via de API
                var lidmaatschappenResult = await _lidmaatschappenApiController.GetLidmaatschappens();

                // Controleer of de API een geldige lijst retourneert
                if (lidmaatschappenResult is OkObjectResult okResult)
                {
                    var lidmaatschappen = okResult.Value as List<Lidmaatschappen>;

                    // Controleer of lidmaatschappen succesvol zijn opgehaald
                    if (lidmaatschappen != null && lidmaatschappen.Any())
                    {
                        // Vul de SelectList met lidmaatschappen
                        ViewData["LidmaatschapId"] = new SelectList(lidmaatschappen, "LidmaatschapId", "Status");
                    }
                    else
                    {
                        // Voeg een waarschuwing toe als de lijst leeg is
                        ModelState.AddModelError(string.Empty, "Geen lidmaatschappen gevonden. Controleer de gegevens.");
                    }
                }
                else
                {
                    // Voeg een fout toe als de API geen geldige respons retourneert
                    ModelState.AddModelError(string.Empty, "Kan geen verbinding maken met de API om lidmaatschappen op te halen.");
                }
            }
            catch (Exception ex)
            {
                // Log de fout (optioneel) en toon een algemene foutmelding
                ModelState.AddModelError(string.Empty, $"Er is een fout opgetreden: {ex.Message}");
            }

            return View();
        }


        // POST: Gebruikers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GebruikerId,Naam,Email,Wachtwoord,IsBestuurslid,LidmaatschapId")] Gebruiker gebruiker)
        {
            if (ModelState.IsValid)
            {
                // Roep de API aan om de gebruiker toe te voegen
                var result = await _gebruikersApiController.PostGebruiker(gebruiker);

                // Controleer of de API een succesvolle CreatedAtActionResult retourneert
                if (result is CreatedAtActionResult)
                {
                    return RedirectToAction(nameof(Index)); // Keer terug naar de indexpagina
                }
                else if (result is BadRequestObjectResult badRequest)
                {
                    // Verwerk eventuele fouten geretourneerd door de API
                    ModelState.AddModelError(string.Empty, badRequest.Value?.ToString() ?? "Er is een fout opgetreden bij het toevoegen van de gebruiker.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Onverwachte fout bij het toevoegen van de gebruiker.");
                }
            }

            // Haal lidmaatschappen opnieuw op als het model ongeldig is of een fout optreedt
            var lidmaatschappenResult = await _lidmaatschappenApiController.GetLidmaatschappens();
            if (lidmaatschappenResult is OkObjectResult okResult)
            {
                var lidmaatschappen = okResult.Value as List<Lidmaatschappen>;
                ViewData["LidmaatschapId"] = new SelectList(lidmaatschappen, "LidmaatschapId", "Status", gebruiker.LidmaatschapId);
            }
            else
            {
                // Voeg een algemene fout toe als het ophalen van lidmaatschappen mislukt
                ModelState.AddModelError(string.Empty, "Kan lidmaatschappen niet ophalen. Probeer het opnieuw.");
            }

            // Geef het formulier opnieuw weer met foutmeldingen
            return View(gebruiker);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _gebruikersApiController.GetGebruiker(id.Value);

            if (result is OkObjectResult okResult)
            {
                var gebruiker = okResult.Value as Gebruiker;
                if (gebruiker != null)
                {
                    var lidmaatschappenResult = await _lidmaatschappenApiController.GetLidmaatschappens();
                    if (lidmaatschappenResult is OkObjectResult lidmaatschappenOkResult)
                    {
                        var lidmaatschappen = lidmaatschappenOkResult.Value as List<Lidmaatschappen>;
                        ViewData["LidmaatschapId"] = new SelectList(lidmaatschappen, "LidmaatschapId", "Status", gebruiker.LidmaatschapId);
                    }

                    return View(gebruiker);
                }
            }

            return View("Error");
        }


        // POST: Gebruikers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GebruikerId,Naam,Email,Wachtwoord,IsBestuurslid,LidmaatschapId")] Gebruiker gebruiker)
        {
            if (id != gebruiker.GebruikerId)
            {
                ModelState.AddModelError(string.Empty, "Het opgegeven GebruikerID komt niet overeen.");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Roep de API aan om de gebruiker bij te werken
                    var result = await _gebruikersApiController.PutGebruiker(id, gebruiker);

                    if (result is NoContentResult)
                    {
                        // Succesvolle update, keer terug naar indexpagina
                        return RedirectToAction(nameof(Index));
                    }
                    else if (result is BadRequestObjectResult badRequest)
                    {
                        // Verwerk eventuele API-validatiefouten
                        ModelState.AddModelError(string.Empty, badRequest.Value?.ToString() ?? "Er is een fout opgetreden bij het bijwerken van de gebruiker.");
                    }
                    else
                    {
                        // Onverwachte API-respons
                        ModelState.AddModelError(string.Empty, "Onverwachte fout tijdens het bijwerken van de gebruiker.");
                    }
                }
                catch (Exception ex)
                {
                    // Log optioneel de fout en voeg een algemene foutmelding toe
                    ModelState.AddModelError(string.Empty, $"Er is een fout opgetreden: {ex.Message}");
                }
            }

            // Haal lidmaatschappen opnieuw op om de dropdown bij te werken
            try
            {
                var lidmaatschappenResult = await _lidmaatschappenApiController.GetLidmaatschappens();

                if (lidmaatschappenResult is OkObjectResult okResult)
                {
                    var lidmaatschappen = okResult.Value as List<Lidmaatschappen>;
                    if (lidmaatschappen != null && lidmaatschappen.Any())
                    {
                        ViewData["LidmaatschapId"] = new SelectList(lidmaatschappen, "LidmaatschapId", "Status", gebruiker.LidmaatschapId);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Geen beschikbare lidmaatschappen gevonden.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Kan geen lidmaatschappen ophalen van de API.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Er is een fout opgetreden bij het ophalen van lidmaatschappen: {ex.Message}");
            }

            // Geef de view opnieuw weer met foutmeldingen
            return View(gebruiker);
        }


        // GET: Gebruikers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _gebruikersApiController.GetGebruiker(id.Value);

            if (result is OkObjectResult okResult)
            {
                var gebruiker = okResult.Value as Gebruiker;
                if (gebruiker != null)
                {
                    return View(gebruiker);
                }
            }

            return View("Error");
        }

        // POST: Gebruikers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _gebruikersApiController.DeleteGebruiker(id);

            if (result is NoContentResult)
            {
                return RedirectToAction(nameof(Index));
            }

            return View("Error");
        }
    }
}
