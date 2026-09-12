using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LabSoft.Domain.Entities;
using LabSoft.Infrastructure.Persistence;

namespace LabSoft.Infrastructure.Seeders;

public static class DbInitializer
{
    public static async Task InitializeAsync(LabSoftDbContext context)
    {
        // Asegurar que la base de datos esté creada
        await context.Database.EnsureCreatedAsync();

        // ==========================================
        // 1. CREACIÓN DE LAS 10 ESPECIALIDADES (1 - 10)
        // ==========================================

        var espHematologia = await ObtenerOCrearEspecialidad(context, 1, "HEMATOLOGIA");
        var espCoagulacion = await ObtenerOCrearEspecialidad(context, 2, "COAGULACION");
        var espBioquimica = await ObtenerOCrearEspecialidad(context, 3, "BIOQUIMICA");
        var espInmunologia = await ObtenerOCrearEspecialidad(context, 4, "INMUNOLOGIA / SEROLOGIA");
        var espHormonas = await ObtenerOCrearEspecialidad(context, 5, "HORMONAS");
        var espMarcadores = await ObtenerOCrearEspecialidad(context, 6, "MARCADORES TUMORALES");
        var espUroanalisis = await ObtenerOCrearEspecialidad(context, 7, "UROANALISIS");
        var espCoprologia = await ObtenerOCrearEspecialidad(context, 8, "COPROLOGIA");
        var espBacteriologia = await ObtenerOCrearEspecialidad(context, 9, "BACTERIOLOGIA");
        var espEspeciales = await ObtenerOCrearEspecialidad(context, 10, "ESPECIALES");

        await context.SaveChangesAsync();

        // ==========================================
        // 2. CREACIÓN DE EXÁMENES (CON CÓDIGOS, JERARQUÍAS Y ORDEN)
        // ==========================================

        // --- BLOQUE 100 (HEMATOLOGIA: 101 - 199) ---
        var hematologiaCompleta = new Examen
        {
            Id = Guid.NewGuid(),
            Codigo = 101,
            Nombre = "HEMATOLOGIA COMPLETA",
            Unidad = "",
            ValoresReferencia = "",
            Precio = 0.00m,
            Activo = true,
            EsOpcionalDinamico = false,
            Orden = 1,
            EspecialidadId = espHematologia.Id,
            SubExamenes = new List<Examen>
            {
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "HEMOGLOBINA", Unidad = "g/dL", ValoresReferencia = "", ValoresReferenciaM = "13.0 - 17.0", ValoresReferenciaF = "12.0 - 15.0", Precio = 0.00m, Activo = true, Orden = 1, DecimalesRequeridos = 1, EspecialidadId = espHematologia.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "HEMATOCRITO", Unidad = "%", ValoresReferencia = "36 - 48", Precio = 0.00m, Activo = true, Orden = 2, DecimalesRequeridos = 0, EspecialidadId = espHematologia.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CUENTA BLANCA", Unidad = "mm3", ValoresReferencia = "5000 - 10000", Precio = 0.00m, Activo = true, Orden = 3, DecimalesRequeridos = 0, EspecialidadId = espHematologia.Id },
                new Examen
                {
                    Id = Guid.NewGuid(),
                    Codigo = 0,
                    Nombre = "FORMULA LEUCOCITARIA",
                    Unidad = "",
                    ValoresReferencia = "",
                    Precio = 0.00m,
                    Activo = true,
                    Orden = 4,
                    EspecialidadId = espHematologia.Id,
                    SubExamenes = new List<Examen>
                    {
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "NEUTROFILOS", Unidad = "%", ValoresReferencia = "50 - 70", Precio = 0.00m, Activo = true, Orden = 1, DecimalesRequeridos = 0, EspecialidadId = espHematologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "LINFOCITOS", Unidad = "%", ValoresReferencia = "20 - 40", Precio = 0.00m, Activo = true, Orden = 2, DecimalesRequeridos = 0, EspecialidadId = espHematologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "EOSINOFILOS", Unidad = "%", ValoresReferencia = "1 - 4", Precio = 0.00m, Activo = true, Orden = 3, DecimalesRequeridos = 0, EspecialidadId = espHematologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "MONOCITOS", Unidad = "%", ValoresReferencia = "2 - 8", Precio = 0.00m, Activo = true, Orden = 4, DecimalesRequeridos = 0, EspecialidadId = espHematologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "BASOFILOS", Unidad = "%", ValoresReferencia = "0 - 1", Precio = 0.00m, Activo = true, Orden = 5, DecimalesRequeridos = 0, EspecialidadId = espHematologia.Id }
                    }
                },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "PLAQUETAS", Unidad = "xmm3", ValoresReferencia = "150000 - 450000", Precio = 0.00m, Activo = true, Orden = 5, DecimalesRequeridos = 0, EspecialidadId = espHematologia.Id }
            }
        };

        var grupoSanguineoRh = new Examen
        {
            Id = Guid.NewGuid(),
            Codigo = 103,
            Nombre = "GRUPO SANGUINEO Y FACTOR Rh",
            Unidad = "",
            ValoresReferencia = "",
            Precio = 0.00m,
            Activo = true,
            EsOpcionalDinamico = false,
            EspecialidadId = espHematologia.Id,
            SubExamenes = new List<Examen>
            {
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "GRUPO SANGUINEO", 
                    Unidad = "", 
                    ValoresReferencia = "", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 1, 
                    EspecialidadId = espHematologia.Id 
                },
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "FACTOR RH", 
                    Unidad = "", 
                    ValoresReferencia = "", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 2, 
                    EspecialidadId = espHematologia.Id 
                }
            }
        };

        var listaHematologia = new List<Examen> 
        { 
            hematologiaCompleta,
            new Examen { Id = Guid.NewGuid(), Codigo = 102, Nombre = "VELOCIDAD DE SEDIMENTACION GLOBULAR", Unidad = "mm/h", ValoresReferencia = "0 - 20", Precio = 0.00m, Activo = true, Orden = 2, DecimalesRequeridos = 0, EspecialidadId = espHematologia.Id },
            grupoSanguineoRh 
        };
        await SincronizarExamenesRecursivo(context, listaHematologia, espHematologia.Id);

        // --- BLOQUE 200 (COAGULACION: 201 - 299) ---

        var tiempoProtrombina = new Examen
        {
            Id = Guid.NewGuid(),
            Codigo = 201,
            Nombre = "TIEMPO DE PROTROMBINA (TP)",
            Unidad = "",
            ValoresReferencia = "",
            Precio = 0.00m,
            Activo = true,
            EsOpcionalDinamico = false,
            EspecialidadId = espCoagulacion.Id,
            SubExamenes = new List<Examen>
            {
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "PACIENTE", 
                    Unidad = "seg.", 
                    ValoresReferencia = "11 - 13", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 1, 
                    DecimalesRequeridos = 0,
                    EspecialidadId = espCoagulacion.Id 
                },
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "CONTROL", 
                    Unidad = "seg.", 
                    ValoresReferencia = "11 - 13", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 2, 
                    DecimalesRequeridos = 0,
                    EspecialidadId = espCoagulacion.Id 
                },
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "I.N.R.", 
                    Unidad = "", 
                    ValoresReferencia = "0.8 - 1.2", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 3, 
                    DecimalesRequeridos = 1,
                    EspecialidadId = espCoagulacion.Id 
                },
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "I.N.R TERAPIA CON ANTICOAGULANTE", 
                    Unidad = "", 
                    ValoresReferencia = "2.0 - 3.0", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 4, 
                    DecimalesRequeridos = 1,
                    EspecialidadId = espCoagulacion.Id 
                },
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "ACT. PROTOMBINICA", 
                    Unidad = "%", 
                    ValoresReferencia = "70 - 100", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 5, 
                    DecimalesRequeridos = 0,
                    EspecialidadId = espCoagulacion.Id 
                }
            }
        };

        var tiempoTromboplastina = new Examen
        {
            Id = Guid.NewGuid(),
            Codigo = 202,
            Nombre = "TIEMPO PARCIAL DE TROMBOPLASTINA (TPT)",
            Unidad = "",
            ValoresReferencia = "",
            Precio = 0.00m,
            Activo = true,
            EsOpcionalDinamico = false,
            EspecialidadId = espCoagulacion.Id,
            SubExamenes = new List<Examen>
            {
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "PACIENTE", 
                    Unidad = "seg.", 
                    ValoresReferencia = "25 - 35", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 1, 
                    DecimalesRequeridos = 0,
                    EspecialidadId = espCoagulacion.Id 
                },
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "CONTROL", 
                    Unidad = "seg.", 
                    ValoresReferencia = "25 - 35", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 2, 
                    DecimalesRequeridos = 0,
                    EspecialidadId = espCoagulacion.Id 
                }
            }
        };

        var listaCoagulacion = new List<Examen> 
        { 
            tiempoProtrombina,
            tiempoTromboplastina,
            new Examen { Id = Guid.NewGuid(), Codigo = 203, Nombre = "FIBRINOGENO", Unidad = "mg/dL", ValoresReferencia = "200 - 400", Precio = 0.00m, Activo = true, Orden = 3, DecimalesRequeridos = 0, EspecialidadId = espCoagulacion.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 204, Nombre = "DIMERO D", Unidad = "ng/mL", ValoresReferencia = "< 500", Precio = 0.00m, Activo = true, Orden = 4, DecimalesRequeridos = 0, EspecialidadId = espCoagulacion.Id }
        };
        await SincronizarExamenesRecursivo(context, listaCoagulacion, espCoagulacion.Id);

        // --- BLOQUE 300 (BIOQUIMICA: 301 - 399) ---

        var bilirrubinaTotalFraccionada = new Examen
        {
            Id = Guid.NewGuid(),
            Codigo = 316,
            Nombre = "BILIRRUBINA TOTAL Y FRACCIONADA",
            Unidad = "",
            ValoresReferencia = "",
            Precio = 0.00m,
            Activo = true,
            EsOpcionalDinamico = false,
            EspecialidadId = espBioquimica.Id,
            SubExamenes = new List<Examen>
            {
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "BILIRRUBINA TOTAL", 
                    Unidad = "mg/dL", 
                    ValoresReferencia = "0.2 - 1.2", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 1,
                    DecimalesRequeridos = 1, 
                    EspecialidadId = espBioquimica.Id 
                },
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "BILIRRUBINA DIRECTA", 
                    Unidad = "mg/dL", 
                    ValoresReferencia = "0.0 - 0.3", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 2, 
                    DecimalesRequeridos = 1,
                    EspecialidadId = espBioquimica.Id 
                },
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "BILIRRUBINA INDIRECTA", 
                    Unidad = "mg/dL", 
                    ValoresReferencia = "0.2 - 0.9", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 3, 
                    DecimalesRequeridos = 1,
                    EspecialidadId = espBioquimica.Id 
                }
            }
        };

        var proteograma = new Examen
        {
            Id = Guid.NewGuid(),
            Codigo = 317,
            Nombre = "PROTEOGRAMA",
            Unidad = "",
            ValoresReferencia = "",
            Precio = 0.00m,
            Activo = true,
            EsOpcionalDinamico = false,
            EspecialidadId = espBioquimica.Id,
            SubExamenes = new List<Examen>
            {
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "PROTEINAS TOTALES", 
                    Unidad = "gr/dl", 
                    ValoresReferencia = "6.0 - 8.3", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 1, 
                    DecimalesRequeridos = 1,
                    EspecialidadId = espBioquimica.Id 
                },
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "ALBUMINA", 
                    Unidad = "gr/dl", 
                    ValoresReferencia = "3.5 - 5.0", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 2, 
                    DecimalesRequeridos = 1,
                    EspecialidadId = espBioquimica.Id 
                },
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "GLOBULINA", 
                    Unidad = "gr/dl", 
                    ValoresReferencia = "2.3 - 3.4", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 3, 
                    DecimalesRequeridos = 1,
                    EspecialidadId = espBioquimica.Id 
                },
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "RELACION A/G", 
                    Unidad = "", 
                    ValoresReferencia = "1.2 - 2.2", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 4, 
                    DecimalesRequeridos = 1,
                    EspecialidadId = espBioquimica.Id 
                }
            }
        };

        var electrolitos = new Examen
        {
            Id = Guid.NewGuid(),
            Codigo = 325,
            Nombre = "ELECTROLITOS (Na,K,Cl)",
            Unidad = "",
            ValoresReferencia = "",
            Precio = 0.00m,
            Activo = true,
            EsOpcionalDinamico = false,
            EspecialidadId = espBioquimica.Id,
            SubExamenes = new List<Examen>
            {
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "SODIO", 
                    Unidad = "meq/L", 
                    ValoresReferencia = "135 - 155", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 1, 
                    DecimalesRequeridos = 0,
                    EspecialidadId = espBioquimica.Id 
                },
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "POTASIO", 
                    Unidad = "meq/L", 
                    ValoresReferencia = "3.5 - 5.5", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 2, 
                    DecimalesRequeridos = 1,
                    EspecialidadId = espBioquimica.Id 
                },
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "CLORO", 
                    Unidad = "meq/L", 
                    ValoresReferencia = "95 - 110", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 3, 
                    DecimalesRequeridos = 0,
                    EspecialidadId = espBioquimica.Id 
                },
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "CALCIO", 
                    Unidad = "meq/L", 
                    ValoresReferencia = "8.5 - 10.6", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 4, 
                    DecimalesRequeridos = 1,
                    EspecialidadId = espBioquimica.Id 
                }
            }
        };

        var perfilHierro = new Examen
        {
            Id = Guid.NewGuid(),
            Codigo = 327,
            Nombre = "HIERRO SERICO+UIBC+TIBC+% DE SATURACIÓN",
            Unidad = "",
            ValoresReferencia = "",
            Precio = 0.00m,
            Activo = true,
            EsOpcionalDinamico = false,
            EspecialidadId = espBioquimica.Id,
            SubExamenes = new List<Examen>
            {
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "HIERRO SERICO", 
                    Unidad = "ug/dl", 
                    ValoresReferencia = "60 - 170", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 1, 
                    EspecialidadId = espBioquimica.Id 
                },
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "UIBC", 
                    Unidad = "ug/dl", 
                    ValoresReferencia = "110 - 370", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 2, 
                    EspecialidadId = espBioquimica.Id 
                },
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "TIBC", 
                    Unidad = "ug/dl", 
                    ValoresReferencia = "240 - 450", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 3, 
                    EspecialidadId = espBioquimica.Id 
                },
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "% DE SATURACION DE TRANSFERRINA", 
                    Unidad = "%", 
                    ValoresReferencia = "20 - 50", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 4, 
                    DecimalesRequeridos = 0,
                    EspecialidadId = espBioquimica.Id 
                }
            }
        };

        var glicemia1H = new Examen
        {
            Id = Guid.NewGuid(),
            Codigo = 333,
            Nombre = "GLICEMIA POSTCARGA (75G) 1 HORA",
            Unidad = "",
            ValoresReferencia = "",
            Precio = 0.00m,
            Activo = true,
            EsOpcionalDinamico = false,
            EspecialidadId = espBioquimica.Id,
            SubExamenes = new List<Examen>
            {
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "GLICEMIA BASAL", Unidad = "mg/dL", ValoresReferencia = "70 - 100", Precio = 0.00m, Activo = true, Orden = 1, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "GLICEMIA POSTCARGA 1 HORA", Unidad = "mg/dL", ValoresReferencia = "< 180", Precio = 0.00m, Activo = true, Orden = 2, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id }
            }
        };

        var glicemia2H = new Examen
        {
            Id = Guid.NewGuid(),
            Codigo = 334,
            Nombre = "GLICEMIA POSTCARGA (75G) 2 HORAS",
            Unidad = "",
            ValoresReferencia = "",
            Precio = 0.00m,
            Activo = true,
            EsOpcionalDinamico = false,
            EspecialidadId = espBioquimica.Id,
            SubExamenes = new List<Examen>
            {
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "GLICEMIA BASAL", Unidad = "mg/dL", ValoresReferencia = "70 - 100", Precio = 0.00m, Activo = true, Orden = 1, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "GLICEMIA POSTCARGA 1 HORA", Unidad = "mg/dL", ValoresReferencia = "< 180", Precio = 0.00m, Activo = true, Orden = 2, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "GLICEMIA POSTCARGA 2 HORAS", Unidad = "mg/dL", ValoresReferencia = "< 140", Precio = 0.00m, Activo = true, Orden = 3, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id }
            }
        };

        var glicemia3H = new Examen
        {
            Id = Guid.NewGuid(),
            Codigo = 335,
            Nombre = "GLICEMIA POSTCARGA (75G) 3 HORAS",
            Unidad = "",
            ValoresReferencia = "",
            Precio = 0.00m,
            Activo = true,
            EsOpcionalDinamico = false,
            EspecialidadId = espBioquimica.Id,
            SubExamenes = new List<Examen>
            {
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "GLICEMIA BASAL", Unidad = "mg/dL", ValoresReferencia = "70 - 100", Precio = 0.00m, Activo = true, Orden = 1, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "GLICEMIA POSTCARGA 1 HORA", Unidad = "mg/dL", ValoresReferencia = "< 180", Precio = 0.00m, Activo = true, Orden = 2, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "GLICEMIA POSTCARGA 2 HORAS", Unidad = "mg/dL", ValoresReferencia = "< 140", Precio = 0.00m, Activo = true, Orden = 3, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "GLICEMIA POSTCARGA 3 HORAS", Unidad = "mg/dL", ValoresReferencia = "70 - 115", Precio = 0.00m, Activo = true, Orden = 4, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id }
            }
        };

        var glicemia4H = new Examen
        {
            Id = Guid.NewGuid(),
            Codigo = 336,
            Nombre = "GLICEMIA POSTCARGA (75G) 4 HORAS",
            Unidad = "",
            ValoresReferencia = "",
            Precio = 0.00m,
            Activo = true,
            EsOpcionalDinamico = false,
            EspecialidadId = espBioquimica.Id,
            SubExamenes = new List<Examen>
            {
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "GLICEMIA BASAL", Unidad = "mg/dL", ValoresReferencia = "70 - 100", Precio = 0.00m, Activo = true, Orden = 1, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "GLICEMIA POSTCARGA 1 HORA", Unidad = "mg/dL", ValoresReferencia = "< 180", Precio = 0.00m, Activo = true, Orden = 2, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "GLICEMIA POSTCARGA 2 HORAS", Unidad = "mg/dL", ValoresReferencia = "< 140", Precio = 0.00m, Activo = true, Orden = 3, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "GLICEMIA POSTCARGA 3 HORAS", Unidad = "mg/dL", ValoresReferencia = "70 - 115", Precio = 0.00m, Activo = true, Orden = 4, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "GLICEMIA POSTCARGA 4 HORAS", Unidad = "mg/dL", ValoresReferencia = "70 - 100", Precio = 0.00m, Activo = true, Orden = 5, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id }
            }
        };

        var listaBioquimica = new List<Examen> 
        {
            new Examen { Id = Guid.NewGuid(), Codigo = 301, Nombre = "GLICEMIA BASAL", Unidad = "mg/dL", ValoresReferencia = "70 - 110", Precio = 0.00m, Activo = true, Orden = 1, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 302, Nombre = "GLICEMIA POST-PRANDIAL 30 MINUTOS", Unidad = "mg/dL", ValoresReferencia = "", Precio = 0.00m, Activo = true, Orden = 2, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 303, Nombre = "GLICEMIA POST- PRANDIAL 1 HORA", Unidad = "mg/dL", ValoresReferencia = "", Precio = 0.00m, Activo = true, Orden = 3, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 304, Nombre = "GLICEMIA POST-PRANDIAL 2 HORAS", Unidad = "mg/dL", ValoresReferencia = "65 - 140", Precio = 0.00m, Activo = true, Orden = 4, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 305, Nombre = "UREA", Unidad = "mg/dL", ValoresReferencia = "10 - 50", Precio = 0.00m, Activo = true, Orden = 5, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 306, Nombre = "BUN (NITROGENO UREICO)", Unidad = "mg/dL", ValoresReferencia = "", Precio = 0.00m, Activo = true, Orden = 6, EspecialidadId = espBioquimica.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 307, Nombre = "CREATININA", Unidad = "mg/dL", ValoresReferencia = "0.6 - 1.2", Precio = 0.00m, Activo = true, Orden = 7, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 308, Nombre = "ACIDO URICO", Unidad = "mg/dL", ValoresReferencia = "", Precio = 0.00m, Activo = true, Orden = 8, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 309, Nombre = "COLESTEROL", Unidad = "mg/dL", ValoresReferencia = "< 200", Precio = 0.00m, Activo = true, Orden = 9, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 310, Nombre = "TRIGLICERIDOS", Unidad = "mg/dL", ValoresReferencia = "< 150", Precio = 0.00m, Activo = true, Orden = 10, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 311, Nombre = "HDL. COLESTEROL", Unidad = "mg/dL", ValoresReferencia = "35 - 55", Precio = 0.00m, Activo = true, Orden = 11, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 312, Nombre = "LDL COLESTEROL", Unidad = "mg/dL", ValoresReferencia = "< 120", Precio = 0.00m, Activo = true, Orden = 12, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 313, Nombre = "VLDL. COLESTEROL", Unidad = "mg/dL", ValoresReferencia = "", Precio = 0.00m, Activo = true, Orden = 13, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 314, Nombre = "AST/TGO (ASPARTATO AMINO TRANSFERASA)", Unidad = "U/L", ValoresReferencia = "< 40", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 315, Nombre = "ALT/TGP (ALANINO AMINO TRANSFERASA)", Unidad = "U/L", ValoresReferencia = "< 40", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id },
            bilirrubinaTotalFraccionada,
            proteograma,
            new Examen { Id = Guid.NewGuid(), Codigo = 318, Nombre = "FOSFATASA ALCALINA", Unidad = "U/L", ValoresReferencia = "40 - 129", Precio = 0.00m, Activo = true, EspecialidadId = espBioquimica.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 319, Nombre = "GAMMA GLUTAMIL TRANSFERASA (GGT)", Unidad = "U/L", ValoresReferencia = "8 - 61", Precio = 0.00m, Activo = true, EspecialidadId = espBioquimica.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 320, Nombre = "DESHIDROGENASA LACTICA (LDH)", Unidad = "U/L", ValoresReferencia = "125 - 243", Precio = 0.00m, Activo = true, EspecialidadId = espBioquimica.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 321, Nombre = "CALCIO", Unidad = "mg/dl", ValoresReferencia = "8.5 - 10.6", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 322, Nombre = "PROTEINAS TOTALES", Unidad = "gr/dl", ValoresReferencia = "6.0 - 8.3", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 323, Nombre = "FOSFORO", Unidad = "mg/dl", ValoresReferencia = "2.5 - 4.5", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 324, Nombre = "MAGNESIO", Unidad = "mg/dl", ValoresReferencia = "1.7 - 2.2", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espBioquimica.Id },
            electrolitos,
            new Examen { Id = Guid.NewGuid(), Codigo = 326, Nombre = "HIERRO SERICO", Unidad = "ug/dl", ValoresReferencia = "60 - 170", Precio = 0.00m, Activo = true, EspecialidadId = espBioquimica.Id },
            perfilHierro,
            new Examen { Id = Guid.NewGuid(), Codigo = 328, Nombre = "FERRITINA", Unidad = "ng/ml", ValoresReferencia = "30 - 400", Precio = 0.00m, Activo = true, EspecialidadId = espBioquimica.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 329, Nombre = "AMILASA", Unidad = "U/L", ValoresReferencia = "28 - 100", Precio = 0.00m, Activo = true, EspecialidadId = espBioquimica.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 330, Nombre = "LIPASA", Unidad = "U/L", ValoresReferencia = "13 - 60", Precio = 0.00m, Activo = true, EspecialidadId = espBioquimica.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 331, Nombre = "CPK TOTAL (CREATIN KINASA)", Unidad = "UI/L", ValoresReferencia = "24 - 195", Precio = 0.00m, Activo = true, EspecialidadId = espBioquimica.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 332, Nombre = "HEMOGLOBINA GLICADA A1C", Unidad = "%", ValoresReferencia = "4.0 - 5.6", Precio = 0.00m, Activo = true, EspecialidadId = espBioquimica.Id },
            glicemia1H,
            glicemia2H,
            glicemia3H,
            glicemia4H,
            new Examen { Id = Guid.NewGuid(), Codigo = 337, Nombre = "TRANSFERRINA", Unidad = "mg/dL", ValoresReferencia = "200 - 360", Precio = 0.00m, Activo = true, EspecialidadId = espBioquimica.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 338, Nombre = "PEPTIDO NATRIURETICO TIPO B (NT-proBNP)", Unidad = "pg/ml", ValoresReferencia = "< 125", Precio = 0.00m, Activo = true, EspecialidadId = espBioquimica.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 339, Nombre = "TROPONINA I (CUALITATIVA)", Unidad = "", ValoresReferencia = "Negativo", Precio = 0.00m, Activo = true, EspecialidadId = espBioquimica.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 340, Nombre = "TROPONINA I (CUANTITATIVA)", Unidad = "ng/ml", ValoresReferencia = "0.00 - 0.04", Precio = 0.00m, Activo = true, EspecialidadId = espBioquimica.Id }
        };
        await SincronizarExamenesRecursivo(context, listaBioquimica, espBioquimica.Id);

        // --- BLOQUE 400 (INMUNOLOGIA / SEROLOGIA: 401 - 499) ---

        var perfilTorch = new Examen
        {
            Id = Guid.NewGuid(),
            Codigo = 427,
            Nombre = "PERFIL TORCH",
            Unidad = "",
            ValoresReferencia = "",
            Precio = 0.00m,
            Activo = true,
            EsOpcionalDinamico = false,
            EspecialidadId = espInmunologia.Id,
            SubExamenes = new List<Examen>
            {
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "CITOMEGALOVIRUS IgM", 
                    Unidad = "Índice", 
                    ValoresReferencia = "< 0.8 (No Reactivo)", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 1, 
                    EspecialidadId = espInmunologia.Id 
                },
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "CITOMEGALOVIRUS IgG", 
                    Unidad = "U/mL", 
                    ValoresReferencia = "< 6.0 (No Reactivo)", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 2, 
                    EspecialidadId = espInmunologia.Id 
                },
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "TOXOPLASMA IgM", 
                    Unidad = "Índice", 
                    ValoresReferencia = "< 0.8 (No Reactivo)", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 3, 
                    EspecialidadId = espInmunologia.Id 
                },
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "TOXOPLASMA IgG", 
                    Unidad = "UI/mL", 
                    ValoresReferencia = "< 1.0 (No Reactivo)", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 4, 
                    EspecialidadId = espInmunologia.Id 
                },
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "RUBEOLA IgM", 
                    Unidad = "Índice", 
                    ValoresReferencia = "< 0.8 (No Reactivo)", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 5, 
                    EspecialidadId = espInmunologia.Id 
                },
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "RUBEOLA IgG", 
                    Unidad = "IU/mL", 
                    ValoresReferencia = "< 10.0 (No Reactivo)", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 6, 
                    EspecialidadId = espInmunologia.Id 
                },
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "HERPES SIMPLE 2 IgM", 
                    Unidad = "Índice", 
                    ValoresReferencia = "< 0.9 (No Reactivo)", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 7, 
                    EspecialidadId = espInmunologia.Id 
                },
                new Examen 
                { 
                    Id = Guid.NewGuid(), 
                    Codigo = 0, 
                    Nombre = "HERPES SIMPLE 2 IgG", 
                    Unidad = "Índice", 
                    ValoresReferencia = "< 0.9 (No Reactivo)", 
                    Precio = 0.00m, 
                    Activo = true, 
                    Orden = 8, 
                    EspecialidadId = espInmunologia.Id 
                }
            }
        };

        var listaInmunologia = new List<Examen> 
        {
            new Examen { Id = Guid.NewGuid(), Codigo = 401, Nombre = "VDRL", Unidad = "", ValoresReferencia = "No Reactivo", Precio = 0.00m, Activo = true, EspecialidadId = espInmunologia.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 402, Nombre = "HIV 1 Y 2", Unidad = "", ValoresReferencia = "No Reactivo", Precio = 0.00m, Activo = true, EspecialidadId = espInmunologia.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 403, Nombre = "PROTEINA C REACTIVA CUANTITATIVA", Unidad = "mg/L", ValoresReferencia = "< 6.0", Precio = 0.00m, Activo = true, EspecialidadId = espInmunologia.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 404, Nombre = "FACTOR REUMATOIDEO", Unidad = "UI/mL", ValoresReferencia = "< 14.0", Precio = 0.00m, Activo = true, EspecialidadId = espInmunologia.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 405, Nombre = "ANTIESTREPTOLISINA O", Unidad = "UI/mL", ValoresReferencia = "< 200", Precio = 0.00m, Activo = true, EspecialidadId = espInmunologia.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 406, Nombre = "ANTIGENO SARS COV-2", Unidad = "", ValoresReferencia = "Negativo", Precio = 0.00m, Activo = true, EspecialidadId = espInmunologia.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 407, Nombre = "HEPATITIS \"A\" IgM", Unidad = "", ValoresReferencia = "No Reactivo", Precio = 0.00m, Activo = true, EspecialidadId = espInmunologia.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 408, Nombre = "HEPATITIS \"A\" IgG", Unidad = "", ValoresReferencia = "No Reactivo", Precio = 0.00m, Activo = true, EspecialidadId = espInmunologia.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 409, Nombre = "HEPATITIS \"B\" AG DE SUPERFICIE (HBsAg)", Unidad = "", ValoresReferencia = "No Reactivo", Precio = 0.00m, Activo = true, EspecialidadId = espInmunologia.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 410, Nombre = "HEPATITIS \"B\" ANTICORE (Anti-HBc)", Unidad = "", ValoresReferencia = "No Reactivo", Precio = 0.00m, Activo = true, EspecialidadId = espInmunologia.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 411, Nombre = "HEPATITIS \"C\"", Unidad = "", ValoresReferencia = "No Reactivo", Precio = 0.00m, Activo = true, EspecialidadId = espInmunologia.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 412, Nombre = "TOXOPLASMA IgG", Unidad = "UI/mL", ValoresReferencia = "< 1.0 (No Reactivo)", Precio = 0.00m, Activo = true, EspecialidadId = espInmunologia.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 413, Nombre = "TOXOPLASMA IgM", Unidad = "Índice", ValoresReferencia = "< 0.8 (No Reactivo)", Precio = 0.00m, Activo = true, EspecialidadId = espInmunologia.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 414, Nombre = "CITOMEGALOVIRUS IgM", Unidad = "Índice", ValoresReferencia = "< 0.8 (No Reactivo)", Precio = 0.00m, Activo = true, EspecialidadId = espInmunologia.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 415, Nombre = "CITOMEGALOVIRUS IgG", Unidad = "U/mL", ValoresReferencia = "< 6.0 (No Reactivo)", Precio = 0.00m, Activo = true, EspecialidadId = espInmunologia.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 416, Nombre = "INMUNOGLOBULINA SERICA G (IgG)", Unidad = "mg/dl", ValoresReferencia = "700 - 1600", Precio = 0.00m, Activo = true, EspecialidadId = espInmunologia.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 417, Nombre = "INMUNOGLOBULINA SERICA M (IgM)", Unidad = "mg/dl", ValoresReferencia = "40 - 230", Precio = 0.00m, Activo = true, EspecialidadId = espInmunologia.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 418, Nombre = "INMUNOGLOBULINA SERICA A (IgA)", Unidad = "mg/dl", ValoresReferencia = "70 - 400", Precio = 0.00m, Activo = true, EspecialidadId = espInmunologia.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 419, Nombre = "INMUNOGLOBULINA SERICA E (IgE)", Unidad = "UI/ml", ValoresReferencia = "< 100", Precio = 0.00m, Activo = true, EspecialidadId = espInmunologia.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 420, Nombre = "HELICOBACTER PYLORI IgM", Unidad = "U/mL", ValoresReferencia = "< 30 (Negativo)", Precio = 0.00m, Activo = true, EspecialidadId = espInmunologia.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 421, Nombre = "HELICOBACTER PYLORI IgG", Unidad = "U/mL", ValoresReferencia = "< 1.8 (Negativo)", Precio = 0.00m, Activo = true, EspecialidadId = espInmunologia.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 422, Nombre = "ANTICUERPOS ANTI DNA", Unidad = "U/mL", ValoresReferencia = "< 25 (Negativo)", Precio = 0.00m, Activo = true, EspecialidadId = espInmunologia.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 423, Nombre = "ANTICUERPOS ANTINUCLEARES (ANA)", Unidad = "", ValoresReferencia = "Negativo", Precio = 0.00m, Activo = true, EspecialidadId = espInmunologia.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 424, Nombre = "DENGUE IgG", Unidad = "", ValoresReferencia = "Negativo", Precio = 0.00m, Activo = true, EspecialidadId = espInmunologia.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 425, Nombre = "DENGUE IgM", Unidad = "", ValoresReferencia = "Negativo", Precio = 0.00m, Activo = true, EspecialidadId = espInmunologia.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 426, Nombre = "DENGUE NS1", Unidad = "", ValoresReferencia = "Negativo", Precio = 0.00m, Activo = true, EspecialidadId = espInmunologia.Id },
            perfilTorch
        };
        await SincronizarExamenesRecursivo(context, listaInmunologia, espInmunologia.Id);

        // --- BLOQUE 500 (HORMONAS: 501 - 599) ---

        var insulinaCarga75g2H = new Examen
        {
            Id = Guid.NewGuid(),
            Codigo = 528,
            Nombre = "INSULINA POST-CARGA (75G) 2 HORAS",
            Unidad = "",
            ValoresReferencia = "",
            Precio = 0.00m,
            Activo = true,
            EsOpcionalDinamico = false,
            EspecialidadId = espHormonas.Id,
            SubExamenes = new List<Examen>
            {
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "INSULINA BASAL", Unidad = "mUI/mL", ValoresReferencia = "2.6 - 24.9", Precio = 0.00m, Activo = true, Orden = 1, EspecialidadId = espHormonas.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "INSULINA POST-CARGA (75G) 2 HORAS", Unidad = "mUI/mL", ValoresReferencia = "< 50.0", Precio = 0.00m, Activo = true, Orden = 2, EspecialidadId = espHormonas.Id }
            }
        };

        var insulinaCarga75g3H = new Examen
        {
            Id = Guid.NewGuid(),
            Codigo = 533,
            Nombre = "INSULINA POST-CARGA (75G) 3 HORAS",
            Unidad = "",
            ValoresReferencia = "",
            Precio = 0.00m,
            Activo = true,
            EsOpcionalDinamico = false,
            EspecialidadId = espHormonas.Id,
            SubExamenes = new List<Examen>
            {
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "INSULINA BASAL", Unidad = "mUI/mL", ValoresReferencia = "2.6 - 24.9", Precio = 0.00m, Activo = true, Orden = 1, EspecialidadId = espHormonas.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "INSULINA POST-CARGA (75G) 1 HORA", Unidad = "mUI/mL", ValoresReferencia = "30.0 - 110.0", Precio = 0.00m, Activo = true, Orden = 2, EspecialidadId = espHormonas.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "INSULINA POST-CARGA (75G) 2 HORAS", Unidad = "mUI/mL", ValoresReferencia = "< 50.0", Precio = 0.00m, Activo = true, Orden = 3, EspecialidadId = espHormonas.Id }
            }
        };

        var listaHormonas = new List<Examen> 
        {
            new Examen { Id = Guid.NewGuid(), Codigo = 501, Nombre = "T3 LIBRE", Unidad = "pmol/L", ValoresReferencia = "3.1 - 6.8", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 502, Nombre = "T3 TOTAL", Unidad = "ng/mL", ValoresReferencia = "0.8 - 2.0", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 503, Nombre = "T4 LIBRE", Unidad = "pmol/L", ValoresReferencia = "12.0 - 22.0", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 504, Nombre = "T4 TOTAL", Unidad = "ug/mL", ValoresReferencia = "5.1 - 14.1", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 505, Nombre = "TSH", Unidad = "uUI/ml", ValoresReferencia = "0.27 - 4.2", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 506, Nombre = "ANTICUERPOS ANTI-TIROGLOBULINAS", Unidad = "UI/mL", ValoresReferencia = "< 115", Precio = 0.00m, Activo = true, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 507, Nombre = "ANTICUERPOS ANTI-TPO", Unidad = "IU/mL", ValoresReferencia = "< 34", Precio = 0.00m, Activo = true, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 508, Nombre = "TIROGLOBULINA", Unidad = "ng/mL", ValoresReferencia = "3.5 - 56.0", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 509, Nombre = "FSH", Unidad = "mUI/mL", ValoresReferencia = "1.5 - 12.4", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 510, Nombre = "LH", Unidad = "mUI/mL", ValoresReferencia = "1.7 - 8.6", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 511, Nombre = "PEPTIDO C", Unidad = "ng/mL", ValoresReferencia = "1.1 - 4.4", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 512, Nombre = "ESTRADIOL", Unidad = "pg/ml", ValoresReferencia = "12.4 - 233.0", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 513, Nombre = "PROGESTERONA", Unidad = "ng/ml", ValoresReferencia = "0.05 - 18.5", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 514, Nombre = "ANDROSTENEDIONA", Unidad = "ng/mL", ValoresReferencia = "0.3 - 3.3", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 515, Nombre = "DHEA-S", Unidad = "ug/mL", ValoresReferencia = "0.8 - 4.9", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 516, Nombre = "TESTOSTERONA TOTAL", Unidad = "nmol/l", ValoresReferencia = "8.6 - 29.0", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 517, Nombre = "TESTOSTERONA LIBRE", Unidad = "pg/ml", ValoresReferencia = "1.0 - 28.2", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 518, Nombre = "PROLACTINA", Unidad = "ng/ml", ValoresReferencia = "4.7 - 23.3", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 519, Nombre = "BHCG CUALITATIVA", Unidad = "", ValoresReferencia = "Negativo", Precio = 0.00m, Activo = true, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 520, Nombre = "CORTISOL AM", Unidad = "ng/mL", ValoresReferencia = "62.0 - 194.0", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 521, Nombre = "CORTISOL PM", Unidad = "ng/mL", ValoresReferencia = "23.0 - 119.0", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 522, Nombre = "INSULINA BASAL", Unidad = "uUI/mL", ValoresReferencia = "2.6 - 24.9", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 523, Nombre = "INSULINA POST-PRANDIAL 2 HORAS", Unidad = "uUI/mL", ValoresReferencia = "< 50.0", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 524, Nombre = "INSULINA POST-PRANDIAL 30 MIN", Unidad = "mUI/mL", ValoresReferencia = "30.0 - 100.0", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 525, Nombre = "INSULINA POST-PRANDIAL 1 HORA", Unidad = "uUI/ml", ValoresReferencia = "30.0 - 110.0", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 526, Nombre = "INDICE HOMA", Unidad = "", ValoresReferencia = "< 2.5", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 527, Nombre = "INSULINA POST-CARGA (50G) 2 HORAS", Unidad = "mUI/mL", ValoresReferencia = "< 50.0", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espHormonas.Id },
            insulinaCarga75g2H,
            new Examen { Id = Guid.NewGuid(), Codigo = 529, Nombre = "BHCG CUANTITATIVA", Unidad = "mUI/mL", ValoresReferencia = "< 5.0 (No Embarazada)", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 530, Nombre = "PARATHORMONA INTACTA (PTH)", Unidad = "pg/mL", ValoresReferencia = "15.0 - 65.0", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 531, Nombre = "TSH ULTRASENSIBLE", Unidad = "uIU/ml", ValoresReferencia = "0.35 - 4.94", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espHormonas.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 532, Nombre = "17-OH-PROGESTERONA", Unidad = "nmol/L", ValoresReferencia = "0.6 - 8.7", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espHormonas.Id },
            insulinaCarga75g3H
        };
        await SincronizarExamenesRecursivo(context, listaHormonas, espHormonas.Id);

        // --- BLOQUE 600 (MARCADORES TUMORALES: 601 - 699) ---
        var listaMarcadores = new List<Examen> 
        {
            new Examen { Id = Guid.NewGuid(), Codigo = 601, Nombre = "ALFAFETOPROTEINAS (AFP)", Unidad = "UI/mL", ValoresReferencia = "< 5.8", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espMarcadores.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 602, Nombre = "CEA", Unidad = "ng/ml", ValoresReferencia = "< 5.0", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espMarcadores.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 603, Nombre = "CA - 125", Unidad = "U/ml", ValoresReferencia = "< 35.0", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espMarcadores.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 604, Nombre = "CA - 15.3", Unidad = "U/mL", ValoresReferencia = "< 30.0", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espMarcadores.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 605, Nombre = "CA - 19.9", Unidad = "UI/ml", ValoresReferencia = "< 37.0", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espMarcadores.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 606, Nombre = "PSA TOTAL", Unidad = "ng/ml", ValoresReferencia = "< 4.0", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espMarcadores.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 607, Nombre = "PSA LIBRE", Unidad = "ng/ml", ValoresReferencia = "< 0.93", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espMarcadores.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 608, Nombre = "RELACION PSAL/PSAT", Unidad = "%", ValoresReferencia = "> 25%", Precio = 0.00m, Activo = true, EspecialidadId = espMarcadores.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 609, Nombre = "HE-4", Unidad = "pM", ValoresReferencia = "< 140.0", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espMarcadores.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 610, Nombre = "CYFRA 21.1", Unidad = "ng/mL", ValoresReferencia = "< 3.3", Precio = 0.00m, Activo = true, DecimalesRequeridos = 1, EspecialidadId = espMarcadores.Id }
        };
        await SincronizarExamenesRecursivo(context, listaMarcadores, espMarcadores.Id);

        // --- BLOQUE 700 (UROANALISIS: 701 - 799) ---

        var orinaCompleta = new Examen
        {
            Id = Guid.NewGuid(),
            Codigo = 701,
            Nombre = "ORINA COMPLETA",
            Unidad = "",
            ValoresReferencia = "",
            Precio = 0.00m,
            Activo = true,
            EsOpcionalDinamico = false,
            EspecialidadId = espUroanalisis.Id,
            SubExamenes = new List<Examen>
            {
                // ==========================================
                // 1. EXAMEN FISICO
                // ==========================================
                new Examen
                {
                    Id = Guid.NewGuid(),
                    Codigo = 0,
                    Nombre = "EXAMEN FISICO",
                    Unidad = "",
                    ValoresReferencia = "",
                    Activo = true,
                    Orden = 1,
                    EsOpcionalDinamico = false,
                    EspecialidadId = espUroanalisis.Id,
                    SubExamenes = new List<Examen>
                    {
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "VOLUMEN", Unidad = "mL", ValoresReferencia = "", Activo = true, Orden = 1, EsOpcionalDinamico = false, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "COLOR", Unidad = "", ValoresReferencia = "Amarillo", Activo = true, Orden = 2, EsOpcionalDinamico = false, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "ASPECTO", Unidad = "", ValoresReferencia = "Límpido", Activo = true, Orden = 3, EsOpcionalDinamico = false, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "OLOR", Unidad = "", ValoresReferencia = "Sui-géneris", Activo = true, Orden = 4, EsOpcionalDinamico = false, EspecialidadId = espUroanalisis.Id }
                    }
                },

                // ==========================================
                // 2. EXAMEN QUIMICO
                // ==========================================
                new Examen
                {
                    Id = Guid.NewGuid(),
                    Codigo = 0,
                    Nombre = "EXAMEN QUIMICO",
                    Unidad = "",
                    ValoresReferencia = "",
                    Activo = true,
                    Orden = 2,
                    EsOpcionalDinamico = false,
                    EspecialidadId = espUroanalisis.Id,
                    SubExamenes = new List<Examen>
                    {
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "DENSIDAD", Unidad = "", ValoresReferencia = "1.015 - 1.025", Activo = true, Orden = 1, EsOpcionalDinamico = false, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "PH/REACCION", Unidad = "", ValoresReferencia = "5.0 - 7.0", Activo = true, Orden = 2, EsOpcionalDinamico = false, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "PROTEINAS", Unidad = "mg/dL", ValoresReferencia = "Negativo", Activo = true, Orden = 3, EsOpcionalDinamico = false, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "GLUCOSA", Unidad = "mg/dL", ValoresReferencia = "Negativo", Activo = true, Orden = 4, EsOpcionalDinamico = false, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "BILIRRUBINA", Unidad = "", ValoresReferencia = "Negativo", Activo = true, Orden = 5, EsOpcionalDinamico = false, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "HEMOGLOBINA/HEMATIES", Unidad = "", ValoresReferencia = "Negativo", Activo = true, Orden = 6, EsOpcionalDinamico = false, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CUERPOS CETONICOS", Unidad = "mg/dL", ValoresReferencia = "Negativo", Activo = true, Orden = 7, EsOpcionalDinamico = false, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "NITRITOS", Unidad = "", ValoresReferencia = "Negativo", Activo = true, Orden = 8, EsOpcionalDinamico = false, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "UROBILINOGENO", Unidad = "mg/dL", ValoresReferencia = "Normal (< 1.0)", Activo = true, Orden = 9, EsOpcionalDinamico = false, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "LEUCOCITOS", Unidad = "Leu/uL", ValoresReferencia = "Negativo", Activo = true, Orden = 10, EsOpcionalDinamico = false, EspecialidadId = espUroanalisis.Id }
                    }
                },

                // ==========================================
                // 3. EXAMEN MICROSCOPICO
                // ==========================================
                new Examen
                {
                    Id = Guid.NewGuid(),
                    Codigo = 0,
                    Nombre = "EXAMEN MICROSCOPICO",
                    Unidad = "",
                    ValoresReferencia = "",
                    Activo = true,
                    Orden = 3,
                    EsOpcionalDinamico = false,
                    EspecialidadId = espUroanalisis.Id,
                    SubExamenes = new List<Examen>
                    {
                        // FIJOS
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "BACTERIAS", Unidad = "", ValoresReferencia = "Escasas o Ausentes", Activo = true, Orden = 1, EsOpcionalDinamico = false, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "LEUCOCITOS", Unidad = "/campo", ValoresReferencia = "0 - 5", Activo = true, Orden = 2, DecimalesRequeridos = 0, EsOpcionalDinamico = false, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "HEMATIES", Unidad = "/campo", ValoresReferencia = "0 - 2", Activo = true, Orden = 3, DecimalesRequeridos = 0, EsOpcionalDinamico = false, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CELULAS EPITELIALES PLANAS", Unidad = "", ValoresReferencia = "Escasas", Activo = true, Orden = 4, EsOpcionalDinamico = false, EspecialidadId = espUroanalisis.Id },

                        // DINÁMICOS / OPCIONALES
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CELULAS DE TRANSICION", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 5, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CELULAS DE TRANSICION VACUOLADAS", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 6, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CELULAS RENALES", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 7, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "FRAGMENTO DE EPITELIO DE TRANSICION", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 8, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "FILAMENTOS DE MUCINA", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 9, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "ACUMULOS LEUCOCITARIOS", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 10, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CRIST. URATOS AMORFOS", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 11, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CRIST. OXALATOS DE CALCIO DIHIDRATADOS", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 12, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CRIST. OXALATO DE CALCIO MONOHIDRATADO", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 13, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CRIST. FOSFATOS AMORFOS", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 14, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CRIST. DE ACIDO URICO", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 15, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CRIST. FOSFATO AMONICO MAGNESICO (FOSFATOS TRIPLE)", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 16, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CRISTALES FOSFATO DE CALCIO", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 17, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CRIST. URATO AMONICO (BIURATO DE AMONIO)", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 18, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "BLASTOCONIDIAS (LEVADURAS NO GEMANTES)", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 19, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "BLASTOCONIDIAS EN GEMACION (LEVADURAS GEMANTES)", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 20, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "LEVADURAS Y SUS HIFAS", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 21, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "ESPERMATOZOIDES", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 22, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CUERPOS OVALES GRASOS", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 23, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "TROFOZOITOS DE TRICHOMONAS VAGINALIS", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 24, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CILINDRO HIALINO (100X)", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 25, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CILINDRO LEUCOCITARIO (100X)", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 26, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CILINDRO GRANULOSO (100X)", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 27, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CILINDRO GRANULOSO FINO (100X)", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 28, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CILINDRO GRANULOSO GRUESO (100X)", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 29, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CILINDRO ERITROCITARIO (100X)", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 30, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CILINDRO CEREO (100X)", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 31, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CILINDRO EPITELIAL", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 32, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "ERITROCITOS DIMORFICOS", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 33, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "AGUJAS DE TIROSINA", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 34, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CELULAS EPITELIALES PLANAS ESCAMOSAS", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 35, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CRIST. SULFATO DE CALCIO", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 36, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CRISTALES DE CISTINA", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 37, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CRISTALES DE COLESTEROL", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 38, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CRISTALES DE LEUCINA", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 39, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CRISTALES DE BILIRRUBINA", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 40, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CELULAS CLAVES SUGESTIVAS DE G.vaginalis", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 41, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "AGREGADOS DE CRISTALES DE OXALATO DE CALCIO", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 42, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "AGREGADOS DE CRISTALES DE ACIDO URICO", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 43, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "MACLAS DE ACIDO URICO", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 44, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "MACLAS DE OXALATO DE CALCIO", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 45, EsOpcionalDinamico = true, EspecialidadId = espUroanalisis.Id }
                    }
                }
            }
        };

        var acidoUrico24h = new Examen
        {
            Id = Guid.NewGuid(), Codigo = 703, Nombre = "ACIDO URICO EN ORINA DE 24 HORAS", Unidad = "", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id,
            SubExamenes = new List<Examen>
            {
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "VOLUMEN DE ORINA", Unidad = "mL/24h", ValoresReferencia = "", Activo = true, Orden = 1, EspecialidadId = espUroanalisis.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "ACIDO URICO EN ORINA DE 24 HORAS", Unidad = "g/24h", ValoresReferencia = "", Activo = true, Orden = 2, EspecialidadId = espUroanalisis.Id }
            }
        };

        var calcio24h = new Examen
        {
            Id = Guid.NewGuid(), Codigo = 705, Nombre = "CALCIO EN ORINA DE 24 HORAS", Unidad = "", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id,
            SubExamenes = new List<Examen>
            {
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "VOLUMEN DE ORINA", Unidad = "mL/24h", ValoresReferencia = "", Activo = true, Orden = 1, EspecialidadId = espUroanalisis.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CALCIO EN ORINA DE 24 HORAS", Unidad = "g/24h", ValoresReferencia = "", Activo = true, Orden = 2, EspecialidadId = espUroanalisis.Id }
            }
        };

        var proteinuria24h = new Examen
        {
            Id = Guid.NewGuid(), Codigo = 706, Nombre = "PROTEINURIA DE 24 HORAS", Unidad = "", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id,
            SubExamenes = new List<Examen>
            {
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "VOLUMEN 24 HORAS", Unidad = "mL/24 h", ValoresReferencia = "", Activo = true, Orden = 1, EspecialidadId = espUroanalisis.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "PROTEINURIA DE 24 HORAS", Unidad = "g/24 h", ValoresReferencia = "", Activo = true, Orden = 2, EspecialidadId = espUroanalisis.Id }
            }
        };

        var creatinina24h = new Examen
        {
            Id = Guid.NewGuid(), Codigo = 708, Nombre = "CREATININA EN ORINA DE 24 HORAS", Unidad = "", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id,
            SubExamenes = new List<Examen>
            {
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "VOLUMEN DE ORINA", Unidad = "ml/24 h", ValoresReferencia = "", Activo = true, Orden = 1, EspecialidadId = espUroanalisis.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CREATININA EN ORINA DE 24 HORAS", Unidad = "g/24 h", ValoresReferencia = "", Activo = true, Orden = 2, EspecialidadId = espUroanalisis.Id }
            }
        };

        var urea24h = new Examen
        {
            Id = Guid.NewGuid(), Codigo = 710, Nombre = "UREA EN ORINA DE 24 HORAS", Unidad = "", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id,
            SubExamenes = new List<Examen>
            {
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "VOLUMEN DE ORINA", Unidad = "ml", ValoresReferencia = "", Activo = true, Orden = 1, EspecialidadId = espUroanalisis.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "UREA EN ORINA DE 24 HORAS", Unidad = "g/24 h", ValoresReferencia = "", Activo = true, Orden = 2, EspecialidadId = espUroanalisis.Id }
            }
        };

        var proteinuria12h = new Examen
        {
            Id = Guid.NewGuid(), Codigo = 714, Nombre = "PROTEINURIA DE 12 HORAS", Unidad = "", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id,
            SubExamenes = new List<Examen>
            {
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "VOLUMEN TOTAL", Unidad = "mL/12 h", ValoresReferencia = "", Activo = true, Orden = 1, EspecialidadId = espUroanalisis.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "PROTEINAS EN ORINA", Unidad = "mg/dl", ValoresReferencia = "", Activo = true, Orden = 2, EspecialidadId = espUroanalisis.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "PROTEINURIA DE 12 HORAS", Unidad = "g/12 h", ValoresReferencia = "", Activo = true, Orden = 3, EspecialidadId = espUroanalisis.Id }
            }
        };

        var microalbuminuria = new Examen
        {
            Id = Guid.NewGuid(), Codigo = 715, Nombre = "ALBUMINURIA ( MICROALBUMINURIA ) CUANTITATIVA", Unidad = "", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id,
            SubExamenes = new List<Examen>
            {
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "ALBUMINURIA CUANTITATIVA", Unidad = "mg/L", ValoresReferencia = "", Activo = true, Orden = 1, EspecialidadId = espUroanalisis.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "ALBUMINURIA (Microalbuminuria) CUANTITATIVA", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 2, EspecialidadId = espUroanalisis.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "ALBUMINURIA (Microalbuminuria ) en Orina de 24Horas", Unidad = "mg/24h", ValoresReferencia = "", Activo = true, Orden = 3, EspecialidadId = espUroanalisis.Id }
            }
        };

        var fosforo24h = new Examen
        {
            Id = Guid.NewGuid(), Codigo = 718, Nombre = "FOSFORO EN ORINA DE 24 HORAS", Unidad = "", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id,
            SubExamenes = new List<Examen>
            {
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "VOLUMEN DE ORINA", Unidad = "ml/24 h", ValoresReferencia = "", Activo = true, Orden = 1, EspecialidadId = espUroanalisis.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "FOSFORO EN ORINA DE 24 HORAS", Unidad = "g/24h", ValoresReferencia = "", Activo = true, Orden = 2, EspecialidadId = espUroanalisis.Id }
            }
        };

        var magnesio24h = new Examen
        {
            Id = Guid.NewGuid(), Codigo = 720, Nombre = "MAGNESIO EN ORINA DE 24 HORAS", Unidad = "", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id,
            SubExamenes = new List<Examen>
            {
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "VOLUMEN 24 HORAS", Unidad = "mL/24 h", ValoresReferencia = "", Activo = true, Orden = 1, EspecialidadId = espUroanalisis.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "MAGNESIO EN ORINA DE 24 HORAS", Unidad = "mg/24 h", ValoresReferencia = "", Activo = true, Orden = 2, EspecialidadId = espUroanalisis.Id }
            }
        };

        var electrolitosOrina = new Examen
        {
            Id = Guid.NewGuid(), Codigo = 721, Nombre = "ELECTROLITOS (Na, K, Cl) EN ORINA OCASIONAL", Unidad = "", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id,
            SubExamenes = new List<Examen>
            {
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "SODIO", Unidad = "mEq/L", ValoresReferencia = "", Activo = true, Orden = 1, EspecialidadId = espUroanalisis.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "POTASIO", Unidad = "mEq/L", ValoresReferencia = "", Activo = true, Orden = 2, EspecialidadId = espUroanalisis.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CLORO", Unidad = "mEq/L", ValoresReferencia = "", Activo = true, Orden = 3, EspecialidadId = espUroanalisis.Id }
            }
        };

        var electrolitos24h = new Examen
        {
            Id = Guid.NewGuid(), Codigo = 722, Nombre = "ELECTROLITOS (Na, K, Cl) EN ORINA DE 24 HORAS", Unidad = "", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id,
            SubExamenes = new List<Examen>
            {
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "VOLUMEN DE ORINA", Unidad = "mL/24 h", ValoresReferencia = "", Activo = true, Orden = 1, EspecialidadId = espUroanalisis.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "SODIO EN ORINA DE 24 HORAS", Unidad = "mEq/24 h", ValoresReferencia = "", Activo = true, Orden = 2, EspecialidadId = espUroanalisis.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "POTASIO EN ORINA DE 24 HORAS", Unidad = "mEq/24 h", ValoresReferencia = "", Activo = true, Orden = 3, EspecialidadId = espUroanalisis.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CLORO EN ORINA DE 24 HORAS", Unidad = "mEq/24 h", ValoresReferencia = "", Activo = true, Orden = 4, EspecialidadId = espUroanalisis.Id }
            }
        };

        var glucosaOrina = new Examen
        {
            Id = Guid.NewGuid(), Codigo = 727, Nombre = "GLUCOSA EN ORINA", Unidad = "", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id,
            SubExamenes = new List<Examen>
            {
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "GLUCOSA EN ORINA", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 1, EspecialidadId = espUroanalisis.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "GLUCOSA EN ORINA 24H", Unidad = "g/24h", ValoresReferencia = "", Activo = true, Orden = 2, EspecialidadId = espUroanalisis.Id }
            }
        };

        var depuracionCreatinina12h = new Examen
        {
            Id = Guid.NewGuid(), Codigo = 728, Nombre = "DEPURACION DE CREATININA 12 HORAS", Unidad = "", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id,
            SubExamenes = new List<Examen>
            {
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "VOLUMEN TOTAL", Unidad = "mL", ValoresReferencia = "", Activo = true, Orden = 1, EspecialidadId = espUroanalisis.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CREATININA EN ORINA", Unidad = "mg/dL", ValoresReferencia = "", Activo = true, Orden = 2, EspecialidadId = espUroanalisis.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CREATININA EN SUERO", Unidad = "mg/dL", ValoresReferencia = "", Activo = true, Orden = 3, EspecialidadId = espUroanalisis.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "DEPURACION DE CREATININA DE 12 H. SIN CORREGIR", Unidad = "mL/min", ValoresReferencia = "", Activo = true, Orden = 4, EspecialidadId = espUroanalisis.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "DEPURACION DE CREATININA 12 HORAS CORREGIDA CON", Unidad = "mL/min", ValoresReferencia = "", Activo = true, Orden = 5, EspecialidadId = espUroanalisis.Id }
            }
        };

        var depuracionCreatinina24h = new Examen
        {
            Id = Guid.NewGuid(), Codigo = 729, Nombre = "DEPURACION DE CREATININA 24 HORAS", Unidad = "", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id,
            SubExamenes = new List<Examen>
            {
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "VOLUMEN TOTAL", Unidad = "mL", ValoresReferencia = "", Activo = true, Orden = 1, EspecialidadId = espUroanalisis.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CREATININA EN ORINA", Unidad = "mg/dL", ValoresReferencia = "", Activo = true, Orden = 2, EspecialidadId = espUroanalisis.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CREATININA EN SUERO", Unidad = "mg/dL", ValoresReferencia = "", Activo = true, Orden = 3, EspecialidadId = espUroanalisis.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "DEP. CREATININA SIN CORREGIR", Unidad = "mL/min", ValoresReferencia = "", Activo = true, Orden = 4, EspecialidadId = espUroanalisis.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "DEPURACION DE CREATININA CORREGIDA CON PESO Y TALLA", Unidad = "mL/min", ValoresReferencia = "", Activo = true, Orden = 5, EspecialidadId = espUroanalisis.Id }
            }
        };

        var listaUroanalisis = new List<Examen> 
        {
            orinaCompleta,
            new Examen { Id = Guid.NewGuid(), Codigo = 702, Nombre = "ACIDO URICO EN ORINA OCASIONAL", Unidad = "mg/dl", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id },
            acidoUrico24h,
            new Examen { Id = Guid.NewGuid(), Codigo = 704, Nombre = "CALCIO EN ORINA OCASIONAL", Unidad = "mg/dL", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id },
            calcio24h,
            proteinuria24h,
            new Examen { Id = Guid.NewGuid(), Codigo = 707, Nombre = "CREATININA EN ORINA OCASIONAL", Unidad = "mg/dL", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id },
            creatinina24h,
            new Examen { Id = Guid.NewGuid(), Codigo = 709, Nombre = "UREA EN ORINA OCASIONAL", Unidad = "mg/dL", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id },
            urea24h,
            new Examen { Id = Guid.NewGuid(), Codigo = 711, Nombre = "PROTEINAS EN ORINA OCASIONAL", Unidad = "mg/dL", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 712, Nombre = "RELACION CALCIO/CREATININA", Unidad = "", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 713, Nombre = "RELACION AC. URICO/CREATININA", Unidad = "", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id },
            proteinuria12h,
            microalbuminuria,
            new Examen { Id = Guid.NewGuid(), Codigo = 716, Nombre = "RELACION ALBUMINA/CREATININA EN ORINA", Unidad = "mg/g", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 717, Nombre = "FOSFORO EN ORINA OCASIONAL", Unidad = "mg/dl", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id },
            fosforo24h,
            new Examen { Id = Guid.NewGuid(), Codigo = 719, Nombre = "MAGNESIO EN ORINA OCASIONAL", Unidad = "mg/dl", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id },
            magnesio24h,
            electrolitosOrina,
            electrolitos24h,
            new Examen { Id = Guid.NewGuid(), Codigo = 723, Nombre = "RELACION PROTEINAS / CREATININA", Unidad = "", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 724, Nombre = "RELACION UREA / CREATININA", Unidad = "", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 725, Nombre = "RELACION FOSFORO / CREATININA", Unidad = "", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 726, Nombre = "RELACION CALCIO/ AC. URICO", Unidad = "", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id },
            glucosaOrina,
            depuracionCreatinina12h,
            depuracionCreatinina24h,
            new Examen { Id = Guid.NewGuid(), Codigo = 730, Nombre = "DENSIDAD URINARIA (METODO: REFRACTOMETRIA)", Unidad = "g/mL", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 731, Nombre = "AMILASA EN ORINA OCASIONAL", Unidad = "U/L", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 732, Nombre = "PROTEINA DE BENCE JONES", Unidad = "", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espUroanalisis.Id }
        };
        await SincronizarExamenesRecursivo(context, listaUroanalisis, espUroanalisis.Id);

        // --- BLOQUE 800 (COPROLOGIA: 801 - 899) ---
        var hecesDirecto = new Examen
        {
            Id = Guid.NewGuid(),
            Codigo = 801,
            Nombre = "HECES DIRECTO",
            Unidad = "",
            ValoresReferencia = "",
            Precio = 0.00m,
            Activo = true,
            EsOpcionalDinamico = false,
            EspecialidadId = espCoprologia.Id,
            SubExamenes = new List<Examen>
            {
                // ==========================================
                // 1. EXAMEN MACROSCOPICO
                // ==========================================
                new Examen
                {
                    Id = Guid.NewGuid(),
                    Codigo = 0,
                    Nombre = "EXAMEN MACROSCOPICO",
                    Unidad = "",
                    ValoresReferencia = "",
                    Activo = true,
                    Orden = 1,
                    EsOpcionalDinamico = false,
                    EspecialidadId = espCoprologia.Id,
                    SubExamenes = new List<Examen>
                    {
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "ASPECTO", Unidad = "", ValoresReferencia = "Heterogéneo", Activo = true, Orden = 1, EsOpcionalDinamico = false, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "COLOR", Unidad = "", ValoresReferencia = "Marrón", Activo = true, Orden = 2, EsOpcionalDinamico = false, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "OLOR", Unidad = "", ValoresReferencia = "Fecal de Ley", Activo = true, Orden = 3, EsOpcionalDinamico = false, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CONSISTENCIA", Unidad = "", ValoresReferencia = "Blanda / Pastosa", Activo = true, Orden = 4, EsOpcionalDinamico = false, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "REACCION", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 5, EsOpcionalDinamico = false, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "RESTOS DE ALIMENTOS", Unidad = "", ValoresReferencia = "Escasos o Ausentes", Activo = true, Orden = 6, EsOpcionalDinamico = false, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "MOCO", Unidad = "", ValoresReferencia = "No se observa", Activo = true, Orden = 7, EsOpcionalDinamico = false, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "SANGRE", Unidad = "", ValoresReferencia = "No se observa", Activo = true, Orden = 8, EsOpcionalDinamico = false, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "PUS", Unidad = "", ValoresReferencia = "No se observa", Activo = true, Orden = 9, EsOpcionalDinamico = false, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "HELMINTOS ADULTOS", Unidad = "", ValoresReferencia = "No se observan", Activo = true, Orden = 10, EsOpcionalDinamico = false, EspecialidadId = espCoprologia.Id }
                    }
                },

                // ==========================================
                // 2. EXAMEN MICROSCOPICO 
                // ==========================================
                new Examen
                {
                    Id = Guid.NewGuid(),
                    Codigo = 0,
                    Nombre = "EXAMEN MICROSCOPICO",
                    Unidad = "",
                    ValoresReferencia = "",
                    Activo = true,
                    Orden = 2,
                    EsOpcionalDinamico = false,
                    EspecialidadId = espCoprologia.Id,
                    SubExamenes = new List<Examen>
                    {
                        // CAMPO PRINCIPAL (Texto libre en la UI. Default: "NO SE OBSERVARON...")
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "FORMAS EVOLUTIVAS DE PARASITOS", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 1, EsOpcionalDinamico = false, EspecialidadId = espCoprologia.Id },

                        // DINÁMICOS / OPCIONALES (Sin valores de referencia rígidos)
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "LEUCOCITOS", Unidad = "/campo", ValoresReferencia = "", Activo = true, Orden = 2, EsOpcionalDinamico = true, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "HEMATIES", Unidad = "/campo", ValoresReferencia = "", Activo = true, Orden = 3, EsOpcionalDinamico = true, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "LEVADURAS", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 4, EsOpcionalDinamico = true, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "FLORA INTESTINAL", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 5, EsOpcionalDinamico = true, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "ALMIDON INDIGERIDO", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 6, EsOpcionalDinamico = true, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "RESTOS DE CELULOSA", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 7, EsOpcionalDinamico = true, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "GLOBULOS DE GRASA", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 8, EsOpcionalDinamico = true, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "FIBRAS MUSCULARES INDIGERIDAS", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 9, EsOpcionalDinamico = true, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "FIBRAS MUSCULARES SEMIDIGERIDAS", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 10, EsOpcionalDinamico = true, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "FIBRAS MUSCULARES DIGERIDAS", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 11, EsOpcionalDinamico = true, EspecialidadId = espCoprologia.Id }
                    }
                }
            }
        };

        var hecesConcentracion = new Examen
        {
            Id = Guid.NewGuid(),
            Codigo = 802,
            Nombre = "HECES POR CONCENTRACION",
            Unidad = "",
            ValoresReferencia = "",
            Precio = 0.00m,
            Activo = true,
            EsOpcionalDinamico = false,
            EspecialidadId = espCoprologia.Id,
            SubExamenes = new List<Examen>
            {
                // ==========================================
                // 1. EXAMEN MACROSCOPICO
                // ==========================================
                new Examen
                {
                    Id = Guid.NewGuid(), Codigo = 0, Nombre = "EXAMEN MACROSCOPICO", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 1, EsOpcionalDinamico = false, EspecialidadId = espCoprologia.Id,
                    SubExamenes = new List<Examen>
                    {
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "ASPECTO", Unidad = "", ValoresReferencia = "Heterogéneo", Activo = true, Orden = 1, EsOpcionalDinamico = false, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "COLOR", Unidad = "", ValoresReferencia = "Marrón", Activo = true, Orden = 2, EsOpcionalDinamico = false, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CONSISTENCIA", Unidad = "", ValoresReferencia = "Blanda / Pastosa", Activo = true, Orden = 3, EsOpcionalDinamico = false, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "REACCION", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 4, EsOpcionalDinamico = false, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "OLOR", Unidad = "", ValoresReferencia = "Fecal de Ley", Activo = true, Orden = 5, EsOpcionalDinamico = false, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "MOCO", Unidad = "", ValoresReferencia = "No se observa", Activo = true, Orden = 6, EsOpcionalDinamico = false, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "SANGRE", Unidad = "", ValoresReferencia = "No se observa", Activo = true, Orden = 7, EsOpcionalDinamico = false, EspecialidadId = espCoprologia.Id }
                    }
                },
                // ==========================================
                // 2. EXAMEN MICROSCOPICO
                // ==========================================
                new Examen
                {
                    Id = Guid.NewGuid(), Codigo = 0, Nombre = "EXAMEN MICROSCOPICO", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 2, EsOpcionalDinamico = false, EspecialidadId = espCoprologia.Id,
                    SubExamenes = new List<Examen>
                    {
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "FORMAS EVOLUTIVAS DE PARASITOS", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 1, EsOpcionalDinamico = false, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "LEUCOCITOS", Unidad = "/campo", ValoresReferencia = "", Activo = true, Orden = 2, EsOpcionalDinamico = true, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "HEMATIES", Unidad = "/campo", ValoresReferencia = "", Activo = true, Orden = 3, EsOpcionalDinamico = true, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "LEVADURAS", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 4, EsOpcionalDinamico = true, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "FLORA INTESTINAL", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 5, EsOpcionalDinamico = true, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "ALMIDON INDIGERIDO", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 6, EsOpcionalDinamico = true, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "RESTOS DE CELULOSA", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 7, EsOpcionalDinamico = true, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "GLOBULOS DE GRASA", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 8, EsOpcionalDinamico = true, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "FIBRAS MUSCULARES INDIGERIDAS", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 9, EsOpcionalDinamico = true, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "FIBRAS MUSCULARES SEMIDIGERIDAS", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 10, EsOpcionalDinamico = true, EspecialidadId = espCoprologia.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "FIBRAS MUSCULARES DIGERIDAS", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 11, EsOpcionalDinamico = true, EspecialidadId = espCoprologia.Id }
                    }
                }
            }
        };

        var sangreOcultaSeriadas = new Examen
        {
            Id = Guid.NewGuid(), Codigo = 804, Nombre = "SANGRE OCULTA (Seriadas 3 muestras)", Unidad = "", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espCoprologia.Id,
            SubExamenes = new List<Examen>
            {
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "SANGRE OCULTA 1ERA MUESTRA", Unidad = "", ValoresReferencia = "Negativo", Activo = true, Orden = 1, EspecialidadId = espCoprologia.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "SANGRE OCULTA 2DA MUESTRA", Unidad = "", ValoresReferencia = "Negativo", Activo = true, Orden = 2, EspecialidadId = espCoprologia.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "SANGRE OCULTA 3ERA MUESTRA", Unidad = "", ValoresReferencia = "Negativo", Activo = true, Orden = 3, EspecialidadId = espCoprologia.Id }
            }
        };

        var phAzucares = new Examen
        {
            Id = Guid.NewGuid(), Codigo = 805, Nombre = "PH Y AZUCARES REDUCTORES", Unidad = "", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espCoprologia.Id,
            SubExamenes = new List<Examen>
            {
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "PH", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 1, EspecialidadId = espCoprologia.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "AZUCARES REDUCTORES", Unidad = "", ValoresReferencia = "Negativo", Activo = true, Orden = 2, EspecialidadId = espCoprologia.Id }
            }
        };

        var recuentoLeucocitosHeces = new Examen
        {
            Id = Guid.NewGuid(), Codigo = 806, Nombre = "RECUENTO DIFERENCIAL DE LEUCOCITOS EN HECES", Unidad = "", ValoresReferencia = "", Precio = 0.00m, Activo = true, EspecialidadId = espCoprologia.Id,
            SubExamenes = new List<Examen>
            {
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "RESULTADO", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 1, EsOpcionalDinamico = false, EspecialidadId = espCoprologia.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "POLIMORFONUCLEARES", Unidad = "%", ValoresReferencia = "", Activo = true, Orden = 2, EsOpcionalDinamico = false, EspecialidadId = espCoprologia.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "MONONUCLEARES", Unidad = "%", ValoresReferencia = "", Activo = true, Orden = 3, EsOpcionalDinamico = false, EspecialidadId = espCoprologia.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "EOSINOFILOS", Unidad = "%", ValoresReferencia = "", Activo = true, Orden = 4, EsOpcionalDinamico = false, EspecialidadId = espCoprologia.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "NOTA", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 5, EsOpcionalDinamico = true, EspecialidadId = espCoprologia.Id }
            }
        };

        var listaCoprologia = new List<Examen> 
        {
            hecesDirecto, 
            hecesConcentracion,
            new Examen { Id = Guid.NewGuid(), Codigo = 803, Nombre = "SANGRE OCULTA EN HECES", Unidad = "", ValoresReferencia = "Negativo", Precio = 0.00m, Activo = true, EspecialidadId = espCoprologia.Id },
            sangreOcultaSeriadas,
            phAzucares,
            recuentoLeucocitosHeces,
            new Examen { Id = Guid.NewGuid(), Codigo = 807, Nombre = "INVEST. DE OXIUROS (TECNICA DE GRAHAM)", Unidad = "", ValoresReferencia = "No se observan", Precio = 0.00m, Activo = true, EspecialidadId = espCoprologia.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 808, Nombre = "DET. CUALIT. DE GRASA (SUDAN III)", Unidad = "", ValoresReferencia = "Negativo", Precio = 0.00m, Activo = true, EspecialidadId = espCoprologia.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 809, Nombre = "HELICOBACTER PYLORI EN HECES AG", Unidad = "", ValoresReferencia = "Negativo", Precio = 0.00m, Activo = true, EspecialidadId = espCoprologia.Id }
        };
        await SincronizarExamenesRecursivo(context, listaCoprologia, espCoprologia.Id);

        // --- BLOQUE 900 (BACTERIOLOGIA: 901 - 999) ---
        var bkGeneral = new Examen
        {
            Id = Guid.NewGuid(), Codigo = 903, Nombre = "B.K (BACILOSCOPIA)", Unidad = "", ValoresReferencia = "", Precio = 0.00m, Activo = true, EsOpcionalDinamico = false, EspecialidadId = espBacteriologia.Id,
            SubExamenes = new List<Examen>
            {
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "TIPO DE MUESTRA", Unidad = "", ValoresReferencia = "Esputo", Activo = true, Orden = 1, EspecialidadId = espBacteriologia.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "B.K (BACILOSCOPIA)", Unidad = "", ValoresReferencia = "Negativo", Activo = true, Orden = 2, EspecialidadId = espBacteriologia.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "ASPECTO DE LA MUESTRA", Unidad = "", ValoresReferencia = "Mucopurulento", Activo = true, Orden = 3, EspecialidadId = espBacteriologia.Id }
            }
        };

        var bkPrimera = new Examen
        {
            Id = Guid.NewGuid(), Codigo = 904, Nombre = "BK (BACILOSCOPIA) PRIMERA MUESTRA", Unidad = "", ValoresReferencia = "", Precio = 0.00m, Activo = true, EsOpcionalDinamico = false, EspecialidadId = espBacteriologia.Id,
            SubExamenes = new List<Examen>
            {
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "TIPO DE MUESTRA", Unidad = "", ValoresReferencia = "Esputo", Activo = true, Orden = 1, EspecialidadId = espBacteriologia.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "B.K (BACILOSCOPIA)", Unidad = "", ValoresReferencia = "Negativo", Activo = true, Orden = 2, EspecialidadId = espBacteriologia.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "ASPECTO DE LA MUESTRA", Unidad = "", ValoresReferencia = "Mucopurulento", Activo = true, Orden = 3, EspecialidadId = espBacteriologia.Id }
            }
        };

        var bkSegunda = new Examen
        {
            Id = Guid.NewGuid(), Codigo = 905, Nombre = "BK (BACILOSCOPIA) SEGUNDA MUESTRA", Unidad = "", ValoresReferencia = "", Precio = 0.00m, Activo = true, EsOpcionalDinamico = false, EspecialidadId = espBacteriologia.Id,
            SubExamenes = new List<Examen>
            {
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "TIPO DE MUESTRA", Unidad = "", ValoresReferencia = "Esputo", Activo = true, Orden = 1, EspecialidadId = espBacteriologia.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "B.K (BACILOSCOPIA)", Unidad = "", ValoresReferencia = "Negativo", Activo = true, Orden = 2, EspecialidadId = espBacteriologia.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "ASPECTO DE LA MUESTRA", Unidad = "", ValoresReferencia = "Mucopurulento", Activo = true, Orden = 3, EspecialidadId = espBacteriologia.Id }
            }
        };

        var bkTercera = new Examen
        {
            Id = Guid.NewGuid(), Codigo = 906, Nombre = "BK (BACILOSCOPIA) TERCERA MUESTRA", Unidad = "", ValoresReferencia = "", Precio = 0.00m, Activo = true, EsOpcionalDinamico = false, EspecialidadId = espBacteriologia.Id,
            SubExamenes = new List<Examen>
            {
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "TIPO DE MUESTRA", Unidad = "", ValoresReferencia = "Esputo", Activo = true, Orden = 1, EspecialidadId = espBacteriologia.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "B.K (BACILOSCOPIA)", Unidad = "", ValoresReferencia = "Negativo", Activo = true, Orden = 2, EspecialidadId = espBacteriologia.Id },
                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "ASPECTO DE LA MUESTRA", Unidad = "", ValoresReferencia = "Mucopurulento", Activo = true, Orden = 3, EspecialidadId = espBacteriologia.Id }
            }
        };

        var listaBacteriologia = new List<Examen> 
        {
            new Examen { Id = Guid.NewGuid(), Codigo = 901, Nombre = "UROCULTIVO", Unidad = "", ValoresReferencia = "Negativo", Precio = 0.00m, Activo = true, EspecialidadId = espBacteriologia.Id },
            new Examen { Id = Guid.NewGuid(), Codigo = 902, Nombre = "EXUDADO FARINGEO", Unidad = "", ValoresReferencia = "Flora Normal", Precio = 0.00m, Activo = true, EspecialidadId = espBacteriologia.Id },
            bkGeneral,
            bkPrimera,
            bkSegunda,
            bkTercera,
            new Examen { Id = Guid.NewGuid(), Codigo = 907, Nombre = "COPROCULTIVO", Unidad = "", ValoresReferencia = "Negativo", Precio = 0.00m, Activo = true, EspecialidadId = espBacteriologia.Id }
        };
        await SincronizarExamenesRecursivo(context, listaBacteriologia, espBacteriologia.Id);

        // --- BLOQUE 1000 (ESPECIALES: 1001 - 1099) ---
        var espermatograma = new Examen
        {
            Id = Guid.NewGuid(),
            Codigo = 1001,
            Nombre = "ESPERMATOGRAMA",
            Unidad = "",
            ValoresReferencia = "",
            Precio = 0.00m,
            Activo = true,
            EsOpcionalDinamico = false,
            EspecialidadId = espEspeciales.Id,
            SubExamenes = new List<Examen>
            {
                // ==========================================
                // 1. EXAMINACIÓN MACROSCÓPICA
                // ==========================================
                new Examen
                {
                    Id = Guid.NewGuid(), Codigo = 0, Nombre = "EXAMINACIÓN MACROSCÓPICA", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 1, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id,
                    SubExamenes = new List<Examen>
                    {
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "VOLUMEN EYACULADO", Unidad = "ml", ValoresReferencia = ">= 1.5", Activo = true, Orden = 1, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "APARIENCIA VISUAL", Unidad = "", ValoresReferencia = "Gris Opaco", Activo = true, Orden = 2, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "LIQUEFACCION", Unidad = "minutos", ValoresReferencia = "< 60", Activo = true, Orden = 3, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "VISCOCIDAD", Unidad = "cm", ValoresReferencia = "< 2.0", Activo = true, Orden = 4, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id },
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "pH", Unidad = "", ValoresReferencia = ">= 7.2", Activo = true, Orden = 5, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id }
                    }
                },

                // ==========================================
                // 2. EXAMINACIÓN MICROSCÓPICA
                // ==========================================
                new Examen
                {
                    Id = Guid.NewGuid(), Codigo = 0, Nombre = "EXAMINACIÓN MICROSCÓPICA", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 2, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id,
                    SubExamenes = new List<Examen>
                    {
                        // 2.1 MOTILIDAD
                        new Examen
                        {
                            Id = Guid.NewGuid(), Codigo = 0, Nombre = "MOTILIDAD", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 1, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id,
                            SubExamenes = new List<Examen>
                            {
                                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "a. MÓVILES PROGRESIVO RÁPIDO", Unidad = "%", ValoresReferencia = "", Activo = true, Orden = 1, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id },
                                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "b. MÓVILES PROGRESIVO LENTO", Unidad = "%", ValoresReferencia = "", Activo = true, Orden = 2, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id },
                                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "c. MÓVILES NO PROGRESIVO", Unidad = "%", ValoresReferencia = "", Activo = true, Orden = 3, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id },
                                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "d. INMÓVILES", Unidad = "%", ValoresReferencia = "", Activo = true, Orden = 4, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id },
                                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "TOTAL MÓVILES (a+b+c)", Unidad = "%", ValoresReferencia = ">= 40", Activo = true, Orden = 5, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id },
                                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "TOTAL PROGRESIVOS (a+b)", Unidad = "%", ValoresReferencia = ">= 32", Activo = true, Orden = 6, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id }
                            }
                        },

                        // 2.2 AGLUTINACION
                        new Examen
                        {
                            Id = Guid.NewGuid(), Codigo = 0, Nombre = "AGLUTINACION", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 2, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id,
                            SubExamenes = new List<Examen>
                            {
                                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CABEZA-CABEZA", Unidad = "", ValoresReferencia = "Negativo", Activo = true, Orden = 1, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id },
                                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "PIEZA INTERMEDIA-PIEZA INTERMEDIA", Unidad = "", ValoresReferencia = "Negativo", Activo = true, Orden = 2, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id },
                                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "COLA-COLA", Unidad = "", ValoresReferencia = "Negativo", Activo = true, Orden = 3, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id },
                                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "MIXTA", Unidad = "", ValoresReferencia = "Negativo", Activo = true, Orden = 4, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id }
                            }
                        },

                        // 2.3 CÉLULAS REDONDAS
                        new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CÉLULAS REDONDAS (LEUCOCITOS Y CÉLULAS)", Unidad = "/ml Esp", ValoresReferencia = "< 1.0 x 10^6", Activo = true, Orden = 3, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id },

                        // 2.4 VITALIDAD
                        new Examen
                        {
                            Id = Guid.NewGuid(), Codigo = 0, Nombre = "VITALIDAD (Test de Eosina)", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 4, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id,
                            SubExamenes = new List<Examen>
                            {
                                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "ESPERMATOZOIDES VIVOS", Unidad = "%", ValoresReferencia = ">= 58", Activo = true, Orden = 1, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id },
                                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "ESPERMATOZOIDES MUERTOS", Unidad = "%", ValoresReferencia = "", Activo = true, Orden = 2, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id },
                                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "INMÓVILES VIVOS", Unidad = "%", ValoresReferencia = "", Activo = true, Orden = 3, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id },
                                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "INMÓVILES MUERTOS", Unidad = "%", ValoresReferencia = "", Activo = true, Orden = 4, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id }
                            }
                        },

                        // 2.5 MORFOLOGÍA
                        new Examen
                        {
                            Id = Guid.NewGuid(), Codigo = 0, Nombre = "MORFOLOGÍA (Wright - Giemsa)", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 5, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id,
                            SubExamenes = new List<Examen>
                            {
                                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "FORMAS NORMALES", Unidad = "%", ValoresReferencia = ">= 4", Activo = true, Orden = 1, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id },
                                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "ANORMALIDAD DE CABEZA", Unidad = "%", ValoresReferencia = "", Activo = true, Orden = 2, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id },
                                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "ANORMALIDAD DE PIEZA INTERMEDIA", Unidad = "%", ValoresReferencia = "", Activo = true, Orden = 3, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id },
                                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "ANORMALIDAD DE COLA", Unidad = "%", ValoresReferencia = "", Activo = true, Orden = 4, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id }
                            }
                        },

                        // 2.6 CONCENTRACIÓN
                        new Examen
                        {
                            Id = Guid.NewGuid(), Codigo = 0, Nombre = "CONCENTRACIÓN ESPERMÁTICA", Unidad = "", ValoresReferencia = "", Activo = true, Orden = 6, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id,
                            SubExamenes = new List<Examen>
                            {
                                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CONCENTRACIÓN", Unidad = "Esp/ml", ValoresReferencia = ">= 15 x 10^6", Activo = true, Orden = 1, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id },
                                new Examen { Id = Guid.NewGuid(), Codigo = 0, Nombre = "CONTAJE TOTAL", Unidad = "Esp/eyaculado", ValoresReferencia = ">= 39 x 10^6", Activo = true, Orden = 2, EsOpcionalDinamico = false, EspecialidadId = espEspeciales.Id }
                            }
                        }
                    }
                }
            }
        };

        var listaEspeciales = new List<Examen> 
        { 
            espermatograma 
        };
        await SincronizarExamenesRecursivo(context, listaEspeciales, espEspeciales.Id);

        // Guardar todos los cambios de golpe en la base de datos
        await context.SaveChangesAsync();
    }

    private static async Task SincronizarExamenesRecursivo(LabSoftDbContext context, List<Examen> examenesSeed, Guid especialidadId)
    {
        foreach (var seed in examenesSeed)
        {
            // Buscamos por el Código único en lugar del ID, ya que el ID cambia con Guid.NewGuid()
            var dbExamen = await context.Examenes
                .FirstOrDefaultAsync(e => e.Codigo == seed.Codigo && e.EspecialidadId == especialidadId);

            if (dbExamen != null)
            {
                // ACTUALIZA SI EXISTE
                dbExamen.Nombre = seed.Nombre;
                dbExamen.Unidad = seed.Unidad;
                dbExamen.ValoresReferencia = seed.ValoresReferencia;
                dbExamen.ValoresReferenciaM = seed.ValoresReferenciaM;
                dbExamen.ValoresReferenciaF = seed.ValoresReferenciaF;
                dbExamen.DecimalesRequeridos = seed.DecimalesRequeridos;
                dbExamen.EsOpcionalDinamico = seed.EsOpcionalDinamico;
                dbExamen.Orden = seed.Orden;
                dbExamen.Activo = seed.Activo;
                context.Examenes.Update(dbExamen);
            }
            else
            {
                // CREA SI NO EXISTE
                // Para evitar errores de rastreo con Entity Framework si el GUID cambia
                seed.Id = Guid.NewGuid(); 
                await context.Examenes.AddAsync(seed);
            }

            // Sincroniza los hijos recursivamente
            if (seed.SubExamenes != null && seed.SubExamenes.Any())
            {
                // A los hijos que tienen código 0, los buscaremos por nombre y padre
                foreach (var subSeed in seed.SubExamenes)
                {
                    var dbSubExamen = await context.Examenes
                        .FirstOrDefaultAsync(e => e.Nombre == subSeed.Nombre && e.EspecialidadId == especialidadId);
                    
                    if (dbSubExamen != null)
                    {
                        dbSubExamen.Unidad = subSeed.Unidad;
                        dbSubExamen.ValoresReferencia = subSeed.ValoresReferencia;
                        dbSubExamen.ValoresReferenciaM = subSeed.ValoresReferenciaM; 
                        dbSubExamen.ValoresReferenciaF = subSeed.ValoresReferenciaF;
                        dbSubExamen.DecimalesRequeridos = subSeed.DecimalesRequeridos;
                        dbSubExamen.EsOpcionalDinamico = subSeed.EsOpcionalDinamico;
                        dbSubExamen.Orden = subSeed.Orden;
                        context.Examenes.Update(dbSubExamen);
                    }
                    else
                    {
                        subSeed.Id = Guid.NewGuid();
                        await context.Examenes.AddAsync(subSeed);
                    }
                }
            }
        }
    }

    private static async Task<Especialidad> ObtenerOCrearEspecialidad(LabSoftDbContext context, int codigo, string nombre)
    {
        var especialidad = await context.Especialidades.FirstOrDefaultAsync(e => e.Codigo == codigo);
        if (especialidad == null)
        {
            especialidad = new Especialidad { Id = Guid.NewGuid(), Codigo = codigo, Nombre = nombre, Activa = true };
            await context.Especialidades.AddAsync(especialidad);
        }
        return especialidad;
    }

}