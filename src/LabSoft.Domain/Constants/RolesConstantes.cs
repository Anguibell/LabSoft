namespace LabSoft.Domain.Constants;

public static class RolesConstantes
{
    public const string ADMINISTRADOR = "ADMINISTRADOR";
    public const string BIOANALISTA = "BIOANALISTA";
    public const string RECEPCIONISTA = "RECEPCIONISTA";
    public const string AUXILIAR = "AUXILIAR";

    // Método útil para validar si un rol enviado desde el exterior es válido
    public static readonly string[] Todos = { ADMINISTRADOR, BIOANALISTA, RECEPCIONISTA, AUXILIAR };

    public static bool EsValido(string rol)
    {
        if (string.IsNullOrWhiteSpace(rol)) return false;
        return Todos.Contains(rol.Trim().ToUpper());
    }
}