namespace LabSoft.Domain.Entities;

public class Especialidad
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Activa { get; set; } = true;

    // Relación: Una especialidad tiene muchos exámenes
    public ICollection<Examen> Examenes { get; set; } = new List<Examen>();
}