using System;
using System.Collections.Generic;

namespace VictuzAppMVC.Models;

public partial class Activiteiten
{
    public int ActiviteitId { get; set; } // Primaire sleutel

    public string Titel { get; set; } = null!; // Titel van de activiteit

    public DateTime Datum { get; set; } // Datum van de activiteit

    public int? MaxDeelnemers { get; set; } // Maximaal aantal deelnemers (optioneel)

    public string? Beschrijving { get; set; } // Beschrijving van de activiteit (optioneel)

    // Nieuwe eigenschappen
    public string? Type { get; set; } // Type van de activiteit (bijv. workshop, seminar) (optioneel)

    public string? Locatie { get; set; } // Locatie van de activiteit (bijv. lokaalnaam of adres) (optioneel)

    public bool IsVoorstel { get; set; } = false; // Markering of dit een voorstel is (standaard: false)

    // Navigatie-eigenschappen
    public virtual ICollection<Aanmeldingen> Aanmeldingens { get; set; } = new List<Aanmeldingen>();

    public virtual ICollection<EvenementCategorieën> Categories { get; set; } = new List<EvenementCategorieën>();
}
