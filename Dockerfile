# --- ETAPA DE COMPILACIÓN ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiamos todo el código fuente de la solución
COPY . .

# Publicamos la API apuntando al archivo .csproj exacto
RUN dotnet publish src/LabSoft.API/LabSoft.API.csproj -c Release -o /app/publish

# --- ETAPA DE EJECUCIÓN ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Render asigna un puerto dinámico mediante la variable PORT
ENV ASPNETCORE_URLS=http://+:$PORT
EXPOSE 8080

# Comando de inicio del servidor
ENTRYPOINT ["dotnet", "LabSoft.API.dll"]