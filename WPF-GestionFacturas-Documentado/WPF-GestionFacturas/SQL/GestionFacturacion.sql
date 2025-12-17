-- 1. CREAR LA BASE DE DATOS
CREATE DATABASE GestionFacturacion;
GO
USE GestionFacturacion;
GO

-- 2. TABLA CLIENTES
CREATE TABLE Clientes (
    IdCliente INT IDENTITY(1,1) PRIMARY KEY,
    NIF NVARCHAR(20) NOT NULL UNIQUE,      -- Documento fiscal único
    NombreRazonSocial NVARCHAR(100) NOT NULL,
    Direccion NVARCHAR(150),
    Telefono NVARCHAR(20),
    Email NVARCHAR(100),
    FechaAlta DATETIME DEFAULT GETDATE()
);
GO

-- 3. TABLA PRODUCTOS
CREATE TABLE Productos (
    IdProducto INT IDENTITY(1,1) PRIMARY KEY,
    Codigo NVARCHAR(50) UNIQUE,            -- Referencia interna (SKU)
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255),
    PrecioVenta DECIMAL(10, 2) NOT NULL,   -- Precio actual del catálogo
    Stock INT DEFAULT 0,                   -- Control de inventario
    Activo BIT DEFAULT 1                   -- Para no borrar productos, solo desactivarlos
);
GO

-- 4. TABLA FACTURAS (Cabecera)
CREATE TABLE Facturas (
    IdFactura INT IDENTITY(1,1) PRIMARY KEY,
    IdCliente INT NOT NULL,
    NumeroFactura NVARCHAR(50) NOT NULL,   -- Ej: "2023/001"
    Fecha DATETIME DEFAULT GETDATE(),
    Observaciones NVARCHAR(MAX),
    
    -- Relación con Clientes
    CONSTRAINT FK_Facturas_Clientes FOREIGN KEY (IdCliente) 
    REFERENCES Clientes(IdCliente)
);
GO

-- 5. TABLA FACTURAS_DETALLE (Líneas)
CREATE TABLE FacturasDetalle (
    IdDetalle INT IDENTITY(1,1) PRIMARY KEY,
    IdFactura INT NOT NULL,
    IdProducto INT NOT NULL,
    
    Cantidad INT NOT NULL DEFAULT 1,
    -- IMPORTANTE: Guardamos el precio al momento de la venta
    PrecioUnitario DECIMAL(10, 2) NOT NULL, 
    
    -- Campo calculado automáticamente por SQL Server (Cantidad * Precio)
    Subtotal AS (Cantidad * PrecioUnitario) PERSISTED, 

    -- Relaciones
    CONSTRAINT FK_Detalle_Factura FOREIGN KEY (IdFactura) 
    REFERENCES Facturas(IdFactura) ON DELETE CASCADE, -- Si borras factura, se borran líneas
    
    CONSTRAINT FK_Detalle_Producto FOREIGN KEY (IdProducto) 
    REFERENCES Productos(IdProducto)
);
GO
USE GestionFacturacion;
GO

-- 1. LIMPIEZA DE DATOS (Reiniciamos contadores de ID)
-- DELETE FROM FacturasDetalle;
-- DELETE FROM Facturas;
-- DELETE FROM Clientes;
-- DELETE FROM Productos;

-- DBCC CHECKIDENT ('FacturasDetalle', RESEED, 1);
-- DBCC CHECKIDENT ('Facturas', RESEED, 1);
-- DBCC CHECKIDENT ('Clientes', RESEED, 1);
-- DBCC CHECKIDENT ('Productos', RESEED, 1);
-- GO

-- 2. INSERTAR CLIENTES (15 Registros)
INSERT INTO Clientes (NIF, NombreRazonSocial, Direccion, Telefono, Email) VALUES 
('B11111111', 'Tech Solutions S.L.', 'Calle Innovación 7, Madrid', '910001111', 'contacto@techsolutions.es'),
('B22222222', 'Restaurante El Buen Sabor', 'Av. Gastronomía 23, Barcelona', '930002222', 'reservas@buensabor.com'),
('A33333333', 'Construcciones y Reformas Paco', 'Polígono Industrial Norte, Nave 5, Valencia', '960003333', 'paco@reformas.es'),
('B44444444', 'Librería Cervantes', 'Plaza Mayor 1, Salamanca', '923004444', 'info@libreriacervantes.com'),
('A55555555', 'Transportes Rápidos SA', 'Ctra. Nacional IV, Km 20, Sevilla', '950005555', 'trafico@transrapidos.com'),
('B66666666', 'Consultoría Legal Martínez', 'Gran Vía 45, 3A, Madrid', '910006666', 'martinez@legal.com'),
('B77777777', 'Clínica Dental Sonrisas', 'Calle Salud 88, Bilbao', '940007777', 'cita@sonrisas.com'),
('A88888888', 'Supermercados del Barrio', 'Calle Comercio 12, Zaragoza', '976008888', 'compras@delbarrio.es'),
('B99999999', 'Gimnasio Hércules', 'Calle Deporte s/n, Málaga', '952009999', 'info@gymhercules.com'),
('B10101010', 'Academia de Idiomas Hello', 'Av. Libertad 5, Alicante', '965001010', 'hello@academia.com'),
('A12121212', 'Industrias Metálicas Fer', 'Polígono Sur, Parcela 2, Vigo', '986001212', 'ventas@metalfer.com'),
('B13131313', 'Diseño Gráfico Pixel', 'Calle Arte 9, Barcelona', '930001313', 'pixel@design.com'),
('B14141414', 'Hotel Vista Mar', 'Paseo Marítimo 100, Cádiz', '956001414', 'reservas@vistamar.com'),
('A15151515', 'Automoción Veloz', 'Calle Motor 66, Madrid', '910001515', 'taller@veloz.com'),
('B16161616', 'Panadería La Migaja', 'Calle Horno 3, Toledo', '925001616', 'pedidos@lamigaja.com');

-- 3. INSERTAR PRODUCTOS (20 Registros variados)
INSERT INTO Productos (Codigo, Nombre, Descripcion, PrecioVenta, Stock) VALUES 
('HW-001', 'Portátil Dell Latitude', 'i7, 16GB RAM, 512GB SSD', 1250.00, 15),
('HW-002', 'Monitor LG 24"', 'Full HD, IPS, HDMI', 145.50, 30),
('HW-003', 'Ratón Logitech Inalámbrico', 'Ergonómico, batería larga duración', 25.99, 100),
('HW-004', 'Teclado Mecánico', 'RGB, Switch Blue', 89.90, 20),
('HW-005', 'Impresora HP LaserJet', 'Monocromo, Wifi', 180.00, 10),
('OF-001', 'Paquete Folios A4', '80gr, 500 hojas', 4.50, 500),
('OF-002', 'Bolígrafos Bic Azul (Pack 10)', 'Punta media', 3.20, 200),
('OF-003', 'Archivador AZ', 'Lomo ancho, color negro', 2.80, 150),
('OF-004', 'Silla de Oficina Ergonómica', 'Respaldo malla, reclinable', 120.00, 25),
('OF-005', 'Mesa Escritorio 140cm', 'Blanco, estructura metálica', 95.00, 15),
('SV-001', 'Servicio Consultoría Hora', 'Asesoramiento técnico', 60.00, 999),
('SV-002', 'Instalación Software', 'Configuración remota', 45.00, 999),
('SV-003', 'Mantenimiento Mensual', 'Soporte 24/7', 150.00, 999),
('AU-001', 'Cambio de Aceite', 'Mano de obra incluida', 55.00, 999),
('AU-002', 'Filtro de Aire Coche', 'Universal', 15.00, 40),
('AU-003', 'Neumático Pirelli 205/55', 'R16 91V', 85.00, 50),
('AU-004', 'Batería Coche 70Ah', 'Garantía 2 años', 90.00, 12),
('EL-001', 'Cable HDMI 2m', '4K Soporte', 7.50, 80),
('EL-002', 'Regleta 5 Enchufes', 'Con interruptor', 12.00, 60),
('EL-003', 'Disco Duro Externo 1TB', 'USB 3.0', 55.00, 25);
GO

-- 4. GENERAR FACTURAS ALEATORIAS (Script dinámico)
DECLARE @i INT = 1;
DECLARE @TotalFacturas INT = 50; -- ¡Generaremos 50 facturas!

WHILE @i <= @TotalFacturas
BEGIN
    -- A) Datos aleatorios para la cabecera
    DECLARE @IdClienteRandom INT;
    SELECT TOP 1 @IdClienteRandom = IdCliente FROM Clientes ORDER BY NEWID(); -- Cliente aleatorio
    
    DECLARE @FechaRandom DATETIME;
    SET @FechaRandom = DATEADD(DAY, -FLOOR(RAND()*(365)), GETDATE()); -- Fecha aleatoria último año
    
    DECLARE @NumeroFactura NVARCHAR(50);
    SET @NumeroFactura = CONCAT(YEAR(@FechaRandom), '/', RIGHT('000' + CAST(@i AS VARCHAR(10)), 3));

    -- B) Insertar Factura
    INSERT INTO Facturas (IdCliente, NumeroFactura, Fecha, Observaciones)
    VALUES (@IdClienteRandom, @NumeroFactura, @FechaRandom, 'Factura generada automáticamente');

    -- C) Obtener ID de la factura recién creada
    DECLARE @IdFacturaNueva INT = SCOPE_IDENTITY();

    -- D) Insertar Líneas de Detalle (Entre 1 y 5 productos por factura)
    DECLARE @j INT = 1;
    DECLARE @NumLineas INT = FLOOR(RAND()*(5-1+1)+1); -- Random entre 1 y 5

    WHILE @j <= @NumLineas
    BEGIN
        DECLARE @IdProductoRandom INT;
        DECLARE @PrecioActual DECIMAL(10,2);
        
        -- Seleccionar producto y su precio actual
        SELECT TOP 1 @IdProductoRandom = IdProducto, @PrecioActual = PrecioVenta 
        FROM Productos ORDER BY NEWID();

        DECLARE @CantidadRandom INT = FLOOR(RAND()*(10-1+1)+1); -- Cantidad entre 1 y 10

        -- Insertar Detalle
        INSERT INTO FacturasDetalle (IdFactura, IdProducto, Cantidad, PrecioUnitario)
        VALUES (@IdFacturaNueva, @IdProductoRandom, @CantidadRandom, @PrecioActual);

        SET @j = @j + 1;
    END

    SET @i = @i + 1;
END
GO

-- 5. VERIFICACIÓN FINAL
SELECT 'Clientes' as Tabla, COUNT(*) as Total FROM Clientes
UNION ALL
SELECT 'Productos', COUNT(*) FROM Productos
UNION ALL
SELECT 'Facturas', COUNT(*) FROM Facturas
UNION ALL
SELECT 'Lineas Detalle', COUNT(*) FROM FacturasDetalle;

-- Ver una muestra bonita
SELECT TOP 20
    f.NumeroFactura,
    FORMAT(f.Fecha, 'dd/MM/yyyy') as Fecha,
    c.NombreRazonSocial,
    p.Nombre as Producto,
    fd.Cantidad,
    fd.PrecioUnitario,
    fd.Subtotal
FROM FacturasDetalle fd
JOIN Facturas f ON fd.IdFactura = f.IdFactura
JOIN Clientes c ON f.IdCliente = c.IdCliente
JOIN Productos p ON fd.IdProducto = p.IdProducto
ORDER BY f.Fecha DESC;
