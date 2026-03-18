-- =============================================
-- Ingresos Country - Seed Data
-- =============================================

USE IngresosCountryDB;
GO

-- Roles
INSERT INTO tbl_roles (Nombre, Descripcion) VALUES 
('Administrador', 'Acceso total al sistema'),
('Oficial de Seguridad', 'Gestión de accesos y seguridad'),
('Mesa de Servicio', 'Atención al socio y visitantes'),
('Finanzas', 'Reportes financieros y morosidad'),
('Gerencia', 'Reportes gerenciales y supervisión');
GO

-- Default Admin User (password: Admin123!)
-- Note: In production, use proper password hashing (BCrypt/PBKDF2)
INSERT INTO tbl_usuarios (Username, PasswordHash, NombreCompleto, Email, RolId)
VALUES ('admin', 'AQAAAAEAACcQAAAAEL2gR8Kv...REPLACE_WITH_HASHED_PASSWORD', 'Administrador del Sistema', 'admin@ingresoscountry.com', 1);
GO

-- Countries
INSERT INTO tbl_paises (Nombre, Codigo) VALUES 
('Guatemala', 'GT'), ('México', 'MX'), ('Estados Unidos', 'US'),
('El Salvador', 'SV'), ('Honduras', 'HN'), ('Costa Rica', 'CR'),
('Panamá', 'PA'), ('Colombia', 'CO'), ('España', 'ES'), ('Argentina', 'AR');
GO

-- Visit Types
INSERT INTO tbl_visitas_nosocio_tipoVisita (Nombre, Descripcion) VALUES
('Proveedor', 'Proveedor de bienes o servicios'),
('Transportista', 'Conductor de transporte o entregas'),
('Visitante Autorizado', 'Persona autorizada por la administración'),
('Mantenimiento', 'Personal de mantenimiento externo'),
('Contratista', 'Contratista de obras o proyectos');
GO

INSERT INTO tbl_visitas_nosocio_otroTipoVisita (Nombre, Descripcion) VALUES
('Familiar de Empleado', 'Familiar de un empleado del club'),
('Autoridad', 'Representante de autoridad gubernamental'),
('Medios', 'Representante de medios de comunicación');
GO

-- Access Areas
INSERT INTO tbl_visitas_areas (Nombre, Descripcion, Ubicacion) VALUES
('Entrada Principal', 'Garita de acceso principal', 'Portón Norte'),
('Entrada Servicio', 'Acceso de servicio y proveedores', 'Portón Sur'),
('Área Social', 'Salones y áreas comunes', 'Edificio Central'),
('Piscina', 'Área de piscinas', 'Sector Oeste'),
('Canchas Deportivas', 'Canchas de tenis, fútbol, etc.', 'Sector Este'),
('Restaurante', 'Área de restaurante y bar', 'Edificio Central'),
('Salón de Eventos', 'Salón para eventos privados', 'Edificio Norte'),
('Gimnasio', 'Área de gimnasio y spa', 'Edificio Sur');
GO

-- Sample Members
INSERT INTO tbl_socios (NumeroMembresia, Nombre, Apellido, DocumentoIdentidad, Email, Telefono, PaisId, TipoMembresia, Estado) VALUES
('SOC-001', 'Carlos', 'Mendoza', 'DPI-1234567', 'carlos.mendoza@email.com', '5555-1234', 1, 'Premium', 'Activo'),
('SOC-002', 'María', 'González', 'DPI-2345678', 'maria.gonzalez@email.com', '5555-2345', 1, 'Estándar', 'Activo'),
('SOC-003', 'Roberto', 'Pérez', 'DPI-3456789', 'roberto.perez@email.com', '5555-3456', 1, 'Premium', 'Moroso'),
('SOC-004', 'Ana', 'López', 'DPI-4567890', 'ana.lopez@email.com', '5555-4567', 1, 'Familiar', 'Activo'),
('SOC-005', 'Juan', 'Ramírez', 'DPI-5678901', 'juan.ramirez@email.com', '5555-5678', 1, 'Estándar', 'Suspendido');
GO

-- Reciprocity Clubs
INSERT INTO tbl_clubes_reciprocidad (NombreClub, Direccion, PaisId) VALUES
('Club Campestre de México', 'Ciudad de México, México', 2),
('Club Unión de San Salvador', 'San Salvador, El Salvador', 4),
('Club Honduras Country', 'Tegucigalpa, Honduras', 5);
GO

PRINT 'Seed data inserted successfully.';
GO