USE IngresosCountry;
GO

INSERT INTO tbl_paises (Nombre, Codigo) VALUES 
('Guatemala', 'GT'), 
('México', 'MX'), 
('Estados Unidos', 'US'),
('El Salvador', 'SV'), 
('Honduras', 'HN'), 
('Costa Rica', 'CR'),
('Panamá', 'PA'), 
('Colombia', 'CO'), 
('España', 'ES'), 
('Argentina', 'AR');
GO

INSERT INTO tbl_visitas_nosocio_tipoVisita (Nombre, Descripcion) VALUES
('Proveedor', 'Proveedor de bienes o servicios'),
('Transportista', 'Conductor de transporte o entregas'),
('Visitante Autorizado', 'Persona autorizada'),
('Mantenimiento', 'Personal de mantenimiento'),
('Contratista', 'Contratista externo');
GO

INSERT INTO tbl_visitas_areas (Nombre, Descripcion, Ubicacion) VALUES
('Entrada Principal', 'Acceso principal', 'Portón Norte'),
('Entrada Servicio', 'Acceso proveedores', 'Portón Sur'),
('Área Social', 'Áreas comunes', 'Central'),
('Piscina', 'Zona de piscina', 'Oeste'),
('Canchas', 'Área deportiva', 'Este'),
('Restaurante', 'Restaurante del club', 'Central'),
('Eventos', 'Salón eventos', 'Norte'),
('Gimnasio', 'Gym y spa', 'Sur');
GO

INSERT INTO tbl_socios (NumeroMembresia, Nombre, Apellido, DocumentoIdentidad, Email, Telefono, PaisId, TipoMembresia, Estado) VALUES
('SOC-001', 'Carlos', 'Mendoza', '123456', 'carlos@email.com', '8888-1111', 6, 'Premium', 'Activo'),
('SOC-002', 'María', 'González', '234567', 'maria@email.com', '8888-2222', 6, 'Estándar', 'Activo'),
('SOC-003', 'Roberto', 'Pérez', '345678', 'roberto@email.com', '8888-3333', 6, 'Premium', 'Moroso'),
('SOC-004', 'Ana', 'López', '456789', 'ana@email.com', '8888-4444', 6, 'Familiar', 'Activo'),
('SOC-005', 'Juan', 'Ramírez', '567890', 'juan@email.com', '8888-5555', 6, 'Estándar', 'Suspendido');
GO

INSERT INTO tbl_clubes_reciprocidad (NombreClub, Direccion, PaisId) VALUES
('Club México', 'CDMX', 2),
('Club El Salvador', 'San Salvador', 4),
('Club Honduras', 'Tegucigalpa', 5);
GO

PRINT 'Seed cargado correctamente';
GO