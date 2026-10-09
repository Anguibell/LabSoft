namespace LabSoft.Application.DTOs;

// DTO para listar paneles con sus exámenes detallados
public class PanelDto
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Activo { get; set; }
    public List<ExamenDto> Examenes { get; set; } = new();
}

// DTO pequeño para mostrar solo lo básico del examen dentro del panel
public class ExamenSimpleDto
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

// DTO para CREAR o ACTUALIZAR un panel (recibiendo IDs)
public class CreateUpdatePanelDto
{
    public int Codigo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public List<Guid> ExamenIds { get; set; } = new();
}