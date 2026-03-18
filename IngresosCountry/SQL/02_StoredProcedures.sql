USE IngresosCountry;
GO

-- =============================================
-- SOCIOS
-- =============================================

CREATE OR ALTER PROCEDURE sp_Socios_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM tbl_socios
    ORDER BY nombre;
END
GO

-- =============================================
-- INVITADOS
-- =============================================

CREATE OR ALTER PROCEDURE sp_Invitados_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM tbl_invitados
    ORDER BY nombre;
END
GO

-- =============================================
-- VISITANTES NO SOCIOS
-- =============================================

CREATE OR ALTER PROCEDURE sp_VisitantesNoSocios_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT v.*, t.descripcion AS TipoVisitaNombre
    FROM tbl_visitas_nosocios v
    LEFT JOIN tbl_visitas_nosocio_tipoVisita t 
        ON v.tipo_visita = t.id_tipo_visita
    ORDER BY v.fecha_ingreso DESC;
END
GO

-- =============================================
-- REGISTRO DE ENTRADA
-- =============================================

CREATE OR ALTER PROCEDURE sp_Visitas_RegistrarEntrada
    @carnet INT,
    @nombre NVARCHAR(150),
    @sexo NVARCHAR(10),
    @fnacimiento DATE,
    @ubicacion NVARCHAR(150),
    @temperatura DECIMAL(5,2),
    @id_linea INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO tbl_visitas (
        carnet, fecha, hora, nombre, sexo, fnacimiento,
        ubicacion, temperatura, salida, id_linea
    )
    VALUES (
        @carnet, GETDATE(), GETDATE(), @nombre, @sexo,
        @fnacimiento, @ubicacion, @temperatura, 0, @id_linea
    );
END
GO

-- =============================================
-- REGISTRO DE SALIDA
-- =============================================

CREATE OR ALTER PROCEDURE sp_Visitas_RegistrarSalida
    @carnet INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE tbl_visitas
    SET salida = 1,
        horasalida = GETDATE()
    WHERE carnet = @carnet AND salida = 0;
END
GO

-- =============================================
-- REPORTES (DASHBOARD)
-- =============================================

CREATE OR ALTER PROCEDURE sp_Reports_Dashboard
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        (SELECT COUNT(*) FROM tbl_visitas WHERE salida = 0) AS DentroDelClub,
        (SELECT COUNT(*) FROM tbl_socios WHERE estatus = 'Activo') AS SociosActivos;
END
GO