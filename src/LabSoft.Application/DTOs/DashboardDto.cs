namespace LabSoft.Application.DTOs;

public class DashboardDto
{
    public int TotalPacientes { get; set; }
    public int OrdenesHoy { get; set; }
    public int OrdenesPendientes { get; set; }
    public int ResultadosValidadosMes { get; set; }
    public List<OrdenRecienteDto> OrdenesRecientes { get; set; } = new();
}

public class OrdenRecienteDto
{
    public Guid Id { get; set; }
    public long NumeroOrden { get; set; }
    public string Paciente { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public string Estado { get; set; } = string.Empty;
}