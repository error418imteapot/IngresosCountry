-- =============================================
-- Ingresos Country - Stored Procedures
-- =============================================

USE IngresosCountryDB;
GO

-- =============================================
-- AUTHENTICATION
-- =============================================

CREATE OR ALTER PROCEDURE sp_Usuario_Login
    @Username NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT u.Id, u.Username, u.PasswordHash, u.NombreCompleto, u.Email, 
           u.RolId, r.Nombre AS RolNombre, u.Activo
    FROM tbl_usuarios u
    INNER JOIN tbl_roles r ON u.RolId = r.Id
    WHERE u.Username = @Username AND u.Activo = 1;

    UPDATE tbl_usuarios SET UltimoAcceso = GETDATE() WHERE Username = @Username;
END
GO

-- =============================================
-- SOCIOS CRUD
-- =============================================

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
        NumeroMembresia = @NumeroMembresia, Nombre = @Nombre, Apellido = @Apellido,
        DocumentoIdentidad = @DocumentoIdentidad, Email = @Email, Telefono = @Telefono,
        Direccion = @Direccion, PaisId = @PaisId, FechaNacimiento = @FechaNacimiento,
        TipoMembresia = @TipoMembresia, Estado = @Estado, FotoUrl = @FotoUrl,
        Notas = @Notas, FechaModificacion = GETDATE()
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

-- =============================================
-- INVITADOS CRUD
-- =============================================

CREATE OR ALTER PROCEDURE sp_Invitados_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM tbl_invitados ORDER BY Apellido, Nombre;
END
GO

CREATE OR ALTER PROCEDURE sp_Invitados_GetById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM tbl_invitados WHERE Id = @Id;
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

CREATE OR ALTER PROCEDURE sp_Invitados_Update
    @Id INT,
    @Nombre NVARCHAR(100),
    @Apellido NVARCHAR(100),
    @DocumentoIdentidad NVARCHAR(50) = NULL,
    @Telefono NVARCHAR(50) = NULL,
    @Email NVARCHAR(150) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tbl_invitados SET Nombre = @Nombre, Apellido = @Apellido,
        DocumentoIdentidad = @DocumentoIdentidad, Telefono = @Telefono, Email = @Email
    WHERE Id = @Id;
END
GO

-- =============================================
-- INVITACIONES (INVITA) CRUD
-- =============================================

CREATE OR ALTER PROCEDURE sp_Invita_GetBySocio
    @SocioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT i.*, inv.Nombre AS InvitadoNombre, inv.Apellido AS InvitadoApellido,
           s.Nombre AS SocioNombre, s.Apellido AS SocioApellido
    FROM tbl_invita i
    INNER JOIN tbl_invitados inv ON i.InvitadoId = inv.Id
    INNER JOIN tbl_socios s ON i.SocioId = s.Id
    WHERE i.SocioId = @SocioId
    ORDER BY i.FechaInvitacion DESC;
END
GO

CREATE OR ALTER PROCEDURE sp_Invita_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT i.*, inv.Nombre AS InvitadoNombre, inv.Apellido AS InvitadoApellido,
           s.Nombre AS SocioNombre, s.Apellido AS SocioApellido, s.NumeroMembresia
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

CREATE OR ALTER PROCEDURE sp_Invita_ValidateQR
    @CodigoQR NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT i.*, inv.Nombre AS InvitadoNombre, inv.Apellido AS InvitadoApellido,
           s.Nombre AS SocioNombre, s.Apellido AS SocioApellido, s.NumeroMembresia
    FROM tbl_invita i
    INNER JOIN tbl_invitados inv ON i.InvitadoId = inv.Id
    INNER JOIN tbl_socios s ON i.SocioId = s.Id
    WHERE i.CodigoQR = @CodigoQR 
      AND i.Estado = 'Aprobado'
      AND (i.FechaExpiracion IS NULL OR i.FechaExpiracion >= CAST(GETDATE() AS DATE));
END
GO

-- =============================================
-- VISITANTES NO SOCIOS CRUD
-- =============================================

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
    @OtroTipoVisitaId INT = NULL,
    @PlacaVehiculo NVARCHAR(20) = NULL,
    @PersonaDestino NVARCHAR(150) = NULL,
    @AreaDestino NVARCHAR(100) = NULL,
    @Telefono NVARCHAR(50) = NULL,
    @Notas NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tbl_visitas_nosocios (Nombre, Apellido, DocumentoIdentidad, Empresa, TipoVisitaId,
        OtroTipoVisitaId, PlacaVehiculo, PersonaDestino, AreaDestino, Telefono, Notas)
    VALUES (@Nombre, @Apellido, @DocumentoIdentidad, @Empresa, @TipoVisitaId,
        @OtroTipoVisitaId, @PlacaVehiculo, @PersonaDestino, @AreaDestino, @Telefono, @Notas);
    SELECT SCOPE_IDENTITY() AS Id;
END
GO

-- =============================================
-- EVENTOS CRUD
-- =============================================

CREATE OR ALTER PROCEDURE sp_Eventos_GetAll
    @Estado NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT e.*, s.Nombre + ' ' + s.Apellido AS OrganizadorNombre,
           (SELECT COUNT(*) FROM tbl_evento_participantes WHERE EventoId = e.Id) AS TotalParticipantes
    FROM tbl_eventos e
    LEFT JOIN tbl_socios s ON e.OrganizadorSocioId = s.Id
    WHERE e.Activo = 1
      AND (@Estado IS NULL OR e.Estado = @Estado)
    ORDER BY e.FechaInicio DESC;
END
GO

CREATE OR ALTER PROCEDURE sp_Eventos_GetById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT e.*, s.Nombre + ' ' + s.Apellido AS OrganizadorNombre
    FROM tbl_eventos e
    LEFT JOIN tbl_socios s ON e.OrganizadorSocioId = s.Id
    WHERE e.Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE sp_Eventos_Insert
    @Nombre NVARCHAR(200),
    @Descripcion NVARCHAR(500) = NULL,
    @FechaInicio DATETIME,
    @FechaFin DATETIME,
    @Ubicacion NVARCHAR(200) = NULL,
    @Capacidad INT = NULL,
    @OrganizadorSocioId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tbl_eventos (Nombre, Descripcion, FechaInicio, FechaFin, Ubicacion, Capacidad, OrganizadorSocioId)
    VALUES (@Nombre, @Descripcion, @FechaInicio, @FechaFin, @Ubicacion, @Capacidad, @OrganizadorSocioId);
    SELECT SCOPE_IDENTITY() AS Id;
END
GO

CREATE OR ALTER PROCEDURE sp_Eventos_Update
    @Id INT,
    @Nombre NVARCHAR(200),
    @Descripcion NVARCHAR(500) = NULL,
    @FechaInicio DATETIME,
    @FechaFin DATETIME,
    @Ubicacion NVARCHAR(200) = NULL,
    @Capacidad INT = NULL,
    @OrganizadorSocioId INT = NULL,
    @Estado NVARCHAR(20) = 'Programado'
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tbl_eventos SET Nombre = @Nombre, Descripcion = @Descripcion,
        FechaInicio = @FechaInicio, FechaFin = @FechaFin, Ubicacion = @Ubicacion,
        Capacidad = @Capacidad, OrganizadorSocioId = @OrganizadorSocioId, Estado = @Estado
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE sp_EventoParticipantes_Insert
    @EventoId INT,
    @Nombre NVARCHAR(150),
    @DocumentoIdentidad NVARCHAR(50) = NULL,
    @TipoParticipante NVARCHAR(20),
    @SocioId INT = NULL,
    @InvitadoId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tbl_evento_participantes (EventoId, Nombre, DocumentoIdentidad, TipoParticipante, SocioId, InvitadoId)
    VALUES (@EventoId, @Nombre, @DocumentoIdentidad, @TipoParticipante, @SocioId, @InvitadoId);
    SELECT SCOPE_IDENTITY() AS Id;
END
GO

CREATE OR ALTER PROCEDURE sp_EventoParticipantes_GetByEvento
    @EventoId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ep.*, s.NumeroMembresia
    FROM tbl_evento_participantes ep
    LEFT JOIN tbl_socios s ON ep.SocioId = s.Id
    WHERE ep.EventoId = @EventoId
    ORDER BY ep.Nombre;
END
GO

-- =============================================
-- ACCESS LOGS (VISITAS)
-- =============================================

CREATE OR ALTER PROCEDURE sp_Visitas_RegistrarEntrada
    @TipoVisitante NVARCHAR(20),
    @SocioId INT = NULL,
    @InvitadoId INT = NULL,
    @NoSocioId INT = NULL,
    @EventoId INT = NULL,
    @AreaId INT = NULL,
    @ResultadoAcceso NVARCHAR(20) = 'Aprobado',
    @MotivoRechazo NVARCHAR(300) = NULL,
    @RegistradoPor INT = NULL,
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

CREATE OR ALTER PROCEDURE sp_Visitas_GetAll
    @FechaDesde DATE = NULL,
    @FechaHasta DATE = NULL,
    @TipoVisitante NVARCHAR(20) = NULL,
    @ResultadoAcceso NVARCHAR(20) = NULL,
    @AreaId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT v.*, 
           a.Nombre AS AreaNombre,
           u.NombreCompleto AS RegistradoPorNombre,
           CASE 
               WHEN v.TipoVisitante = 'Socio' THEN s.Nombre + ' ' + s.Apellido
               WHEN v.TipoVisitante = 'Invitado' THEN inv.Nombre + ' ' + inv.Apellido
               WHEN v.TipoVisitante = 'NoSocio' THEN ns.Nombre + ' ' + ns.Apellido
               ELSE 'N/A'
           END AS VisitanteNombre
    FROM tbl_visitas v
    LEFT JOIN tbl_visitas_areas a ON v.AreaId = a.Id
    LEFT JOIN tbl_usuarios u ON v.RegistradoPor = u.Id
    LEFT JOIN tbl_socios s ON v.SocioId = s.Id
    LEFT JOIN tbl_invitados inv ON v.InvitadoId = inv.Id
    LEFT JOIN tbl_visitas_nosocios ns ON v.NoSocioId = ns.Id
    WHERE (@FechaDesde IS NULL OR CAST(v.FechaEntrada AS DATE) >= @FechaDesde)
      AND (@FechaHasta IS NULL OR CAST(v.FechaEntrada AS DATE) <= @FechaHasta)
      AND (@TipoVisitante IS NULL OR v.TipoVisitante = @TipoVisitante)
      AND (@ResultadoAcceso IS NULL OR v.ResultadoAcceso = @ResultadoAcceso)
      AND (@AreaId IS NULL OR v.AreaId = @AreaId)
    ORDER BY v.FechaEntrada DESC;
END
GO

-- =============================================
-- REPORTS
-- =============================================

CREATE OR ALTER PROCEDURE sp_Reports_Dashboard
AS
BEGIN
    SET NOCOUNT ON;
    -- Today's stats
    SELECT 
        (SELECT COUNT(*) FROM tbl_visitas WHERE CAST(FechaEntrada AS DATE) = CAST(GETDATE() AS DATE) AND ResultadoAcceso = 'Aprobado') AS EntradasHoy,
        (SELECT COUNT(*) FROM tbl_visitas WHERE CAST(FechaEntrada AS DATE) = CAST(GETDATE() AS DATE) AND ResultadoAcceso = 'Denegado') AS DenegadosHoy,
        (SELECT COUNT(*) FROM tbl_visitas WHERE CAST(FechaEntrada AS DATE) = CAST(GETDATE() AS DATE) AND FechaSalida IS NULL) AS DentroDelClub,
        (SELECT COUNT(*) FROM tbl_socios WHERE Activo = 1 AND Estado = 'Activo') AS SociosActivos,
        (SELECT COUNT(*) FROM tbl_socios WHERE Estado = 'Moroso') AS SociosMorosos,
        (SELECT COUNT(*) FROM tbl_eventos WHERE Estado = 'Programado' AND FechaInicio >= GETDATE()) AS EventosProximos;
END
GO

CREATE OR ALTER PROCEDURE sp_Reports_AccessByDate
    @FechaDesde DATE,
    @FechaHasta DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CAST(FechaEntrada AS DATE) AS Fecha,
           TipoVisitante,
           ResultadoAcceso,
           COUNT(*) AS Total
    FROM tbl_visitas
    WHERE CAST(FechaEntrada AS DATE) BETWEEN @FechaDesde AND @FechaHasta
    GROUP BY CAST(FechaEntrada AS DATE), TipoVisitante, ResultadoAcceso
    ORDER BY Fecha DESC;
END
GO

CREATE OR ALTER PROCEDURE sp_Reports_DeniedAccess
    @FechaDesde DATE = NULL,
    @FechaHasta DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT v.*, 
           CASE 
               WHEN v.TipoVisitante = 'Socio' THEN s.Nombre + ' ' + s.Apellido
               WHEN v.TipoVisitante = 'Invitado' THEN inv.Nombre + ' ' + inv.Apellido
               WHEN v.TipoVisitante = 'NoSocio' THEN ns.Nombre + ' ' + ns.Apellido
               ELSE 'N/A'
           END AS VisitanteNombre
    FROM tbl_visitas v
    LEFT JOIN tbl_socios s ON v.SocioId = s.Id
    LEFT JOIN tbl_invitados inv ON v.InvitadoId = inv.Id
    LEFT JOIN tbl_visitas_nosocios ns ON v.NoSocioId = ns.Id
    WHERE v.ResultadoAcceso = 'Denegado'
      AND (@FechaDesde IS NULL OR CAST(v.FechaEntrada AS DATE) >= @FechaDesde)
      AND (@FechaHasta IS NULL OR CAST(v.FechaEntrada AS DATE) <= @FechaHasta)
    ORDER BY v.FechaEntrada DESC;
END
GO

-- =============================================
-- AUDIT
-- =============================================

CREATE OR ALTER PROCEDURE sp_AuditLog_Insert
    @UsuarioId INT = NULL,
    @Accion NVARCHAR(100),
    @Tabla NVARCHAR(100) = NULL,
    @RegistroId INT = NULL,
    @Detalle NVARCHAR(500) = NULL,
    @DireccionIP NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tbl_audit_log (UsuarioId, Accion, Tabla, RegistroId, Detalle, DireccionIP)
    VALUES (@UsuarioId, @Accion, @Tabla, @RegistroId, @Detalle, @DireccionIP);
END
GO

PRINT 'All stored procedures created successfully.';
GO