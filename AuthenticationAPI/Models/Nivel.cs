using System;
using System.Collections.Generic;

namespace AuthenticationAPI.Models;

public partial class Nivel
{
    public int IdNivel { get; set; }

    public int IdUsuario { get; set; }

    public int IdProyecto { get; set; }

    public int? IdDivision { get; set; }

    public int? IdMaster { get; set; }

    public int? IdRol { get; set; }

    public virtual Master? IdMasterNavigation { get; set; }

    public virtual ProyectoUsr IdProyectoNavigation { get; set; } = null!;

    public virtual Rol? IdRolNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
