using System;
using System.Collections.Generic;

namespace VictuzAppMVC.Models;

public partial class ViewActiviteitenAanmeldingen
{
    public int ActiviteitId { get; set; }

    public string Titel { get; set; } = null!;

    public DateTime Datum { get; set; }

    public int? MaxDeelnemers { get; set; }

    public int? AantalAanmeldingen { get; set; }
}
