using System;
using System.Collections.Generic;

namespace VictuzAppMVC.Models;

public partial class Lidmaatschappen
{
    public int LidmaatschapId { get; set; }

    public string Status { get; set; } = null!;

    public string? Beschrijving { get; set; }

    public virtual ICollection<Gebruiker> Gebruikers { get; set; } = new List<Gebruiker>();
}
