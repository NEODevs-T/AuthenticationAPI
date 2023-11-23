using System;
using System.Collections.Generic;

namespace AuthenticationAPI.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string UsNombre { get; set; } = null!;

    public bool UsEstatus { get; set; }

    public string UsClave { get; set; } = null!;

    public string? UsCorreo { get; set; }

    public string UsApellido { get; set; } = null!;

    public string UsUsuario { get; set; } = null!;

    public string UsFicha { get; set; } = null!;

    public string UsPass { get; set; } = null!;

    public virtual ICollection<Nivel> Nivels { get; } = new List<Nivel>();
}
