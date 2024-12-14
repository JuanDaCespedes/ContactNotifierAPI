FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiamos la solución y archivos de proyecto
COPY ContactNotifierAPI.sln ./
COPY ./src/Domain/Domain.csproj ./src/Domain/
COPY ./src/Application/Application.csproj ./src/Application/
COPY ./src/Infrastructure/Infrastructure.csproj ./src/Infrastructure/
COPY ./src/WebApi/WebApi.csproj ./src/WebApi/

RUN dotnet restore

# Copiamos todo el código
COPY . .

RUN dotnet publish ./src/WebApi/WebApi.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
# Seteamos el entorno en Development para que muestre Swagger
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "WebApi.dll"]