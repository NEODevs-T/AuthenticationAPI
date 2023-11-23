using System;
using System.Collections.Generic;

namespace AuthenticationAPI.Models;

public partial class Pai
{
    public int IdPais { get; set; }

    public string Pnombre { get; set; } = null!;

    public bool Pestado { get; set; }

    public virtual ICollection<Master> Masters { get; } = new List<Master>();
}
