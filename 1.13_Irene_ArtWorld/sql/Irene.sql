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