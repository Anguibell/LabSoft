using Microsoft.EntityFrameworkCore;
using LabSoft.Domain.Entities;

namespace LabSoft.Infrastructure.Persistence;

public class LabSoftDbContext : DbContext
{
    public LabSoftDbContext(DbContextOptions<LabSoftDbContext> options) : base(options) {}
    public DbSet<Paciente> Pacientes { get; set; } = null!;
    public DbSet<Especialidad> Especialidades { get; set; }
    public DbSet<Examen> Examenes { get; set; }
    public DbSet<Panel> Paneles { get; set; }
    public DbSet<PanelExamen> PanelExamenes { get; set; }
    public DbSet<Orden> Ordenes { get; set; }
    public DbSet<OrdenDetalle> OrdenDetalles { get; set; }
    public DbSet<Resultado> Resultados { get; set; } = null!;
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de Pacientes (Índice único para la cédula)
        modelBuilder.Entity<Paciente>()
            .HasIndex(p => p.Cedula)
            .IsUnique();

        modelBuilder.Entity<Paciente>()
            .Property(p => p.Sexo)
            .HasConversion<string>(); // Guarda el Enum como texto en la BD
        
        // Configuración de la relación de Autoreferencia (Titular -> Beneficiarios)
        modelBuilder.Entity<Paciente>()
            .HasOne(p => p.Titular)
            .WithMany(p => p.Beneficiarios)
            .HasForeignKey(p => p.TitularId)
            .OnDelete(DeleteBehavior.Restrict); // Evita borrados en cascada accidentales

        // Configuración de Exámenes y su relación con Especialidades
        modelBuilder.Entity<Examen>()
            .HasOne(e => e.Especialidad)
            .WithMany(esp => esp.Examenes)
            .HasForeignKey(e => e.EspecialidadId);

        modelBuilder.Entity<Examen>()
            .Property(e => e.Codigo)
            .IsRequired();

        //Asegurar que el código sea único en la BD
        // Asegurar que el código sea único SOLO para exámenes principales (Codigo > 0)
        modelBuilder.Entity<Examen>()
            .HasIndex(e => e.Codigo)
            .IsUnique()
            .HasFilter("\"Codigo\" > 0");

        // Configurar la llave compuesta para la tabla intermedia de paneles
        modelBuilder.Entity<PanelExamen>()
            .HasKey(pe => new { pe.PanelId, pe.ExamenId });

        modelBuilder.Entity<PanelExamen>()
            .HasOne(pe => pe.Panel)
            .WithMany(p => p.PanelExamenes)
            .HasForeignKey(pe => pe.PanelId);

        modelBuilder.Entity<PanelExamen>()
            .HasOne(pe => pe.Examen)
            .WithMany()
            .HasForeignKey(pe => pe.ExamenId);

        // Configuración de Ordenes y su relación con Paciente 
        modelBuilder.Entity<Orden>()
            .HasOne(o => o.Paciente)
            .WithMany()
            .HasForeignKey(o => o.PacienteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Orden>()
            .Property(o => o.NumeroOrden)
            .ValueGeneratedOnAdd()
            .UseIdentityByDefaultColumn(); // Comando específico de Npgsql para crear la secuencia en PostgreSQL

        modelBuilder.Entity<OrdenDetalle>()
            .HasOne(od => od.Orden)
            .WithMany(o => o.Detalles)
            .HasForeignKey(od => od.OrdenId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuración de Resultado
        modelBuilder.Entity<Resultado>(entity =>
        {
            entity.HasKey(r => r.Id);

            // Configura la relación 1 a muchos con OrdenDetalle
            entity.HasOne(r => r.OrdenDetalle)
                .WithMany(od => od.Resultados)
                .HasForeignKey(r => r.OrdenDetalleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuración de Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(u => u.Id);
            
            entity.Property(u => u.NombreUsuario)
                .IsRequired()
                .HasMaxLength(50);
                
            entity.HasIndex(u => u.NombreUsuario)
                .IsUnique();

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.HasIndex(u => u.Email)
                .IsUnique();

            entity.Property(u => u.PasswordHash)
                .IsRequired();

            entity.Property(u => u.Rol)
                .IsRequired()
                .HasMaxLength(30);

            // Para mayor robustez
            entity.Property(u => u.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

    }

}