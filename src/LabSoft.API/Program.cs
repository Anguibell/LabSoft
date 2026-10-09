using Microsoft.EntityFrameworkCore;
using LabSoft.Infrastructure.Persistence;
using LabSoft.Infrastructure.Interfaces;
using LabSoft.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LabSoft API", Version = "v1" });

    // ESTO ES LO QUE HACE APARECER EL BOTÓN DE AUTHORIZE EN SWAGGER:
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Ejemplo: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// 1. REGISTRAR EL DBCONTEXT DE POSTGRESQL AQUí:
builder.Services.AddDbContext<LabSoftDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Registrar el Repositorio que hicimos antes:
//DependencyInjection 
builder.Services.AddScoped<LabSoft.Application.Interfaces.IPacienteRepository, LabSoft.Infrastructure.Repositories.PacienteRepository>();
builder.Services.AddScoped<LabSoft.Application.Interfaces.IExamenRepository, LabSoft.Infrastructure.Repositories.ExamenRepository>();
builder.Services.AddScoped<LabSoft.Application.Interfaces.IPanelRepository, LabSoft.Infrastructure.Repositories.PanelRepository>();
// Registro de Repositorio y Servicio de Ordenes
builder.Services.AddScoped<LabSoft.Application.Interfaces.IOrdenRepository, LabSoft.Infrastructure.Repositories.OrdenRepository>();
builder.Services.AddScoped<LabSoft.Application.Interfaces.IOrdenService, LabSoft.Application.Services.OrdenService>();
// Registro de Repositorio y Servicio de Resultados
builder.Services.AddScoped<LabSoft.Application.Interfaces.IResultadoRepository, LabSoft.Infrastructure.Repositories.ResultadoRepository>();
builder.Services.AddScoped<LabSoft.Application.Interfaces.IResultadoService, LabSoft.Application.Services.ResultadoService>();
//SERVICIOS PARA EL MODULO DE REPORTES DE RESULTADOS EN PDF (IMPRESION)
builder.Services.AddScoped<IPdfService, PdfService>();
//SERVICIOS PARA EL TOKEN EN EL MODULO USUARIO
builder.Services.AddScoped<LabSoft.Application.Interfaces.ITokenService, TokenService>();
//SERVICIOS PARA EL USUARIO
builder.Services.AddScoped<LabSoft.Application.Interfaces.IUsuarioService, LabSoft.Application.Services.UsuarioService>();
builder.Services.AddScoped<LabSoft.Application.Interfaces.IUsuarioRepository, LabSoft.Infrastructure.Repositories.UsuarioRepository>();
//SERVICCIOS PARA EL DASHBOARD
builder.Services.AddScoped<LabSoft.Application.Interfaces.IDashboardService, LabSoft.Application.Services.DashboardService>();
builder.Services.AddScoped<LabSoft.Application.Interfaces.IDashboardRepository, LabSoft.Infrastructure.Repositories.DashboardRepository>();
//SERVICIOS PARA AUDITORIA
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<LabSoft.Application.Interfaces.ICurrentUserService, LabSoft.API.Services.CurrentUserService>();

//CONFIGURACION DE LA AUTENTICACION
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]!)),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidateAudience = true, 
            ValidAudience = builder.Configuration["JwtSettings:Audience"], // <-- VALIDAMOS QUE COINCIDA
            ClockSkew = TimeSpan.Zero // Opcional: elimina los 5 minutos de gracia por defecto en la expiración
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Esto le dice a C# que convierta los Enums a texto ("Masculino") en lugar de números (0)
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// REGISTRO MODERNO DE FLUENTVALIDATION
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// Esto busca automáticamente todos los validadores en el proyecto de Application:
builder.Services.AddValidatorsFromAssemblyContaining<LabSoft.Application.Validators.RegistrarUsuarioValidator>();

// 1. AGREGAR ESTO: Configurar política de CORS
// PARA PROBAR FRONTEND LUEGO QUITARLOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
// QUITAR ESTA Y LA LINEA DE ABAJOOOOO app.UseCors("AllowFrontend");
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp", policy =>
    {
        policy.WithOrigins("https://lab-soft.vercel.app") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// <-- REGISTRAMOS EL MIDDLEWARE
app.UseMiddleware<LabSoft.API.Middlewares.ExceptionHandlingMiddleware>();

// Bloque para ejecutar los Seeders automáticamente al arrancar
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<LabSoftDbContext>();
        
        // 1. Primero poblamos los exámenes y especialidades base
        await LabSoft.Infrastructure.Seeders.DbInitializer.InitializeAsync(context);
        
        // 2. Después poblamos los paneles (perfiles) utilizando los exámenes ya creados
        await LabSoft.Infrastructure.Seeders.PanelSeeder.InitializeAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al poblar la base de datos.");
    }
}

// Configure the HTTP request pipeline.
// Configuración de middlewares, Swagger, etc.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 1. FORZAR HTTPS OBLIGATORIO (Redirige cualquier petición HTTP a HTTPS)
app.UseHttpsRedirection();

// AGREGAR ESTO PARA PROBAR FRONTEND LUEGO QUITARLOOOOOOOOOOOOOOOOO
// QUITAR ESTA Y LA LINEA DE ARRIBAAAAAAAAAAAAAAAA builder.Services.AddCors
app.UseCors("AllowWebApp");

// 2. ORDEN DE SEGURIDAD (Primero autenticar quién es, luego autorizar qué puede hacer)
app.UseAuthentication();
app.UseAuthorization();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapControllers();
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}