using LabSoft.Domain.Entities;
using LabSoft.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LabSoft.Infrastructure.Seeders;

public static class PanelSeeder
{
    public static async Task InitializeAsync(LabSoftDbContext context)
    {
        // 1. Obtenemos todos los exámenes para sacar sus IDs
        var examenes = await context.Examenes.ToListAsync();
        if (!examenes.Any()) return;

        Guid? GetExamenId(string nombre) => 
            examenes.FirstOrDefault(e => e.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase))?.Id;

        // 2. Definimos la lista maestra de paneles y su composición
        var panelesDefinicion = new List<(int Codigo, string Nombre, string[] Examenes)>
        {
            (2001, "PERFIL 20", new[] { "HEMATOLOGIA COMPLETA", "GLICEMIA BASAL", "UREA", "CREATININA", "ACIDO URICO", "COLESTEROL", "TRIGLICERIDOS", "HDL. COLESTEROL", "LDL COLESTEROL", "VLDL. COLESTEROL", "AST/TGO (ASPARTATO AMINO TRANSFERASA)", "ALT/TGP (ALANINO AMINO TRANSFERASA)", "BILIRRUBINA TOTAL Y FRACCIONADA", "PROTEOGRAMA", "VELOCIDAD DE SEDIMENTACION GLOBULAR", "CALCIO" }),
            (2002, "PERFIL HORMONAL", new[] { "LH", "FSH", "ESTRADIOL", "PROGESTERONA", "PROLACTINA" }),
            (2003, "PERFIL TIROIDEO", new[] { "TSH", "T3 LIBRE", "T4 LIBRE" }),
            (2004, "PERFIL LIPIDICO", new[] { "COLESTEROL", "TRIGLICERIDOS", "HDL. COLESTEROL", "LDL COLESTEROL", "VLDL. COLESTEROL" }),
            (2005, "CHEQUEO GENERAL", new[] { "HEMATOLOGIA COMPLETA", "VELOCIDAD DE SEDIMENTACION GLOBULAR", "GLICEMIA BASAL", "UREA", "CREATININA", "ACIDO URICO", "COLESTEROL", "TRIGLICERIDOS", "HDL. COLESTEROL", "LDL COLESTEROL", "VLDL. COLESTEROL", "AST/TGO (ASPARTATO AMINO TRANSFERASA)", "ALT/TGP (ALANINO AMINO TRANSFERASA)", "BILIRRUBINA TOTAL Y FRACCIONADA", "PROTEOGRAMA", "CALCIO" }),
            (2006, "PERFIL PROSTATICO", new[] { "PSA TOTAL", "PSA LIBRE", "RELACION PSAL/PSAT" }),
            (2007, "PERFIL PREOPERATORIO", new[] { "HEMATOLOGIA COMPLETA", "TIEMPO DE PROTROMBINA (TP)", "TIEMPO PARCIAL DE TROMBOPLASTINA (TPT)", "GLICEMIA BASAL", "UREA", "CREATININA", "VDRL", "HIV" }),
            (2008, "PERFIL HEPATICO", new[] { "AST/TGO (ASPARTATO AMINO TRANSFERASA)", "ALT/TGP (ALANINO AMINO TRANSFERASA)", "BILIRRUBINA TOTAL Y FRACCIONADA", "PROTEOGRAMA", "GAMMA GLUTAMIL TRANSFERASA (GGT)", "FOSFATASA ALCALINA" }),
            (2009, "HIERRO SERICO+TIBC+%SATURACION", new[] { "HIERRO SERICO+UIBC+TIBC+% DE SATURACIÓN" }),
            (2010, "PERFIL HOMA", new[] { "GLICEMIA BASAL", "INSULINA BASAL", "INDICE HOMA" }),
            (2011, "MARCADORES TUMORALES", new[] { "ALFAFETOPROTEINAS (AFP)", "CEA", "CA - 125", "CA - 15.3", "CA - 19.9", "HE-4", "CYFRA 21.1" })
        };

        // 3. Lógica Upsert (Actualizar si existe, Crear si no existe)
        foreach (var def in panelesDefinicion)
        {
            var panelDb = await context.Paneles
                .Include(p => p.PanelExamenes)
                .FirstOrDefaultAsync(p => p.Codigo == def.Codigo);

            var idsParaAsociar = def.Examenes
                .Select(GetExamenId)
                .Where(id => id.HasValue)
                .Select(id => id!.Value)
                .ToList();

            if (panelDb != null)
            {
                // ACTUALIZAR PANEL EXISTENTE
                panelDb.Nombre = def.Nombre;
                
                // Borramos las relaciones viejas para evitar duplicados
                context.RemoveRange(panelDb.PanelExamenes);
                
                // Asignamos las nuevas relaciones calculadas
                panelDb.PanelExamenes = idsParaAsociar.Select(exId => new PanelExamen { ExamenId = exId, PanelId = panelDb.Id }).ToList();
                
                context.Paneles.Update(panelDb);
            }
            else
            {
                // CREAR NUEVO PANEL
                var nuevoPanel = new Panel
                {
                    Id = Guid.NewGuid(),
                    Codigo = def.Codigo,
                    Nombre = def.Nombre,
                    Activo = true,
                    PanelExamenes = idsParaAsociar.Select(exId => new PanelExamen { ExamenId = exId }).ToList()
                };
                await context.Paneles.AddAsync(nuevoPanel);
            }
        }

        // 4. Guardamos todo en una sola transacción
        await context.SaveChangesAsync();
    }
}