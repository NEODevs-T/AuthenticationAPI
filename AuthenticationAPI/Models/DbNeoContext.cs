using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationAPI.Models;

public partial class DbNeoContext : DbContext
{
    public DbNeoContext()
    {
    }

    public DbNeoContext(DbContextOptions<DbNeoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Empresa> Empresas { get; set; }

    public virtual DbSet<Nivel> Nivels { get; set; }

    public virtual DbSet<ProyectoUsr> ProyectoUsrs { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=10.20.1.60\\DESARROLLO;Initial Catalog=DbNeo;TrustServerCertificate=True;Persist Security Info=True;User ID=UsrEncuesta;Password=Enc2022**Ing");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.HasKey(e => e.IdEmpresa);

            entity.ToTable("Empresa");

            entity.Property(e => e.Edescri)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("EDescri");
            entity.Property(e => e.Eestado).HasColumnName("EEstado");
            entity.Property(e => e.Enombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ENombre");
        });

        modelBuilder.Entity<Nivel>(entity =>
        {
            entity.HasKey(e => e.IdNivel);

            entity.ToTable("Nivel");

            entity.Property(e => e.Nrol).HasColumnName("NRol");
            entity.Property(e => e.NrolDesc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NRolDesc");

            entity.HasOne(d => d.IdProyectoNavigation).WithMany(p => p.Nivels)
                .HasForeignKey(d => d.IdProyecto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Nivel_ProyectoUsr");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Nivels)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Nivel_Usuario");
        });

        modelBuilder.Entity<ProyectoUsr>(entity =>
        {
            entity.HasKey(e => e.IdProyecto).HasName("PK_Proyecto_1");

            entity.ToTable("ProyectoUsr");

            entity.Property(e => e.Pestado).HasColumnName("PEstado");
            entity.Property(e => e.Pnombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PNombre");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);

            entity.ToTable("Usuario", tb => tb.HasComment("Responsable del proyecto"));

            entity.Property(e => e.IdUsuario).HasComment("Identificador del usuario");
            entity.Property(e => e.UsApellido)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UsClave)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.UsCorreo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UsEstatus).HasComment("estatus(0:inactivo,1:activo)");
            entity.Property(e => e.UsFicha)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UsNombre)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("nombre del usuario");
            entity.Property(e => e.UsUsuario)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdEmpresa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Empresa");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
