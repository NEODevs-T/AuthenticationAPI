﻿using System;
using System.Collections.Generic;

namespace AuthenticationAPI.Models;

public partial class Empresa
{
    public int IdEmpresa { get; set; }

    public int IdPais { get; set; }

    public string Enombre { get; set; } = null!;

    public string? Edescri { get; set; }

    public bool Eestado { get; set; }

    public virtual ICollection<Centro> Centros { get; } = new List<Centro>();

    public virtual Pai IdPaisNavigation { get; set; } = null!;
}
