using System;
using System.Collections.Generic;

namespace AuthenticationAPI.Models;

public partial class Centro
{
    public int IdCentro { get; set; }

    public string Cnom { get; set; } = null!;

    public string? Cdetalle { get; set; }

    public bool Cestado { get; set; }

    public virtual ICollection<Master> Masters { get; } = new List<Master>();
}
