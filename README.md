# ContactNotifierAPI

Este proyecto es una API construida con ASP.NET Core (.NET 8) que permite:

- Recibir datos de contacto (nombre y número de teléfono) vía POST.
- Guardar estos datos en una base de datos PostgreSQL.
- Simular el envío de un mensaje de bienvenida a dichos contactos, registrando en logs el envío.
- Consultar la lista de contactos guardados.

La arquitectura sigue principios de **Clean Architecture**, aplicando **DDD**, **CQRS** con **MediatR**, y separación de responsabilidades. Para la persistencia, se utiliza EF Core para las operaciones de escritura, y se muestra un ejemplo de lectura con Dapper. El proyecto está preparado para ejecutarse localmente o dentro de contenedores Docker, junto con Docker Compose para orquestar el contenedor de la base de datos y la propia API.

## Tecnologías Utilizadas

- **.NET 8**: Última versión del framework para crear aplicaciones web de alta performance.
- **ASP.NET Core**: Framework para construir servicios HTTP y microservicios robustos.
- **Clean Architecture / DDD / CQRS**: Patrones y principios que facilitan la mantenibilidad, escalabilidad y testeo.
- **MediatR**: Librería para implementar CQRS (Commands y Queries) de manera sencilla.
- **Entity Framework Core**: ORM para operaciones de escritura.  
- **Dapper**: Micro-ORM para operaciones de lectura, ofreciendo alto rendimiento.
- **PostgreSQL**: Base de datos relacional robusta y open source.
- **Serilog**: Logging estructurado para mayor claridad en logs.
- **Docker & Docker Compose**: Para el despliegue fácil y reproducible en entornos aislados.

## Estructura del Proyecto

```
ContactNotifierAPI/
├─ docker-compose.yml
├─ ContactNotifierAPI.sln
└─ src/
   ├─ Domain/              # Entidades, interfaces de dominio, lógica pura
   ├─ Application/         # Lógica de aplicación, casos de uso (Commands/Queries), DTOs
   ├─ Infrastructure/      # Implementaciones técnicas (EF, Dapper, Repos, Migrations)
   └─ WebApi/              # API ASP.NET Core, Endpoints, Program.cs, Configuración
```

## Requisitos Previos

- **.NET 8 SDK** instalado (para ejecución local).
- **Docker y Docker Compose** instalados (para ejecución en contenedores).
- **PostgreSQL** instalado localmente (opcional, si deseas correr sin Docker).

## Configuración de la Conexión a la Base de Datos

La cadena de conexión se encuentra en `appsettings.json` en el proyecto `WebApi`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=ContactsDb;Username=postgres;Password=postgres"
}
```

- Si ejecutas con Docker Compose, la API se conectará al servicio `db` definido en `docker-compose.yml`.
- Si ejecutas localmente, asegúrate de que PostgreSQL esté corriendo localmente y la cadena de conexión apunte a `Host=localhost` (ajusta usuario/contraseña según tu entorno).

## Ejecución Local (Sin Docker)

1. Clona el repositorio:

2. Restaura paquetes e instala dependencias:
   ```bash
   dotnet restore
   ```

3. Aplica las migraciones (si tienes PostgreSQL corriendo localmente):
   ```bash
   dotnet ef database update -p Infrastructure -s WebApi
   ```
   *(Asegúrate que la cadena de conexión en `WebApi` esté apuntando a tu instancia local de PostgreSQL.)*

4. Ejecuta la API:
   ```bash
   dotnet run --project src/WebApi/WebApi.csproj
   ```

5. Accede a Swagger para probar:
   ```
   http://localhost:5000/swagger
   ```
   
   Podrás hacer `POST /api/contacts` y `GET /api/contacts`.

## Ejecución con Docker y Docker Compose

Este método levanta la API y la base de datos en contenedores separados:

1. Clona el repositorio (si no lo has hecho)

2. Construye e inicia los contenedores:
   ```bash
   docker-compose up --build
   ```
   
   Esto:
   - Levanta un contenedor con PostgreSQL (db)
   - Levanta un contenedor con la API (webapi) en modo Desarrollo
   - Aplica migraciones automáticamente al arrancar la API

3. Accede a la API en:
   ```
   http://localhost:5001/swagger
   ```
   
   *(Suponiendo que has configurado el puerto 5001:8080 en el docker-compose. Ajusta según tu configuración actual.)*

4. Podrás probar los endpoints desde Swagger.

## Ejecución con Docker (Sin Compose)

Si solo quieres crear la imagen de la API y correrla con PostgreSQL externo o local, haz lo siguiente:

1. Crear la imagen:
   ```bash
   docker build -f src/WebApi/Dockerfile -t contactnotifierapi:latest .
   ```
   
2. Ejecutar el contenedor apuntando a una base de datos PostgreSQL existente:
   ```bash
   docker run -p 5001:8080 \
     -e ConnectionStrings__DefaultConnection="Host=mihost;Database=ContactsDb;Username=postgres;Password=postgres" \
     contactnotifierapi:latest
   ```

3. Si la base de datos no está inicializada, deberás aplicar migraciones externamente o añadir código para que se apliquen al arrancar.

## Migraciones de EF Core

- Crear una nueva migración:
  ```bash
  dotnet ef migrations add InitialCreate -p Infrastructure -s WebApi
  ```
  
- Aplicar migraciones a la base de datos:
  ```bash
  dotnet ef database update -p Infrastructure -s WebApi
  ```

Cuando ejecutas con Docker Compose, el código en `Program.cs` (`db.Database.Migrate();`) aplica las migraciones automáticamente al inicio.

## Patrones y Arquitectura

- **Clean Architecture:** Separación clara entre capas (Domain, Application, Infrastructure, WebApi). Esto facilita el mantenimiento y escalabilidad.
- **DDD (Domain-Driven Design):** La capa Domain contiene las Entidades y Lógica de Negocio pura, sin dependencias de infraestructura.
- **CQRS (Command Query Responsibility Segregation):** Commands para operaciones de escritura (a través de EF Core) y Queries para lectura (ejemplo con Dapper). Esto mejora la claridad y escalabilidad.
- **MediatR:** Implementación sencilla de CQRS, eliminando la necesidad de controladores “gordos”. Cada caso de uso se implementa como un Handler.
- **Logging con Serilog:** Para auditoría y debugging, registrando eventos importantes como el "envío" del mensaje de bienvenida.