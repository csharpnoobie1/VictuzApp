using System;
using System.Collections.Generic;

namespace VictuzAppMVC.Models;

public partial class Gebruiker
{
    public int GebruikerId { get; set; }

    public string Naam { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Wachtwoord { get; set; } = null!;

    public bool IsBestuurslid { get; set; }

    public int? LidmaatschapId { get; set; }

    public virtual ICollection<Aanmeldingen> Aanmeldingens { get; set; } = new List<Aanmeldingen>();

    public virtual Lidmaatschappen? Lidmaatschap { get; set; }
}
