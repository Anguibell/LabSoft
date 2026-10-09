/**
 * Evalúa si un valor numérico está fuera de su rango de referencia.
 * Soporta formatos como: "70 - 100", "70.5 al 100.2", "< 200", "> 50".
 */
export const isOutOfRange = (valor: string | undefined, rangoRef: string | undefined): boolean => {
  if (!valor || !rangoRef) return false;

  // Reemplazamos comas por puntos por si el usuario escribe "95,5"
  const valNum = parseFloat(valor.replace(',', '.'));
  if (isNaN(valNum)) return false; // Si el resultado es texto ("Positivo", "Rojo"), no lo evaluamos numéricamente

  // 1. Caso Rango normal: "70 - 100" o "70 al 100"
  const rangeMatch = rangoRef.match(/([-+]?[\d.,]+)\s*(?:-|al)\s*([-+]?[\d.,]+)/);
  if (rangeMatch) {
    const min = parseFloat(rangeMatch[1].replace(',', '.'));
    const max = parseFloat(rangeMatch[2].replace(',', '.'));
    return valNum < min || valNum > max;
  }

  // 2. Caso Menor que: "< 200" o "<= 200"
  const lessMatch = rangoRef.match(/<[=]?\s*([-+]?[\d.,]+)/);
  if (lessMatch) {
    const max = parseFloat(lessMatch[1].replace(',', '.'));
    return valNum >= max;
  }

  // 3. Caso Mayor que: "> 50" o ">= 50"
  const greaterMatch = rangoRef.match(/>[=]?\s*([-+]?[\d.,]+)/);
  if (greaterMatch) {
    const min = parseFloat(greaterMatch[1].replace(',', '.'));
    return valNum <= min;
  }

  return false;
};