using System;
using System.Collections.Generic;

namespace AuthenticationAPI.Models;

public partial class Nivel
{
    public int IdNivel { get; set; }

    public int IdUsuario { get; set; }

    public int IdProyecto { get; set; }

    public int Nrol { get; set; }

    public string? NrolDesc { get; set; }

    public virtual ProyectoUsr IdProyectoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
