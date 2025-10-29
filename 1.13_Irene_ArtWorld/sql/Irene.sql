CREATE DATABASE ArtWorldDB;
GO

USE ArtWorldDB;
GO

CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Usuario NVARCHAR(50) NOT NULL UNIQUE,
    Contrasena NVARCHAR(100) NOT NULL
);
GO

INSERT INTO Usuarios (Usuario, Contrasena)
VALUES ('admin', '1234');
GO