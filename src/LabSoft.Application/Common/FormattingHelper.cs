using System.Globalization;

namespace LabSoft.Application.Common;

public static class FormattingHelper
{
    // Normaliza textos a MAYÚSCULAS y sin espacios sobrantes
    public static string NormalizarTexto(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;
        return input.Trim().ToUpperInvariant();
    }

    // Uniformiza los valores de resultados: decimales con PUNTO, texto en mayúsculas
    public static string NormalizarValorResultado(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        string trimmed = input.Trim();

        // Si el usuario ingresó un número (ya sea con coma o con punto), 
        // lo estandarizamos y formateamos estrictamente usando el PUNTO.
        if (decimal.TryParse(trimmed.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var numero))
        {
            return numero.ToString("0.0", CultureInfo.InvariantCulture);
        }

        // Si es texto cualitativo (ej: "positivo", "aspecto diarreico"), va en MAYÚSCULAS
        return trimmed.ToUpperInvariant();
    }
}