USE IngresosCountry;
GO

CREATE OR ALTER PROCEDURE sp_Socios_GetAll
    @Estado NVARCHAR(20) = NULL,
    @Busqueda NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT s.*, p.Nombre AS PaisNombre
    FROM tbl_socios s
    LEFT JOIN tbl_paises p ON s.PaisId = p.Id
    WHERE s.Activo = 1
      AND (@Estado IS NULL OR s.Estado = @Estado)
      AND (@Busqueda IS NULL OR s.Nombre LIKE '%' + @Busqueda + '%' 
           OR s.Apellido LIKE '%' + @Busqueda + '%'
           OR s.NumeroMembresia LIKE '%' + @Busqueda + '%'
           OR s.DocumentoIdentidad LIKE '%' + @Busqueda + '%')
    ORDER BY s.Apellido, s.Nombre;
END
GO

CREATE OR ALTER PROCEDURE sp_Socios_GetById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT s.*, p.Nombre AS PaisNombre
    FROM tbl_socios s
    LEFT JOIN tbl_paises p ON s.PaisId = p.Id
    WHERE s.Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE sp_Socios_GetByMembresia
    @NumeroMembresia NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT s.*, p.Nombre AS PaisNombre
    FROM tbl_socios s
    LEFT JOIN tbl_paises p ON s.PaisId = p.Id
    WHERE s.NumeroMembresia = @NumeroMembresia AND s.Activo = 1;
END
GO

CREATE OR ALTER PROCEDURE sp_Socios_Insert
    @NumeroMembresia NVARCHAR(50),
    @Nombre NVARCHAR(100),
    @Apellido NVARCHAR(100),
    @DocumentoIdentidad NVARCHAR(50) = NULL,
    @Email NVARCHAR(150) = NULL,
    @Telefono NVARCHAR(50) = NULL,
    @Direccion NVARCHAR(300) = NULL,
    @PaisId INT = NULL,
    @FechaNacimiento DATE = NULL,
    @TipoMembresia NVARCHAR(50) = NULL,
    @Estado NVARCHAR(20) = 'Activo',
    @FotoUrl NVARCHAR(500) = NULL,
    @Notas NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tbl_socios (NumeroMembresia, Nombre, Apellido, DocumentoIdentidad, Email, 
        Telefono, Direccion, PaisId, FechaNacimiento, TipoMembresia, Estado, FotoUrl, Notas)
    VALUES (@NumeroMembresia, @Nombre, @Apellido, @DocumentoIdentidad, @Email,
        @Telefono, @Direccion, @PaisId, @FechaNacimiento, @TipoMembresia, @Estado, @FotoUrl, @Notas);
    SELECT SCOPE_IDENTITY() AS Id;
END
GO

CREATE OR ALTER PROCEDURE sp_Socios_Update
    @Id INT,
    @NumeroMembresia NVARCHAR(50),
    @Nombre NVARCHAR(100),
    @Apellido NVARCHAR(100),
    @DocumentoIdentidad NVARCHAR(50) = NULL,
    @Email NVARCHAR(150) = NULL,
    @Telefono NVARCHAR(50) = NULL,
    @Direccion NVARCHAR(300) = NULL,
    @PaisId INT = NULL,
    @FechaNacimiento DATE = NULL,
    @TipoMembresia NVARCHAR(50) = NULL,
    @Estado NVARCHAR(20) = 'Activo',
    @FotoUrl NVARCHAR(500) = NULL,
    @Notas NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tbl_socios SET
        NumeroMembresia = @NumeroMembresia,
        Nombre = @Nombre,
        Apellido = @Apellido,
        DocumentoIdentidad = @DocumentoIdentidad,
        Email = @Email,
        Telefono = @Telefono,
        Direccion = @Direccion,
        PaisId = @PaisId,
        FechaNacimiento = @FechaNacimiento,
        TipoMembresia = @TipoMembresia,
        Estado = @Estado,
        FotoUrl = @FotoUrl,
        Notas = @Notas,
        FechaModificacion = GETDATE()
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE sp_Socios_Delete
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tbl_socios SET Activo = 0, FechaModificacion = GETDATE() WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE sp_Invitados_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM tbl_invitados ORDER BY Apellido, Nombre;
END
GO

CREATE OR ALTER PROCEDURE sp_Invitados_Insert
    @Nombre NVARCHAR(100),
    @Apellido NVARCHAR(100),
    @DocumentoIdentidad NVARCHAR(50) = NULL,
    @Telefono NVARCHAR(50) = NULL,
    @Email NVARCHAR(150) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tbl_invitados (Nombre, Apellido, DocumentoIdentidad, Telefono, Email)
    VALUES (@Nombre, @Apellido, @DocumentoIdentidad, @Telefono, @Email);
    SELECT SCOPE_IDENTITY() AS Id;
END
GO

CREATE OR ALTER PROCEDURE sp_Invita_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT i.*, inv.Nombre AS InvitadoNombre, inv.Apellido AS InvitadoApellido,
           s.Nombre AS SocioNombre, s.Apellido AS SocioApellido
    FROM tbl_invita i
    INNER JOIN tbl_invitados inv ON i.InvitadoId = inv.Id
    INNER JOIN tbl_socios s ON i.SocioId = s.Id
    ORDER BY i.FechaInvitacion DESC;
END
GO

CREATE OR ALTER PROCEDURE sp_Invita_Insert
    @SocioId INT,
    @InvitadoId INT,
    @FechaInvitacion DATE,
    @FechaExpiracion DATE = NULL,
    @CodigoQR NVARCHAR(200) = NULL,
    @Notas NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tbl_invita (SocioId, InvitadoId, FechaInvitacion, FechaExpiracion, CodigoQR, Estado, Notas)
    VALUES (@SocioId, @InvitadoId, @FechaInvitacion, @FechaExpiracion, @CodigoQR, 'Pendiente', @Notas);
    SELECT SCOPE_IDENTITY() AS Id;
END
GO

CREATE OR ALTER PROCEDURE sp_VisitantesNoSocios_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT v.*, t.Nombre AS TipoVisitaNombre
    FROM tbl_visitas_nosocios v
    LEFT JOIN tbl_visitas_nosocio_tipoVisita t ON v.TipoVisitaId = t.Id
    ORDER BY v.FechaCreacion DESC;
END
GO

CREATE OR ALTER PROCEDURE sp_VisitantesNoSocios_Insert
    @Nombre NVARCHAR(100),
    @Apellido NVARCHAR(100),
    @DocumentoIdentidad NVARCHAR(50) = NULL,
    @Empresa NVARCHAR(150) = NULL,
    @TipoVisitaId INT = NULL,
    @PlacaVehiculo NVARCHAR(20) = NULL,
    @PersonaDestino NVARCHAR(150) = NULL,
    @AreaDestino NVARCHAR(100) = NULL,
    @Telefono NVARCHAR(50) = NULL,
    @Notas NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tbl_visitas_nosocios (Nombre, Apellido, DocumentoIdentidad, Empresa, TipoVisitaId,
        PlacaVehiculo, PersonaDestino, AreaDestino, Telefono, Notas)
    VALUES (@Nombre, @Apellido, @DocumentoIdentidad, @Empresa, @TipoVisitaId,
        @PlacaVehiculo, @PersonaDestino, @AreaDestino, @Telefono, @Notas);
    SELECT SCOPE_IDENTITY() AS Id;
END
GO

CREATE OR ALTER PROCEDURE sp_Visitas_RegistrarEntrada
    @TipoVisitante NVARCHAR(20),
    @SocioId INT = NULL,
    @InvitadoId INT = NULL,
    @NoSocioId INT = NULL,
    @EventoId INT = NULL,
    @AreaId INT = NULL,
    @ResultadoAcceso NVARCHAR(20) = 'Aprobado',
    @MotivoRechazo NVARCHAR(300) = NULL,
    @RegistradoPor NVARCHAR(100) = NULL,
    @PuntoAcceso NVARCHAR(100) = NULL,
    @Notas NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tbl_visitas (TipoVisitante, SocioId, InvitadoId, NoSocioId, EventoId, AreaId,
        FechaEntrada, ResultadoAcceso, MotivoRechazo, RegistradoPor, PuntoAcceso, Notas)
    VALUES (@TipoVisitante, @SocioId, @InvitadoId, @NoSocioId, @EventoId, @AreaId,
        GETDATE(), @ResultadoAcceso, @MotivoRechazo, @RegistradoPor, @PuntoAcceso, @Notas);
    SELECT SCOPE_IDENTITY() AS Id;
END
GO

CREATE OR ALTER PROCEDURE sp_Visitas_RegistrarSalida
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tbl_visitas SET FechaSalida = GETDATE() WHERE Id = @Id AND FechaSalida IS NULL;
END
GO

CREATE OR ALTER PROCEDURE sp_Reports_Dashboard
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        (SELECT COUNT(*) FROM tbl_visitas WHERE CAST(FechaEntrada AS DATE) = CAST(GETDATE() AS DATE) AND ResultadoAcceso = 'Aprobado') AS EntradasHoy,
        (SELECT COUNT(*) FROM tbl_visitas WHERE CAST(FechaEntrada AS DATE) = CAST(GETDATE() AS DATE) AND ResultadoAcceso = 'Denegado') AS DenegadosHoy,
        (SELECT COUNT(*) FROM tbl_visitas WHERE CAST(FechaEntrada AS DATE) = CAST(GETDATE() AS DATE) AND FechaSalida IS NULL) AS DentroDelClub,
        (SELECT COUNT(*) FROM tbl_socios WHERE Activo = 1 AND Estado = 'Activo') AS SociosActivos;
END
GO