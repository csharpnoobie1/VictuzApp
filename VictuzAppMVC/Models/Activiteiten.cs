using System;
using System.Collections.Generic;

namespace VictuzAppMVC.Models;

public partial class Activiteiten
{
    public int ActiviteitId { get; set; }

    public string Titel { get; set; } = null!;

    public DateTime Datum { get; set; }

    public int? MaxDeelnemers { get; set; }

    public string? Beschrijving { get; set; }

    public virtual ICollection<Aanmeldingen> Aanmeldingens { get; set; } = new List<Aanmeldingen>();

    public virtual ICollection<EvenementCategorieën> Categories { get; set; } = new List<EvenementCategorieën>();
}
