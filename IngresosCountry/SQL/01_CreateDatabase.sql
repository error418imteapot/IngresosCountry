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
-- PAISES
-- =============================================

CREATE TABLE tbl_paises (
    id_pais INT IDENTITY(1,1) PRIMARY KEY,
    pais NVARCHAR(100),
    cod_area NVARCHAR(10)
);
GO

-- =============================================
-- SOCIOS
-- =============================================

CREATE TABLE tbl_socios (
    carne INT PRIMARY KEY,
    nombre NVARCHAR(150),
    cedula NVARCHAR(50),
    sexo NVARCHAR(10),
    telefono NVARCHAR(50),
    telefono2 NVARCHAR(50),
    celular NVARCHAR(50),
    direccion NVARCHAR(300),
    email1 NVARCHAR(150),
    email2 NVARCHAR(150),
    fechaNacimiento DATE,
    fechaIngreso DATE,
    estadoCivil NVARCHAR(50),
    nacionalidad NVARCHAR(100),
    estatus NVARCHAR(50)
);
GO

-- =============================================
-- INVITADOS
-- =============================================

CREATE TABLE tbl_invitados (
    identificacion NVARCHAR(50) PRIMARY KEY,
    nombre NVARCHAR(150),
    fechanac DATE,
    fechaing DATE,
    horaing TIME,
    carne INT FOREIGN KEY REFERENCES tbl_socios(carne),
    nombrosoc NVARCHAR(150),
    estadoingreso NVARCHAR(50),
    ubicacion NVARCHAR(100),
    temperatura DECIMAL(5,2),
    id_visita INT,
    tipo_visita NVARCHAR(50)
);
GO

-- =============================================
-- TIPOS VISITA
-- =============================================

CREATE TABLE tbl_visitas_nosocio_tipoVisita (
    id_tipo_visita INT IDENTITY(1,1) PRIMARY KEY,
    descripcion NVARCHAR(200),
    aplica_vencimiento BIT,
    evento BIT,
    activo BIT
);
GO

CREATE TABLE tbl_visitas_nosocio_otroTipoVisita (
    id_tipo_visita INT IDENTITY(1,1) PRIMARY KEY,
    descripcion NVARCHAR(200),
    activo BIT,
    id_linea INT
);
GO

-- =============================================
-- VISITANTES NO SOCIOS
-- =============================================

CREATE TABLE tbl_visitas_nosocios (
    id_visita INT IDENTITY(1,1) PRIMARY KEY,
    tipo_visita INT FOREIGN KEY REFERENCES tbl_visitas_nosocio_tipoVisita(id_tipo_visita),
    nombre_visita NVARCHAR(150),
    identificacion_visita NVARCHAR(50),
    tipo_identificacion_visita NVARCHAR(50),
    celular_visita NVARCHAR(50),
    email_visita NVARCHAR(150),
    nombre_socio NVARCHAR(150),
    carne_socio INT,
    email_socio NVARCHAR(150),
    nombre_solicita NVARCHAR(150),
    fecha_ingreso DATETIME,
    fecha_vence DATETIME,
    nombre_evento NVARCHAR(150),
    codigo_evento NVARCHAR(50)
);
GO

-- =============================================
-- AREAS
-- =============================================

CREATE TABLE tbl_visitas_areas (
    id_linea INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(150),
    subarea NVARCHAR(150)
);
GO

-- =============================================
-- VISITAS (ACCESOS)
-- =============================================

CREATE TABLE tbl_visitas (
    carnet INT,
    fecha DATE,
    hora TIME,
    nombre NVARCHAR(150),
    sexo NVARCHAR(10),
    fnacimiento DATE,
    ubicacion NVARCHAR(150),
    temperatura DECIMAL(5,2),
    salida BIT,
    horasalida TIME,
    ubicacionsalida NVARCHAR(150),
    id_linea INT FOREIGN KEY REFERENCES tbl_visitas_areas(id_linea)
);
GO

-- =============================================
-- PADRON
-- =============================================

CREATE TABLE tbl_padron (
    cedula NVARCHAR(50) PRIMARY KEY,
    codelect NVARCHAR(50),
    fechacaduc DATE,
    junta NVARCHAR(100),
    nombre NVARCHAR(150)
);
GO

-- =============================================
-- CLUBES RECIPROCIDAD
-- =============================================

CREATE TABLE tbl_clubes_reciprocidad (
    id_club INT IDENTITY(1,1) PRIMARY KEY,
    cedula NVARCHAR(50),
    nombre NVARCHAR(150),
    pais INT FOREIGN KEY REFERENCES tbl_paises(id_pais),
    contacto1 NVARCHAR(100),
    contacto2 NVARCHAR(100),
    email1 NVARCHAR(150),
    telefono1 NVARCHAR(50)
);
GO

CREATE TABLE tbl_socios_otros_clubes (
    id_socio INT IDENTITY(1,1) PRIMARY KEY,
    identificacion NVARCHAR(50),
    nombre NVARCHAR(150),
    id_club INT FOREIGN KEY REFERENCES tbl_clubes_reciprocidad(id_club),
    pais INT FOREIGN KEY REFERENCES tbl_paises(id_pais),
    email1 NVARCHAR(150),
    telefono1 NVARCHAR(50),
    fecha_registro DATETIME,
    activo BIT
);
GO

CREATE TABLE tbl_pases_reciprocidad (
    id_pase INT IDENTITY(1,1) PRIMARY KEY,
    id_socio INT FOREIGN KEY REFERENCES tbl_socios_otros_clubes(id_socio),
    nombre NVARCHAR(150),
    identificacion NVARCHAR(50),
    pais INT,
    fecha_solicitud DATE,
    fecha_inicio DATE,
    fecha_final DATE
);
GO

PRINT 'Base de datos alineada al diagrama creada correctamente';
GO