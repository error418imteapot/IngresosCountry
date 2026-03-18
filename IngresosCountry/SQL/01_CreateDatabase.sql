
USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'IngresosCountry')
BEGIN
    ALTER DATABASE IngresosCountry SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE IngresosCountry;
END
GO

CREATE DATABASE IngresosCountry;
GO

USE IngresosCountry;
GO

-- =============================================
-- CATALOGOS
-- =============================================

CREATE TABLE tbl_paises (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Codigo NVARCHAR(10),
    Activo BIT DEFAULT 1,
    FechaCreacion DATETIME DEFAULT GETDATE()
);
GO

-- =============================================
-- SOCIOS
-- =============================================

CREATE TABLE tbl_socios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NumeroMembresia NVARCHAR(50) NOT NULL UNIQUE,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    DocumentoIdentidad NVARCHAR(50),
    Email NVARCHAR(150),
    Telefono NVARCHAR(50),
    Direccion NVARCHAR(300),
    PaisId INT FOREIGN KEY REFERENCES tbl_paises(Id),
    FechaNacimiento DATE,
    FechaIngreso DATE DEFAULT GETDATE(),
    FechaVencimiento DATE,
    TipoMembresia NVARCHAR(50),
    Estado NVARCHAR(20) DEFAULT 'Activo',
    FotoUrl NVARCHAR(500),
    Notas NVARCHAR(500),
    Activo BIT DEFAULT 1,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FechaModificacion DATETIME
);
GO

-- =============================================
-- CLUBES RECIPROCIDAD
-- =============================================

CREATE TABLE tbl_clubes_reciprocidad (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreClub NVARCHAR(150) NOT NULL,
    Direccion NVARCHAR(300),
    Telefono NVARCHAR(50),
    PaisId INT FOREIGN KEY REFERENCES tbl_paises(Id),
    Activo BIT DEFAULT 1,
    FechaCreacion DATETIME DEFAULT GETDATE()
);
GO

CREATE TABLE tbl_socios_otros_clubes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SocioId INT NOT NULL FOREIGN KEY REFERENCES tbl_socios(Id),
    ClubReciprocidadId INT NOT NULL FOREIGN KEY REFERENCES tbl_clubes_reciprocidad(Id),
    NumeroMembresiaExterno NVARCHAR(50),
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
    AprobadoPor NVARCHAR(100),
    Notas NVARCHAR(500),
    FechaCreacion DATETIME DEFAULT GETDATE()
);
GO

-- =============================================
-- INVITADOS
-- =============================================

CREATE TABLE tbl_invitados (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    DocumentoIdentidad NVARCHAR(50),
    Telefono NVARCHAR(50),
    Email NVARCHAR(150),
    FechaCreacion DATETIME DEFAULT GETDATE()
);
GO

CREATE TABLE tbl_invita (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SocioId INT NOT NULL FOREIGN KEY REFERENCES tbl_socios(Id),
    InvitadoId INT NOT NULL FOREIGN KEY REFERENCES tbl_invitados(Id),
    FechaInvitacion DATE DEFAULT GETDATE(),
    FechaExpiracion DATE,
    CodigoQR NVARCHAR(200),
    Estado NVARCHAR(20) DEFAULT 'Pendiente',
    Notas NVARCHAR(500),
    FechaCreacion DATETIME DEFAULT GETDATE()
);
GO

-- =============================================
-- TIPOS VISITA
-- =============================================

CREATE TABLE tbl_visitas_nosocio_tipoVisita (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(200),
    Activo BIT DEFAULT 1
);
GO

-- =============================================
-- VISITANTES NO SOCIOS
-- =============================================

CREATE TABLE tbl_visitas_nosocios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    DocumentoIdentidad NVARCHAR(50),
    Empresa NVARCHAR(150),
    TipoVisitaId INT FOREIGN KEY REFERENCES tbl_visitas_nosocio_tipoVisita(Id),
    PlacaVehiculo NVARCHAR(20),
    PersonaDestino NVARCHAR(150),
    AreaDestino NVARCHAR(100),
    Telefono NVARCHAR(50),
    Notas NVARCHAR(500),
    FechaCreacion DATETIME DEFAULT GETDATE()
);
GO

-- =============================================
-- EVENTOS
-- =============================================

CREATE TABLE tbl_eventos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(200) NOT NULL,
    Descripcion NVARCHAR(500),
    FechaInicio DATETIME NOT NULL,
    FechaFin DATETIME NOT NULL,
    Ubicacion NVARCHAR(200),
    Capacidad INT,
    OrganizadorSocioId INT FOREIGN KEY REFERENCES tbl_socios(Id),
    Estado NVARCHAR(20) DEFAULT 'Programado',
    Activo BIT DEFAULT 1,
    FechaCreacion DATETIME DEFAULT GETDATE()
);
GO

CREATE TABLE tbl_evento_participantes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EventoId INT NOT NULL FOREIGN KEY REFERENCES tbl_eventos(Id),
    Nombre NVARCHAR(150) NOT NULL,
    DocumentoIdentidad NVARCHAR(50),
    TipoParticipante NVARCHAR(20),
    SocioId INT FOREIGN KEY REFERENCES tbl_socios(Id),
    InvitadoId INT FOREIGN KEY REFERENCES tbl_invitados(Id),
    Confirmado BIT DEFAULT 0,
    FechaRegistro DATETIME DEFAULT GETDATE()
);
GO

-- =============================================
-- AREAS
-- =============================================

CREATE TABLE tbl_visitas_areas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(200),
    Ubicacion NVARCHAR(200),
    Activo BIT DEFAULT 1
);
GO

-- =============================================
-- REGISTRO DE ACCESOS
-- =============================================

CREATE TABLE tbl_visitas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TipoVisitante NVARCHAR(20) NOT NULL,
    SocioId INT FOREIGN KEY REFERENCES tbl_socios(Id),
    InvitadoId INT FOREIGN KEY REFERENCES tbl_invitados(Id),
    NoSocioId INT FOREIGN KEY REFERENCES tbl_visitas_nosocios(Id),
    EventoId INT FOREIGN KEY REFERENCES tbl_eventos(Id),
    AreaId INT FOREIGN KEY REFERENCES tbl_visitas_areas(Id),
    FechaEntrada DATETIME DEFAULT GETDATE(),
    FechaSalida DATETIME,
    ResultadoAcceso NVARCHAR(20) DEFAULT 'Aprobado',
    MotivoRechazo NVARCHAR(300),
    RegistradoPor NVARCHAR(100),
    PuntoAcceso NVARCHAR(100),
    Notas NVARCHAR(500),
    FechaCreacion DATETIME DEFAULT GETDATE()
);
GO

-- =============================================
-- INDEXES
-- =============================================

CREATE INDEX IX_tbl_socios_NumeroMembresia ON tbl_socios(NumeroMembresia);
CREATE INDEX IX_tbl_socios_Estado ON tbl_socios(Estado);
CREATE INDEX IX_tbl_visitas_FechaEntrada ON tbl_visitas(FechaEntrada);
GO

PRINT 'Base de datos IngresosCountry creada correctamente.';
GO