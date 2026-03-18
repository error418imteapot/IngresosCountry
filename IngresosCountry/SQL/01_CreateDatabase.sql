-- =============================================
-- Ingresos Country - Database Creation Script
-- Microsoft SQL Server
-- =============================================

USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'IngresosCountryDB')
BEGIN
    ALTER DATABASE IngresosCountryDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE IngresosCountryDB;
END
GO

CREATE DATABASE IngresosCountryDB;
GO

USE IngresosCountryDB;
GO

-- =============================================
-- CATALOG TABLES
-- =============================================

CREATE TABLE tbl_paises (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Codigo NVARCHAR(10) NULL,
    Activo BIT DEFAULT 1,
    FechaCreacion DATETIME DEFAULT GETDATE()
);
GO

CREATE TABLE tbl_roles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL,
    Descripcion NVARCHAR(200) NULL,
    Activo BIT DEFAULT 1
);
GO

CREATE TABLE tbl_usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(256) NOT NULL,
    NombreCompleto NVARCHAR(150) NOT NULL,
    Email NVARCHAR(150) NULL,
    RolId INT NOT NULL FOREIGN KEY REFERENCES tbl_roles(Id),
    Activo BIT DEFAULT 1,
    UltimoAcceso DATETIME NULL,
    FechaCreacion DATETIME DEFAULT GETDATE()
);
GO

-- =============================================
-- MEMBERS (SOCIOS)
-- =============================================

CREATE TABLE tbl_socios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NumeroMembresia NVARCHAR(50) NOT NULL UNIQUE,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    DocumentoIdentidad NVARCHAR(50) NULL,
    Email NVARCHAR(150) NULL,
    Telefono NVARCHAR(50) NULL,
    Direccion NVARCHAR(300) NULL,
    PaisId INT NULL FOREIGN KEY REFERENCES tbl_paises(Id),
    FechaNacimiento DATE NULL,
    FechaIngreso DATE NOT NULL DEFAULT GETDATE(),
    FechaVencimiento DATE NULL,
    TipoMembresia NVARCHAR(50) NULL,
    Estado NVARCHAR(20) NOT NULL DEFAULT 'Activo', -- Activo, Suspendido, Moroso, Inactivo
    FotoUrl NVARCHAR(500) NULL,
    Notas NVARCHAR(500) NULL,
    Activo BIT DEFAULT 1,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FechaModificacion DATETIME NULL
);
GO

-- =============================================
-- RECIPROCITY CLUBS
-- =============================================

CREATE TABLE tbl_clubes_reciprocidad (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreClub NVARCHAR(150) NOT NULL,
    Direccion NVARCHAR(300) NULL,
    Telefono NVARCHAR(50) NULL,
    PaisId INT NULL FOREIGN KEY REFERENCES tbl_paises(Id),
    Activo BIT DEFAULT 1,
    FechaCreacion DATETIME DEFAULT GETDATE()
);
GO

CREATE TABLE tbl_socios_otros_clubes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SocioId INT NOT NULL FOREIGN KEY REFERENCES tbl_socios(Id),
    ClubReciprocidadId INT NOT NULL FOREIGN KEY REFERENCES tbl_clubes_reciprocidad(Id),
    NumeroMembresiaExterno NVARCHAR(50) NULL,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1
);
GO

CREATE TABLE tbl_pases_reciprocidad (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SocioOtroClubId INT NOT NULL FOREIGN KEY REFERENCES tbl_socios_otros_clubes(Id),
    FechaInicio DATE NOT NULL,
    FechaFin DATE NOT NULL,
    Aprobado BIT DEFAULT 0,
    AprobadoPor INT NULL FOREIGN KEY REFERENCES tbl_usuarios(Id),
    Notas NVARCHAR(500) NULL,
    FechaCreacion DATETIME DEFAULT GETDATE()
);
GO

-- =============================================
-- GUESTS (INVITADOS)
-- =============================================

CREATE TABLE tbl_invitados (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    DocumentoIdentidad NVARCHAR(50) NULL,
    Telefono NVARCHAR(50) NULL,
    Email NVARCHAR(150) NULL,
    FechaCreacion DATETIME DEFAULT GETDATE()
);
GO

CREATE TABLE tbl_invita (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SocioId INT NOT NULL FOREIGN KEY REFERENCES tbl_socios(Id),
    InvitadoId INT NOT NULL FOREIGN KEY REFERENCES tbl_invitados(Id),
    FechaInvitacion DATE NOT NULL DEFAULT GETDATE(),
    FechaExpiracion DATE NULL,
    CodigoQR NVARCHAR(200) NULL,
    Estado NVARCHAR(20) DEFAULT 'Pendiente', -- Pendiente, Aprobado, Usado, Expirado
    Notas NVARCHAR(500) NULL,
    FechaCreacion DATETIME DEFAULT GETDATE()
);
GO

-- =============================================
-- VISIT TYPES
-- =============================================

CREATE TABLE tbl_visitas_nosocio_tipoVisita (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL, -- Proveedor, Transportista, Visitante Autorizado, etc.
    Descripcion NVARCHAR(200) NULL,
    Activo BIT DEFAULT 1
);
GO

CREATE TABLE tbl_visitas_nosocio_otroTipoVisita (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(200) NULL,
    Activo BIT DEFAULT 1
);
GO

-- =============================================
-- NON-MEMBER VISITORS
-- =============================================

CREATE TABLE tbl_visitas_nosocios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    DocumentoIdentidad NVARCHAR(50) NULL,
    Empresa NVARCHAR(150) NULL,
    TipoVisitaId INT NULL FOREIGN KEY REFERENCES tbl_visitas_nosocio_tipoVisita(Id),
    OtroTipoVisitaId INT NULL FOREIGN KEY REFERENCES tbl_visitas_nosocio_otroTipoVisita(Id),
    PlacaVehiculo NVARCHAR(20) NULL,
    PersonaDestino NVARCHAR(150) NULL,
    AreaDestino NVARCHAR(100) NULL,
    Telefono NVARCHAR(50) NULL,
    Notas NVARCHAR(500) NULL,
    FechaCreacion DATETIME DEFAULT GETDATE()
);
GO

-- =============================================
-- EVENTS
-- =============================================

CREATE TABLE tbl_eventos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(200) NOT NULL,
    Descripcion NVARCHAR(500) NULL,
    FechaInicio DATETIME NOT NULL,
    FechaFin DATETIME NOT NULL,
    Ubicacion NVARCHAR(200) NULL,
    Capacidad INT NULL,
    OrganizadorSocioId INT NULL FOREIGN KEY REFERENCES tbl_socios(Id),
    Estado NVARCHAR(20) DEFAULT 'Programado', -- Programado, EnCurso, Finalizado, Cancelado
    Activo BIT DEFAULT 1,
    FechaCreacion DATETIME DEFAULT GETDATE()
);
GO

CREATE TABLE tbl_evento_participantes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EventoId INT NOT NULL FOREIGN KEY REFERENCES tbl_eventos(Id),
    Nombre NVARCHAR(150) NOT NULL,
    DocumentoIdentidad NVARCHAR(50) NULL,
    TipoParticipante NVARCHAR(20) NOT NULL, -- Socio, Invitado, Externo
    SocioId INT NULL FOREIGN KEY REFERENCES tbl_socios(Id),
    InvitadoId INT NULL FOREIGN KEY REFERENCES tbl_invitados(Id),
    Confirmado BIT DEFAULT 0,
    FechaRegistro DATETIME DEFAULT GETDATE()
);
GO

-- =============================================
-- ACCESS AREAS
-- =============================================

CREATE TABLE tbl_visitas_areas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(200) NULL,
    Ubicacion NVARCHAR(200) NULL,
    Activo BIT DEFAULT 1
);
GO

-- =============================================
-- ACCESS LOGS (VISITAS)
-- =============================================

CREATE TABLE tbl_visitas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TipoVisitante NVARCHAR(20) NOT NULL, -- Socio, Invitado, NoSocio, Evento
    SocioId INT NULL FOREIGN KEY REFERENCES tbl_socios(Id),
    InvitadoId INT NULL FOREIGN KEY REFERENCES tbl_invitados(Id),
    NoSocioId INT NULL FOREIGN KEY REFERENCES tbl_visitas_nosocios(Id),
    EventoId INT NULL FOREIGN KEY REFERENCES tbl_eventos(Id),
    AreaId INT NULL FOREIGN KEY REFERENCES tbl_visitas_areas(Id),
    FechaEntrada DATETIME NOT NULL DEFAULT GETDATE(),
    FechaSalida DATETIME NULL,
    ResultadoAcceso NVARCHAR(20) NOT NULL DEFAULT 'Aprobado', -- Aprobado, Denegado
    MotivoRechazo NVARCHAR(300) NULL,
    RegistradoPor INT NULL FOREIGN KEY REFERENCES tbl_usuarios(Id),
    PuntoAcceso NVARCHAR(100) NULL,
    Notas NVARCHAR(500) NULL,
    FechaCreacion DATETIME DEFAULT GETDATE()
);
GO

-- =============================================
-- AUDIT LOG
-- =============================================

CREATE TABLE tbl_audit_log (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioId INT NULL FOREIGN KEY REFERENCES tbl_usuarios(Id),
    Accion NVARCHAR(100) NOT NULL,
    Tabla NVARCHAR(100) NULL,
    RegistroId INT NULL,
    Detalle NVARCHAR(500) NULL,
    DireccionIP NVARCHAR(50) NULL,
    FechaCreacion DATETIME DEFAULT GETDATE()
);
GO

-- =============================================
-- INDEXES
-- =============================================

CREATE INDEX IX_tbl_socios_NumeroMembresia ON tbl_socios(NumeroMembresia);
CREATE INDEX IX_tbl_socios_Estado ON tbl_socios(Estado);
CREATE INDEX IX_tbl_visitas_FechaEntrada ON tbl_visitas(FechaEntrada);
CREATE INDEX IX_tbl_visitas_TipoVisitante ON tbl_visitas(TipoVisitante);
CREATE INDEX IX_tbl_visitas_ResultadoAcceso ON tbl_visitas(ResultadoAcceso);
CREATE INDEX IX_tbl_invita_SocioId ON tbl_invita(SocioId);
CREATE INDEX IX_tbl_invita_CodigoQR ON tbl_invita(CodigoQR);
GO

PRINT 'Database IngresosCountryDB created successfully.';
GO