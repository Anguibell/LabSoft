using LabSoft.Domain.Entities;
using LabSoft.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LabSoft.Infrastructure.Seeders;

public static class PanelSeeder
{
    public static async Task InitializeAsync(LabSoftDbContext context)
    {
        // Si ya hay paneles cargados, no hacemos nada
        if (await context.Paneles.AnyAsync())
        {
            return;
        }

        // Obtenemos los exámenes existentes en la BD para poder enlazarlos por su nombre
        var examenes = await context.Examenes.ToListAsync();
        if (!examenes.Any())
        {
            return; // Si no hay exámenes creados todavía, salimos para evitar errores
        }

        // Función auxiliar para buscar exámenes por nombre de forma segura
        Guid? GetExamenId(string nombre) => 
            examenes.FirstOrDefault(e => e.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase))?.Id;

        var panelesParaCrear = new List<Panel>();

        // 1. PERFIL LIPIDICO (Ejemplo basado en tus catálogos)
        var idsLipidico = new[] { 
            GetExamenId("COLESTEROL"), 
            GetExamenId("TRIGLICERIDOS"), 
            GetExamenId("HDL. COLESTEROL"), 
            GetExamenId("LDL COLESTEROL"), 
            GetExamenId("VLDL. COLESTEROL") 
        }.Where(id => id.HasValue).Select(id => id!.Value).ToList();

        if (idsLipidico.Any())
        {
            panelesParaCrear.Add(new Panel
            {
                Id = Guid.NewGuid(),
                Nombre = "PERFIL LIPIDICO",
                Activo = true,
                PanelExamenes = idsLipidico.Select(exId => new PanelExamen { ExamenId = exId }).ToList()
            });
        }

        // 2. PERFIL TIROIDEO
        var idsTiroideo = new[] { 
            GetExamenId("TSH"), 
            GetExamenId("T3 LIBRE"), 
            GetExamenId("T4 LIBRE") 
        }.Where(id => id.HasValue).Select(id => id!.Value).ToList();

        if (idsTiroideo.Any())
        {
            panelesParaCrear.Add(new Panel
            {
                Id = Guid.NewGuid(),
                Nombre = "PERFIL TIROIDEO",
                Activo = true,
                PanelExamenes = idsTiroideo.Select(exId => new PanelExamen { ExamenId = exId }).ToList()
            });
        }

        if (panelesParaCrear.Any())
        {
            await context.Paneles.AddRangeAsync(panelesParaCrear);
            await context.SaveChangesAsync();
        }
    }
}