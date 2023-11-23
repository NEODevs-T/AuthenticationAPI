using System;
using System.Collections.Generic;

namespace AuthenticationAPI.Models;

public partial class Linea
{
    public int IdLinea { get; set; }

    public string Lnom { get; set; } = null!;

    public string? Ldetalle { get; set; }

    public bool Lestado { get; set; }

    public string? LcenCos { get; set; }

    public string? Lofic { get; set; }

    public virtual ICollection<Master> Masters { get; } = new List<Master>();
}
