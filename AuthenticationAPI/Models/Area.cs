using System;
using System.Collections.Generic;

namespace AuthenticationAPI.Models;

public partial class Area
{
    public int IdArea { get; set; }

    public string Anom { get; set; } = null!;

    public string? Adetalle { get; set; }

    public bool Aestado { get; set; }
}
