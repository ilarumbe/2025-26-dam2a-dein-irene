CREATE DATABASE VideojuegosDB;
GO

USE VideojuegosDB;
GO

CREATE TABLE Videojuegos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(100) NOT NULL,
    Genero NVARCHAR(50) NOT NULL,
    Plataforma NVARCHAR(50) NOT NULL,
    Desarrollador NVARCHAR(100) NOT NULL,
    FechaLanzamiento DATE NOT NULL,
    Precio DECIMAL(10,2) NOT NULL
);

-- Inserción de 10 registros de prueba
INSERT INTO Videojuegos (Titulo, Genero, Plataforma, Desarrollador, FechaLanzamiento, Precio)
VALUES
('The Legend of Zelda', 'Aventura', 'Nintendo Switch', 'Nintendo', '2017-03-03', 59.99),
('God of War', 'Acción', 'PS4', 'Santa Monica Studio', '2018-04-20', 39.99),
('Cyberpunk 2077', 'RPG', 'PC', 'CD Projekt', '2020-12-10', 49.99),
('Minecraft', 'Sandbox', 'Multiplataforma', 'Mojang', '2011-11-18', 26.95),
('FIFA 23', 'Deportes', 'PS5', 'EA Sports', '2022-09-27', 69.99),
('Call of Duty: Modern Warfare', 'FPS', 'Xbox', 'Infinity Ward', '2019-10-25', 59.99),
('Super Mario Odyssey', 'Plataformas', 'Nintendo Switch', 'Nintendo', '2017-10-27', 59.99),
('Among Us', 'Multijugador', 'PC', 'InnerSloth', '2018-06-15', 4.99),
('Hollow Knight', 'Metroidvania', 'PC', 'Team Cherry', '2017-02-24', 14.99),
('Elden Ring', 'RPG', 'PS5', 'FromSoftware', '2022-02-25', 69.99);