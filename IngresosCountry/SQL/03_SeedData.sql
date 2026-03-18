USE IngresosCountry;
GO

-- =============================================
-- PAISES
-- =============================================

INSERT INTO tbl_paises (pais, cod_area) VALUES 
('Guatemala', '502'), 
('México', '52'), 
('Estados Unidos', '1'),
('El Salvador', '503'), 
('Honduras', '504'), 
('Costa Rica', '506'),
('Panamá', '507'), 
('Colombia', '57'), 
('España', '34'), 
('Argentina', '54');
GO

-- =============================================
-- TIPOS VISITA
-- =============================================

INSERT INTO tbl_visitas_nosocio_tipoVisita (descripcion, aplica_vencimiento, evento, activo) VALUES
('Proveedor', 1, 0, 1),
('Transportista', 0, 0, 1),
('Visitante Autorizado', 1, 0, 1),
('Mantenimiento', 0, 0, 1),
('Contratista', 1, 0, 1);
GO

-- =============================================
-- AREAS
-- =============================================

INSERT INTO tbl_visitas_areas (nombre, subarea) VALUES
('Entrada Principal', 'Portón Norte'),
('Entrada Servicio', 'Portón Sur'),
('Área Social', 'Central'),
('Piscina', 'Oeste'),
('Canchas', 'Este'),
('Restaurante', 'Central'),
('Eventos', 'Norte'),
('Gimnasio', 'Sur');
GO

-- =============================================
-- SOCIOS
-- =============================================

INSERT INTO tbl_socios (
    carne, nombre, cedula, sexo, telefono, celular,
    direccion, email1, fechaNacimiento, fechaIngreso, estatus
)
VALUES
(1001, 'Carlos Mendoza', '123456', 'M', '8888-1111', '8888-1111', 'San José', 'carlos@email.com', '1990-01-01', GETDATE(), 'Activo'),
(1002, 'María González', '234567', 'F', '8888-2222', '8888-2222', 'Heredia', 'maria@email.com', '1992-05-10', GETDATE(), 'Activo'),
(1003, 'Roberto Pérez', '345678', 'M', '8888-3333', '8888-3333', 'Alajuela', 'roberto@email.com', '1985-08-20', GETDATE(), 'Moroso');
GO

PRINT 'Seed data cargado correctamente';
GO