using System;
using System.Collections.Generic;

namespace AuthenticationAPI.Models;

public partial class Master
{
    public int IdMaster { get; set; }

    public int IdCentro { get; set; }

    public int IdDivision { get; set; }

    public int IdPais { get; set; }

    public int IdEmpresa { get; set; }

    public int IdLinea { get; set; }

    public virtual Centro IdCentroNavigation { get; set; } = null!;

    public virtual Division IdDivisionNavigation { get; set; } = null!;

    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    public virtual Linea IdLineaNavigation { get; set; } = null!;

    public virtual Pai IdPaisNavigation { get; set; } = null!;

    public virtual ICollection<Nivel> Nivels { get; } = new List<Nivel>();
}
