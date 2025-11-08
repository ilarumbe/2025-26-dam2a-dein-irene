/*CREATE DATABASE ArtWorldDB;
GO*/

USE ArtWorldDB;
GO

CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Usuario NVARCHAR(50) NOT NULL UNIQUE,
    Contrasena NVARCHAR(100) NOT NULL
);
GO

INSERT INTO Usuarios (Usuario, Contrasena)
VALUES ('admin', 'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg='), /* admin */
('usuario1', 'XohImNooBHFR0OVvjcYpJ3NgPQ1qq73WKhHvch0VQtg='); /* password */
GO

CREATE TABLE Cuadros (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Autor NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(MAX),
    Imagen NVARCHAR(200)
);
GO

INSERT INTO Cuadros (Nombre, Autor, Descripcion, Imagen) VALUES
('La noche estrellada', 'Vincent van Gogh', 'Obra postimpresionista pintada en 1889 desde la habitación del asilo de Saint-Rémy-de-Provence.', 'images/cuadro1.jpg'),
('El Guernica', 'Pablo Picasso', 'Representa los horrores de la guerra civil española, pintado en 1937.', 'images/cuadro2.jpg'),
('El jardín de las delicias', 'El Bosco', 'Tríptico alegórico sobre el paraíso, el pecado y el infierno.', 'images/cuadro3.jpg'),
('La ronda de noche', 'Rembrandt', 'Una de las pinturas más famosas del Siglo de Oro neerlandés.', 'images/cuadro4.jpg');
GO