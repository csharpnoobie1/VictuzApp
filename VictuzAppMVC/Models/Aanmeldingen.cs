using System;
using System.Collections.Generic;

namespace VictuzAppMVC.Models;

public partial class Aanmeldingen
{
    public int AanmeldingId { get; set; } // Primaire sleutel

    public int GebruikerId { get; set; } // Verwijzing naar de gebruiker

    public int ActiviteitId { get; set; } // Verwijzing naar de activiteit

    public DateTime AanmeldDatum { get; set; } = DateTime.Now; // Datum van aanmelding (standaardwaarde: huidige datum)

    // Navigatie-eigenschappen
    public virtual Activiteiten Activiteit { get; set; } = null!; // Relatie naar Activiteiten

    public virtual Gebruiker Gebruiker { get; set; } = null!; // Relatie naar Gebruikers
}
