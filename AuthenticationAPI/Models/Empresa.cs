using System;
using System.Collections.Generic;

namespace AuthenticationAPI.Models;

public partial class Empresa
{
    public int IdEmpresa { get; set; }

    public string Enombre { get; set; } = null!;

    public string? Edescri { get; set; }

    public bool Eestado { get; set; }

    public virtual ICollection<Master> Masters { get; } = new List<Master>();
}
