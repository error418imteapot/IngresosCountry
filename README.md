# Ingresos Country - Sistema de Control de Acceso

Sistema web para la gestión de control de acceso en el Club

## Tecnologías

- **Backend:** ASP.NET Core 8.0 MVC, C#
- **Frontend:** HTML5, CSS3, Bootstrap 5, JavaScript, jQuery
- **Base de Datos:** Microsoft SQL Server
- **Autenticación:** Cookie Authentication con roles

## Arquitectura

Arquitectura de tres capas:

1. **Capa de Presentación** - Views (Razor Pages con Bootstrap 5)
2. **Capa de Lógica de Negocio** - Services (interfaces + implementaciones)
3. **Capa de Acceso a Datos** - Data (SQL Server con Stored Procedures)

## Estructura del Proyecto

```
IngresosCountry/
├── IngresosCountry.sln              # Solución de Visual Studio
├── README.md
└── IngresosCountry/
    ├── IngresosCountry.csproj       # Proyecto ASP.NET Core
    ├── Program.cs                    # Configuración de la aplicación
    ├── appsettings.json             # Cadena de conexión
    ├── Controllers/                  # Controladores MVC + API
    │   ├── AccountController.cs     # Login/Logout
    │   ├── HomeController.cs        # Dashboard
    │   ├── SociosController.cs      # CRUD Socios
    │   ├── InvitadosController.cs   # CRUD Invitados/Invitaciones
    │   ├── VisitantesController.cs  # CRUD Visitantes No Socios
    │   ├── EventosController.cs     # CRUD Eventos
    │   ├── AccessLogsController.cs  # Registro de Accesos
    │   ├── ReportsController.cs     # Reportes
    │   └── ApiController.cs         # API REST endpoints
    ├── Models/                       # Modelos de datos
    │   ├── Usuario.cs
    │   ├── Socio.cs
    │   ├── Invitado.cs
    │   ├── VisitanteNoSocio.cs
    │   ├── Evento.cs
    │   ├── AccessLog.cs
    │   └── Report.cs
    ├── Services/                     # Capa de lógica de negocio
    │   ├── IAuthService.cs / AuthService.cs
    │   ├── ISocioService.cs / SocioService.cs
    │   ├── IInvitadoService.cs / InvitadoService.cs
    │   ├── IVisitanteService.cs / VisitanteService.cs
    │   ├── IEventoService.cs / EventoService.cs
    │   ├── IAccessLogService.cs / AccessLogService.cs
    │   ├── IReportService.cs / ReportService.cs
    │   ├── ICatalogService.cs / CatalogService.cs
    │   └── IAuditService.cs / AuditService.cs
    ├── Data/                         # Acceso a datos
    │   └── DatabaseConnection.cs
    ├── Views/                        # Vistas Razor
    │   ├── Shared/_Layout.cshtml    # Layout principal
    │   ├── Account/                 # Login, AccessDenied
    │   ├── Home/                    # Dashboard
    │   ├── Socios/                  # Index, Create, Edit, Details
    │   ├── Invitados/               # Index, Create, Invitados, CreateInvitado
    │   ├── Visitantes/              # Index, Create
    │   ├── Eventos/                 # Index, Create, Edit, Details
    │   ├── AccessLogs/              # Index, RegistrarAcceso
    │   └── Reports/                 # Index, AccessByDate, DeniedAccess
    ├── SQL/                          # Scripts de base de datos
    │   ├── 01_CreateDatabase.sql    # Creación de tablas
    │   ├── 02_StoredProcedures.sql  # Procedimientos almacenados
    │   └── 03_SeedData.sql          # Datos iniciales
    └── wwwroot/                      # Archivos estáticos
        ├── css/site.css
        └── js/site.js
```

## Requisitos Previos

- .NET 8.0 SDK
- Microsoft SQL Server 2019+
- Visual Studio 2022 (recomendado)

## Instalación

### 1. Base de Datos

Ejecutar los scripts SQL en orden en SQL Server Management Studio:

```sql
-- Paso 1: Crear base de datos y tablas
-- Abrir y ejecutar: SQL/01_CreateDatabase.sql

-- Paso 2: Crear procedimientos almacenados
-- Abrir y ejecutar: SQL/02_StoredProcedures.sql

-- Paso 3: Insertar datos iniciales
-- Abrir y ejecutar: SQL/03_SeedData.sql
```

### 2. Configurar Cadena de Conexión

Editar `appsettings.json` y actualizar la cadena de conexión:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR;Database=IngresosCountryDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 3. Crear Usuario Administrador

Antes de iniciar sesión, debe generar un hash BCrypt para la contraseña del administrador.
Puede usar una herramienta en línea o ejecutar el siguiente código C# en una consola:

```csharp
string hash = BCrypt.Net.BCrypt.HashPassword("Admin123!");
Console.WriteLine(hash);
```

Luego actualizar el registro en la tabla `tbl_usuarios`:

```sql
UPDATE tbl_usuarios SET PasswordHash = 'HASH_GENERADO' WHERE Username = 'admin';
```

### 4. Ejecutar el Proyecto

```bash
cd IngresosCountry
dotnet restore
dotnet run
```

O abrir `IngresosCountry.sln` en Visual Studio y presionar F5.

## Módulos

### Socios (Members)
- Registro y gestión de membresías
- Validación de tarjeta de membresía
- Búsqueda por nombre, documento o número
- Control de estados (Activo, Moroso, Suspendido)
- Denegación automática si tiene restricciones

### Invitados (Guests)
- Registro de invitados
- Creación de invitaciones vinculadas a socios
- Generación automática de código QR
- Validación de QR en punto de acceso

### Visitantes No Socios
- Registro de proveedores, transportistas, visitantes autorizados
- Captura de identificación, placa vehicular, persona destino

### Eventos
- Creación y gestión de eventos
- Registro de participantes
- Control de capacidad

### Registro de Accesos
- Registro de entrada y salida
- Validación automática de restricciones
- Filtros por fecha, tipo, área, resultado

### Reportes
- Dashboard con estadísticas en tiempo real
- Reporte de accesos por fecha
- Reporte de accesos denegados

## Roles y Permisos

| Rol | Permisos |
|-----|----------|
| Administrador | Acceso total |
| Oficial de Seguridad | Registro de accesos, visitantes |
| Mesa de Servicio | CRUD socios, invitados, eventos |
| Finanzas | Reportes financieros |
| Gerencia | Reportes gerenciales |

## API REST Endpoints

- `GET /api/socios/buscar?q={query}` - Buscar socios
- `GET /api/socios/validar/{numero}` - Validar membresía
- `GET /api/invitados/validar-qr/{codigo}` - Validar código QR

## Licencia

Proyecto privado - Todos los derechos reservados.
