using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using LabSoft.Infrastructure.Persistence;
using LabSoft.Infrastructure.Interfaces;
using System.Globalization;
using QRCoder;
using System.Linq; 

namespace LabSoft.Infrastructure.Services
{
    public class PdfService : IPdfService
    {
        private readonly LabSoftDbContext _context;

        public PdfService(LabSoftDbContext context)
        {
            _context = context;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public (byte[] PdfBytes, string FileName) GenerarReporteResultadosPdf(Guid ordenId)
        {
            var orden = _context.Ordenes
                .Include(o => o.Paciente)
                .Include(o => o.Detalles)
                    .ThenInclude(d => d.Examen)
                        .ThenInclude(e => e.Especialidad)
                .Include(o => o.Detalles)
                    .ThenInclude(d => d.Examen)
                        .ThenInclude(e => e.SubExamenes) 
                            .ThenInclude(sub1 => sub1.SubExamenes) 
                                .ThenInclude(sub2 => sub2.SubExamenes) 
                .Include(o => o.Detalles)
                    .ThenInclude(d => d.Resultados)
                .FirstOrDefault(o => o.Id == ordenId);

            if (orden == null)
            {
                throw new Exception("La orden especificada no existe para generar el PDF.");
            }

            // Extracción de datos del paciente
            var pacienteNombre = orden.Paciente != null ? $"{orden.Paciente.Nombre} {orden.Paciente.Apellido}" : "N/A";

            // Manejo de la cédula considerando la bandera de recién nacido
            var pacienteCedula = "N/A";
            if (orden.Paciente != null)
            {
                pacienteCedula = orden.Paciente.EsRecienNacido 
                    ? "Recién Nacido" 
                    : (string.IsNullOrWhiteSpace(orden.Paciente.Cedula) ? "N/A" : orden.Paciente.Cedula);
            }

            // Conversión directa del Enum a string
            var pacienteSexo = orden.Paciente != null ? orden.Paciente.Sexo.ToString() : "N/A";

            // Uso de la propiedad calculada nativa de tu entidad
            var edad = orden.Paciente != null ? orden.Paciente.EdadFormateada : "N/A";

            // Datos de la orden
            var nroOrden = orden.NumeroOrden.ToString("D6");
            var fechaRealizacion = orden.FechaCreacion.ToString("dd/MM/yyyy");

            // Creamos esta específicamente para el nombre del archivo (sin barras)
            var fechaArchivo = orden.FechaCreacion.ToString("dd-MM-yyyy");
            var nombreCompleto = $"{orden.Paciente?.Nombre} {orden.Paciente?.Apellido}".ToUpperInvariant();
    
            var fileName = $"RESULTADO {nroOrden} {nombreCompleto} {fechaArchivo}.pdf";

            // Usando el directorio base donde se compila la API
            // Rutas de las imágenes
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var logoPath = Path.Combine(basePath, "Assets", "images", "logo.svg");
            var firmaPath = Path.Combine(basePath, "Assets", "images", "firmaYasmin.svg");
            var selloBioanalistaPath = Path.Combine(basePath, "Assets", "images", "selloYasmin.svg");
            var selloLabPath = Path.Combine(basePath, "Assets", "images", "selloLab.svg");

            // Define la URL (luego la cambiarás por el dominio real de tu frontend en React)
            string urlValidacion = $"https://labgenesis.com/validar/{orden.Id}";
            byte[] qrImageBytes = GenerarQrValidacion(urlValidacion);

            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);

                    // ==========================================
                    // MARCA DE AGUA (FONDO)
                    // ==========================================
                    page.Background().AlignCenter().AlignMiddle().Container().Width(400).Element(c => 
                    {
                        if (File.Exists(logoPath))
                        {
                            var svgLogo = File.ReadAllText(logoPath);
                            // Truco: Inyectamos un estilo de opacidad del 5% directamente en la etiqueta principal del SVG
                            if(svgLogo.Contains("<svg")) 
                            {
                                svgLogo = svgLogo.Replace("<svg", "<svg opacity=\"0.05\"");
                            }
                            c.Svg(svgLogo).FitArea();
                        }
                    });

                    // Cabecera del reporte con Logo
                    page.Header().Row(row =>
                    {
                        // Columna Izquierda: Logo (fijamos 50 de ancho)
                        if (File.Exists(logoPath))
                        {
                            var svgLogo = File.ReadAllText(logoPath);
                            row.ConstantItem(50).AlignMiddle().Svg(svgLogo).FitArea();
                        }
                        else
                        {
                            row.ConstantItem(50); // Mantiene el equilibrio si falta el logo
                        }

                        // Columna Central: Texto del membrete
                        row.RelativeItem().AlignMiddle().Column(header =>
                        {
                            header.Item().AlignCenter().Text("LABORATORIO CLINICO ¨GENESIS¨").FontSize(18).Bold().FontColor(Colors.Blue.Darken3);
                            header.Item().AlignCenter().Text("RIF NO. J-31283999-6").FontSize(10).SemiBold().FontColor(Colors.Grey.Darken3);
                            header.Item().AlignCenter().Text("Sector Noriega Trigo, Avenida 21, Casa N° 24-161").FontSize(9).FontColor(Colors.Grey.Darken2);
                            header.Item().AlignCenter().Text("(Frente a la Emergencia del Hospital Noriega Trigo), San Francisco, Edo Zulia").FontSize(9).FontColor(Colors.Grey.Darken2);
                        });

                        // Columna Derecha: Espacio vacío "fantasma" con el mismo ancho del logo (50)
                        // Esto es lo que logra el centrado perfecto del texto respecto a la página.
                        row.ConstantItem(50).AlignMiddle().Image(qrImageBytes);
                    });
                    
                    // Cuerpo / Contenido dinámico
                    page.Content().Column(column =>
                    {
                        // Datos del Paciente
                        column.Item().PaddingTop(15).Background(Colors.Grey.Lighten4).Padding(10).Row(row =>
                        {
                            // Columna Izquierda (Datos del paciente)
                            row.RelativeItem().Column(info =>
                            {
                                info.Item().Text(text => { text.Span("Paciente: ").SemiBold(); text.Span(pacienteNombre); });
                                info.Item().Text(text => { text.Span("Cédula: ").SemiBold(); text.Span(pacienteCedula); });
                                info.Item().Text(text => { text.Span("Sexo: ").SemiBold(); text.Span(pacienteSexo); });
                                info.Item().Text(text => { text.Span("Edad: ").SemiBold(); text.Span(edad); });
                            });

                            // Columna Derecha (Datos de la Orden)
                            row.RelativeItem().AlignRight().Column(info =>
                            {
                                info.Item().Text(text => { text.Span("Nro de Orden: ").SemiBold(); text.Span(nroOrden).FontColor(Colors.Red.Medium); });
                                info.Item().Text(text => { text.Span("Fecha: ").SemiBold(); text.Span(fechaRealizacion); });
                            });
                        });

                        // Tabla de Resultados Dinámica
                        column.Item().PaddingTop(10).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2); // Examen / Parámetro
                                columns.RelativeColumn(1); // Resultado
                                columns.RelativeColumn(1); // Unidad
                                columns.RelativeColumn(2); // Valores de Referencia
                            });

                            // Cabecera de la tabla
                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderCellStyle).AlignLeft().Text("EXAMEN / PARAMETRO");
                                header.Cell().Element(HeaderCellStyle).AlignCenter().Text("RESULTADO");
                                header.Cell().Element(HeaderCellStyle).AlignCenter().Text("UNIDAD");
                                header.Cell().Element(HeaderCellStyle).AlignCenter().Text("VALORES DE REFERENCIA");
                            });

                            var detallesPorEspecialidad = orden.Detalles
                                .Where(d => d.Examen != null)
                                .GroupBy(d => d.Examen.Especialidad?.Nombre ?? "GENERAL")
                                .OrderBy(g => g.Min(d => d.Examen.Codigo))
                                .ToList();

                            foreach (var grupoEspecialidad in detallesPorEspecialidad)
                            {

                                // Título de Especialidad 
                                table.Cell().ColumnSpan(4)
                                    .PaddingTop(10) // Espacio EXTERNO: Separa este bloque del examen anterior
                                    .BorderBottom(1).BorderColor(Colors.Blue.Medium) // BORDE: Envuelve todo hacia abajo
                                    .Background(Colors.Blue.Lighten5) // FONDO: Queda exactamente al ras del borde
                                    .PaddingVertical(4) // Espacio INTERNO: Le da aire al texto para que no pegue con las líneas
                                    .Text(grupoEspecialidad.Key.ToUpper())
                                    .AlignCenter().Bold().FontSize(11).FontColor(Colors.Blue.Darken3);

                                var detallesOrdenados = grupoEspecialidad.OrderBy(d => d.Examen.Codigo).ToList();

                                foreach (var detalle in detallesOrdenados)
                                {
                                    var examenPrincipal = detalle.Examen;
                                    if (examenPrincipal == null) continue;

                                    // Función auxiliar centralizada para limpiar y formatear los resultados (decimales y mayúsculas)
                                    string FormatearValor(string? valorRaw, int? decimalesRequeridos)
                                    {
                                        if (string.IsNullOrWhiteSpace(valorRaw)) return "";

                                        var valStr = valorRaw.Trim().Replace(',', '.');

                                        // Si es un número válido, aplicamos el formato
                                        if (decimal.TryParse(valStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var valorNum))
                                        {
                                            if (decimalesRequeridos.HasValue)
                                            {
                                                // Genera dinámicamente el formato (ej. "F1" para 1 decimal, "F0" para 0)
                                                return valorNum.ToString($"F{decimalesRequeridos.Value}", CultureInfo.InvariantCulture);
                                            }
                                            
                                            // Si no hay configuración en la BD, devuelve el número sin forzar ceros extra
                                            return valorNum.ToString(CultureInfo.InvariantCulture); 
                                        }

                                        // Si es texto (ej. "POSITIVO"), lo devuelve tal cual
                                        return valorRaw;
                                    }

                                    // Función auxiliar para evaluar si el resultado está fuera del rango de referencia
                                    bool EsAnormal(string? valorRaw, string? referencia)
                                    {
                                        if (string.IsNullOrWhiteSpace(valorRaw) || string.IsNullOrWhiteSpace(referencia)) return false;
                                        
                                        var valStr = valorRaw.Trim().Replace(',', '.');
                                        var refStr = referencia.Trim().Replace(',', '.');

                                        // 1. EVALUACIÓN NUMÉRICA
                                        if (decimal.TryParse(valStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var valorNum))
                                        {
                                            if (refStr.Contains("-"))
                                            {
                                                var partes = refStr.Split('-');
                                                if (partes.Length == 2 && 
                                                    decimal.TryParse(partes[0].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var min) && 
                                                    decimal.TryParse(partes[1].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var max))
                                                {
                                                    return valorNum < min || valorNum > max; 
                                                }
                                            }
                                            
                                            if (refStr.StartsWith("<") && decimal.TryParse(refStr.Replace("<", "").Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var maxVal))
                                                return valorNum >= maxVal;

                                            if (refStr.StartsWith(">") && decimal.TryParse(refStr.Replace(">", "").Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var minVal))
                                                return valorNum <= minVal;
                                                
                                            return false;
                                        }

                                        // 2. EVALUACIÓN DE TEXTO
                                        var valUpper = valStr.ToUpperInvariant();
                                        var refUpper = refStr.ToUpperInvariant();

                                        // A. Igualdad exacta (Cubre "Amarillo" == "Amarillo", "Sui-géneris" == "Sui-géneris")
                                        if (valUpper == refUpper) return false;

                                        // B. Ausencia vs Presencia
                                        string[] referenciasNegativas = { "NEGATIVO", "NO REACTIVO", "NO SE OBSERVA", "NO SE OBSERVAN", "AUSENTE" };
                                        if (referenciasNegativas.Any(r => refUpper.Contains(r)))
                                        {
                                            // Si el resultado incluye palabras que indican positividad o presencia, es anormal
                                            string[] marcadoresPositivos = { "POSITIVO", "REACTIVO", "SE OBSERVA", "MODERADAS", "ABUNDANTES", "PRESENTE" };
                                            if (marcadoresPositivos.Any(p => valUpper.Contains(p))) return true;
                                            
                                            // Si es un número (ej. Referencia "Negativo", Resultado "5"), es anormal
                                            if (valUpper.Any(char.IsDigit)) return true;
                                        }

                                        // C. Escalas de Cantidad (Referencia baja vs Resultado alto)
                                        if (refUpper.Contains("ESCASAS") || refUpper.Contains("AUSENTES"))
                                        {
                                            if (valUpper.Contains("MODERADAS") || valUpper.Contains("ABUNDANTES") || valUpper.Contains("NUMEROSAS"))
                                                return true;
                                        }

                                        // D. Atributos descriptivos estrictos (Si esperaba "LÍMPIDO" y llegó "TURBIO")
                                        // Se ignoran diferencias de espacios o guiones básicos
                                        if (valUpper.Replace(" ", "") != refUpper.Replace(" ", "")) 
                                        {
                                            // Solo marcamos como anormal si realmente difieren sustancialmente, 
                                            // asumiendo que los valores cualitativos no coincidentes (ej. Aspecto) son un hallazgo patológico.
                                            return true; 
                                        }

                                        return false;
                                    }

                                    // Evaluamos si es un examen simple o compuesto
                                    bool esCompuesto = examenPrincipal.SubExamenes != null && examenPrincipal.SubExamenes.Any();

                                    if (esCompuesto)
                                    {
                                        // CASO A: Examen Compuesto (Ej: HEMATOLOGIA COMPLETA). Se imprime como encabezado.
                                        table.Cell().Element(c => CellStyle(c, 0)).Text(examenPrincipal.Nombre).Bold();
                                        table.Cell().Element(c => CellStyle(c, 0)).Text(""); 
                                        table.Cell().Element(c => CellStyle(c, 0)).Text(""); 
                                        table.Cell().Element(c => CellStyle(c, 0)).Text(""); 

                                        // Función recursiva nativa para los hijos
                                        void PintarExamenesRecursivos(IEnumerable<LabSoft.Domain.Entities.Examen> subExamenes, int nivel)
                                        {
                                            var subExamenesOrdenados = subExamenes.OrderBy(s => s.Orden);

                                            foreach (var sub in subExamenesOrdenados)
                                            {
                                                var resultadoAsociado = detalle.Resultados?.FirstOrDefault(r => r.ExamenId == sub.Id);
                                                var valorRaw = resultadoAsociado?.ValorResultado;

                                                if (sub.EsOpcionalDinamico && string.IsNullOrWhiteSpace(valorRaw))
                                                    continue;

                                                var valorFormateado = FormatearValor(valorRaw, sub.DecimalesRequeridos);
                                                var unidad = !string.IsNullOrEmpty(sub.Unidad) ? sub.Unidad : "";

                                                // 1. Tomamos el valor general por defecto (si existe)
                                                string refValores = sub.ValoresReferencia ?? ""; 

                                                // 2. Evaluamos el sexo del paciente
                                                var sexoPaciente = orden.Paciente?.Sexo.ToString().ToUpper().Trim() ?? "";

                                                // 3. Sobreescribimos si hay un valor específico para su sexo
                                                if (sexoPaciente == "MASCULINO" && !string.IsNullOrWhiteSpace(sub.ValoresReferenciaM))
                                                {
                                                    refValores = sub.ValoresReferenciaM;
                                                }
                                                else if (sexoPaciente == "FEMENINO" && !string.IsNullOrWhiteSpace(sub.ValoresReferenciaF))
                                                {
                                                    refValores = sub.ValoresReferenciaF;
                                                }

                                                if (sub.SubExamenes != null && sub.SubExamenes.Any())
                                                {
                                                    // Título de categoría interna (Ej: EXAMEN MACROSCOPICO)
                                                    table.Cell().Element(c => SubCellStyle(c, nivel)).Text($"> {sub.Nombre}").SemiBold().FontColor(Colors.Grey.Darken3);
                                                    table.Cell().Element(c => SubCellStyle(c, 0)).Text(""); 
                                                    table.Cell().Element(c => SubCellStyle(c, 0)).Text(""); 
                                                    table.Cell().Element(c => SubCellStyle(c, 0)).Text(""); 

                                                    PintarExamenesRecursivos(sub.SubExamenes, nivel + 1);
                                                }
                                                else
                                                {
                                                    // Parámetro final hijo (Ej: GLUCOSA)
                                                    table.Cell().Element(c => SubCellStyle(c, nivel)).Text(sub.Nombre);
                                                    
                                                    // 1. Evaluamos la anormalidad usando los valores de referencia del sub-examen
                                                    bool esAnormal = EsAnormal(valorRaw, refValores);
                                                    var textoResultado = esAnormal ? $"* {valorFormateado} *" : valorFormateado;

                                                    // 2. Imprimimos la celda de resultado de forma dinámica
                                                    var celdaResultado = table.Cell().Element(c => SubCellStyle(c, 0)).AlignCenter().Text(textoResultado);

                                                    if (esAnormal)
                                                    {
                                                        celdaResultado.FontColor(Colors.Red.Medium).Bold();
                                                    }

                                                    // 3. Imprimimos el resto de las columnas (Unidad y Referencia)
                                                    table.Cell().Element(c => SubCellStyle(c, 0)).AlignCenter().Text(unidad);
                                                    table.Cell().Element(c => SubCellStyle(c, 0)).AlignCenter().Text(refValores);

                                                    // 4. Imprimimos la nota clínica (Caso A)
                                                    var notaClinica = resultadoAsociado?.NotaClinica;
                                                    var imprimirNota = resultadoAsociado?.ImprimirObservacion ?? false;

                                                    if (imprimirNota && !string.IsNullOrWhiteSpace(notaClinica))
                                                    {
                                                        table.Cell().ColumnSpan(4)
                                                            .PaddingLeft((nivel * 12) + 15) // Aquí usamos 'nivel' que es la variable correcta
                                                            .PaddingBottom(4)
                                                            .Text($"*Nota: {notaClinica}*")
                                                            .FontSize(8)
                                                            .Italic()
                                                            .FontColor(Colors.Grey.Darken2);
                                                    }
                                                }
                                            }
                                        }

                                        // Llamamos a la recursividad para imprimir todos los hijos
                                        PintarExamenesRecursivos(examenPrincipal.SubExamenes!, 1);
                                    }
                                    else
                                    {
                                        // CASO B: Examen Simple (Ej: GLICEMIA BASAL). El resultado se imprime directamente en su fila padre.
                                        var resultadoAsociado = detalle.Resultados?.FirstOrDefault(r => r.ExamenId == examenPrincipal.Id);
                                        var valorRaw = resultadoAsociado?.ValorResultado;
                                        var valorFormateado = FormatearValor(valorRaw, examenPrincipal.DecimalesRequeridos);
                                        
                                        var unidad = !string.IsNullOrEmpty(examenPrincipal.Unidad) ? examenPrincipal.Unidad : "";

                                        // 1. Tomamos el valor general por defecto (si existe)
                                        string refValores = examenPrincipal.ValoresReferencia ?? ""; 

                                        // 2. Evaluamos el sexo del paciente
                                        var sexoPaciente = orden.Paciente?.Sexo.ToString().ToUpper().Trim() ?? "";

                                        // 3. Sobreescribimos si hay un valor específico para su sexo
                                        if (sexoPaciente == "MASCULINO" && !string.IsNullOrWhiteSpace(examenPrincipal.ValoresReferenciaM))
                                        {
                                            refValores = examenPrincipal.ValoresReferenciaM;
                                        }
                                        else if (sexoPaciente == "FEMENINO" && !string.IsNullOrWhiteSpace(examenPrincipal.ValoresReferenciaF))
                                        {
                                            refValores = examenPrincipal.ValoresReferenciaF;
                                        }

                                        table.Cell().Element(c => CellStyle(c, 0)).Text(examenPrincipal.Nombre).Bold();
                                        
                                        // Evaluamos la anormalidad usando los valores de referencia del examen principal
                                        bool esAnormal = EsAnormal(valorRaw, refValores);
                                        var textoResultado = esAnormal ? $"* {valorFormateado} *" : valorFormateado;

                                        var celdaResultado = table.Cell().Element(c => CellStyle(c, 0)).AlignCenter().Text(textoResultado);
                                        
                                        if (esAnormal)
                                        {
                                            celdaResultado.FontColor(Colors.Red.Medium).Bold();
                                        }

                                        table.Cell().Element(c => CellStyle(c, 0)).AlignCenter().Text(unidad);
                                        table.Cell().Element(c => CellStyle(c, 0)).AlignCenter().Text(refValores);

                                        // 4. Imprimimos la nota clínica (Caso B)
                                        var notaClinica = resultadoAsociado?.NotaClinica;
                                        var imprimirNota = resultadoAsociado?.ImprimirObservacion ?? false;

                                        if (imprimirNota && !string.IsNullOrWhiteSpace(notaClinica))
                                        {
                                            table.Cell().ColumnSpan(4)
                                                .PaddingLeft(15) // Espaciado fijo, ya que no hay 'nivel'
                                                .PaddingBottom(4)
                                                .Text($"*Nota: {notaClinica}*")
                                                .FontSize(8)
                                                .Italic()
                                                .FontColor(Colors.Grey.Darken2);
                                        }
                                    }
                                }
                            }
                        });

                        // ==========================================
                        // FIRMA Y SELLO
                        // ==========================================

                        //column.Item().PaddingTop(40).Row(row =>
                        column.Item().ExtendVertical().AlignBottom().PaddingTop(15).Row(row =>
                        {
                            // ==========================================
                            // COLUMNA IZQUIERDA: Sello del Laboratorio
                            // ==========================================
                            row.RelativeItem().AlignCenter().AlignMiddle().Column(colSelloLab =>
                            {
                                if (File.Exists(selloLabPath))
                                {
                                    var svgSelloLab = File.ReadAllText(selloLabPath);
                                    colSelloLab.Item().Height(80).Svg(svgSelloLab).FitArea();
                                }
                            });

                            // ==========================================
                            // COLUMNA DERECHA: Firma y Sello de Bioanalista
                            // ==========================================
                            row.RelativeItem().AlignCenter().AlignMiddle().Column(colBioanalista =>
                            {
                                // 1. Firma arriba
                                if (File.Exists(firmaPath))
                                {
                                    var svgFirma = File.ReadAllText(firmaPath);
                                    colBioanalista.Item().AlignCenter().Height(50).Svg(svgFirma).FitHeight();
                                }
                                
                                // 2. Sello del bioanalista abajo
                                if (File.Exists(selloBioanalistaPath))
                                {
                                    var svgSelloBio = File.ReadAllText(selloBioanalistaPath);
                                    colBioanalista.Item().AlignCenter().Height(50).Svg(svgSelloBio).FitHeight();
                                }

                                colBioanalista.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Grey.Medium);
                                colBioanalista.Item().AlignCenter().Text("Bioanalista Titular").FontSize(9).FontColor(Colors.Grey.Darken2);
                            });
                        });
                    });

                    // Pie de página
                    page.Footer().Column(footer =>
                    {
                        footer.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                        
                        footer.Item().PaddingTop(5).Row(row =>
                        {
                            // Izquierda: Contacto
                            row.RelativeItem().Text("Tlf: 0424-6408937 | Correo: labgenesis@gmail.com | Ig: @LabGenesis").FontSize(8).FontColor(Colors.Grey.Medium);
                            
                            // Derecha: Paginación y Software
                            row.RelativeItem().AlignRight().Text(x =>
                            {
                                x.Span("Página ").FontSize(8).FontColor(Colors.Grey.Medium);
                                x.CurrentPageNumber().FontSize(8).FontColor(Colors.Grey.Medium);
                                x.Span(" de ").FontSize(8).FontColor(Colors.Grey.Medium);
                                x.TotalPages().FontSize(8).FontColor(Colors.Grey.Medium);
                                x.Span("  |  Software: LabSoft LIMS").FontSize(8).SemiBold().FontColor(Colors.Grey.Medium);
                            });
                        });
                    });
                });
            }).GeneratePdf();

            return (pdfBytes, fileName);
        }

        // Estilos auxiliares para las celdas de la tabla
        static IContainer HeaderCellStyle(IContainer container) =>
            container
                .Background(Colors.Blue.Darken2) // Fondo sólido azul oscuro
                .PaddingVertical(6) 
                .PaddingHorizontal(8)
                .DefaultTextStyle(x => x.Bold().FontSize(10).FontColor(Colors.White)); // Texto blanco

        // Modificamos CellStyle y SubCellStyle para aceptar el nivel de indentación
        static IContainer CellStyle(IContainer container, int indentLevel) =>
            container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                    .PaddingVertical(3) // Reducido para compactar la altura de las filas
                    .PaddingHorizontal(5)
                    .PaddingLeft(indentLevel * 12) // Aplica la sangría de forma nativa en el PDF
                    .DefaultTextStyle(x => x.FontSize(10));

        static IContainer SubCellStyle(IContainer container, int indentLevel) =>
            container.BorderBottom(1).BorderColor(Colors.Grey.Lighten4)
                    .PaddingVertical(2) // Reducido para compactar la altura de las filas
                    .PaddingHorizontal(5)
                    .PaddingLeft(indentLevel * 12) // Aplica la sangría de forma nativa en el PDF
                    .DefaultTextStyle(x => x.FontSize(9));

        private byte[] GenerarQrValidacion(string url)
        {
            using var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);
            return qrCode.GetGraphic(5); // El número define la resolución/tamaño de los píxeles
        }
    }
}