namespace LabSoft.Infrastructure.Interfaces
{
    public interface IPdfService
    {
        (byte[] PdfBytes, string FileName) GenerarReporteResultadosPdf(Guid ordenId);
    }
}