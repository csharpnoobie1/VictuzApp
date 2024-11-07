using System;
using System.Collections.Generic;

namespace VictuzAppMVC.Models;

public partial class Aanmeldingen
{
    public int AanmeldingId { get; set; }

    public int GebruikerId { get; set; }

    public int ActiviteitId { get; set; }

    public DateTime? AanmeldDatum { get; set; }

    public virtual Activiteiten Activiteit { get; set; } = null!;

    public virtual Gebruiker Gebruiker { get; set; } = null!;
}
