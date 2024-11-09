using System;
using System.Collections.Generic;

namespace VictuzAppMVC.Models;

public partial class EvenementCategorieën
{
    public int CategorieId { get; set; }

    public string CategorieNaam { get; set; } = null!;

    public virtual ICollection<Activiteiten> Activiteits { get; set; } = new List<Activiteiten>();
}
