# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["HRMSystem/HRMSystem.csproj", "HRMSystem/"]
RUN dotnet restore "HRMSystem/HRMSystem.csproj"






# Copy everything else and build
COPY . .
WORKDIR "/src/HRMSystem"
RUN dotnet build "HRMSystem.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "HRMSystem.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create non-root user
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser:appuser /app
USER appuser

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "HRMSystem.dll"]
