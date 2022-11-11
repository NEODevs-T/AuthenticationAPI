using System;
using System.Collections.Generic;

namespace AuthenticationAPI.Models;

public partial class ProyectoUsr
{
    public int IdProyecto { get; set; }

    public string Pnombre { get; set; } = null!;

    public bool Pestado { get; set; }

    public virtual ICollection<Nivel> Nivels { get; } = new List<Nivel>();
}
