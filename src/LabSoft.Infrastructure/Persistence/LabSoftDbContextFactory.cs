using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LabSoft.Infrastructure.Persistence;

public class LabSoftDbContextFactory : IDesignTimeDbContextFactory<LabSoftDbContext>
{
    public LabSoftDbContext CreateDbContext(string[] args)
    {
        // Ubicar el archivo appsettings.json del proyecto API para leer la cadena de conexión
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../LabSoft.API"))
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<LabSoftDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        builder.UseNpgsql(connectionString);

        return new LabSoftDbContext(builder.Options);
    }
}