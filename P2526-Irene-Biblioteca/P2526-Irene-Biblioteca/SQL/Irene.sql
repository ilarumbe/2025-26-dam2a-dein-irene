CREATE DATABASE P2526_Irene_Biblioteca;
GO

USE P2526_Irene_Biblioteca;
GO

CREATE TABLE Empleados (
    idEmpleado INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    usuario VARCHAR(50) NOT NULL UNIQUE,
    password VARCHAR(200) NOT NULL
);

CREATE TABLE Autores (
    idAutor INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(150) NOT NULL,
    nacionalidad VARCHAR(100)
);

CREATE TABLE Categorias (
    idCategoria INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL
);

CREATE TABLE Clientes (
    idCliente INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    usuario VARCHAR(50) NOT NULL UNIQUE,
    password VARCHAR(200) NOT NULL
);

CREATE TABLE Libros (
    idLibro INT IDENTITY(1,1) PRIMARY KEY,
    titulo VARCHAR(200) NOT NULL,
    año INT NOT NULL,
    idAutor INT NOT NULL,
    idCategoria INT NOT NULL,
    stock INT NOT NULL DEFAULT 0,

    FOREIGN KEY (idAutor) REFERENCES Autores(idAutor),
    FOREIGN KEY (idCategoria) REFERENCES Categorias(idCategoria)
);

CREATE TABLE Prestamos (
    idPrestamo INT IDENTITY(1,1) PRIMARY KEY,
    idLibro INT NOT NULL,
    idCliente INT NOT NULL,
    idEmpleado INT NOT NULL,
    fechaPrestamo DATE NOT NULL,
    fechaDevolucion DATE NULL,
    devuelto BIT NOT NULL DEFAULT 0,

    FOREIGN KEY (idLibro) REFERENCES Libros(idLibro),
    FOREIGN KEY (idCliente) REFERENCES Clientes(idCliente),
    FOREIGN KEY (idEmpleado) REFERENCES Empleados(idEmpleado)
);

INSERT INTO Empleados (nombre, usuario, password) VALUES
('Administrador General', 'admin', 'YbSBswFz0bwEUm50hQcQ5Qp5FyMBcPsIsuD6oHu2mEw='), /*admin123*/
('Laura Méndez', 'lmendez', 'h+JpP7mBCHuAtdX9VvCY9Q=='), /*1234*/
('Carlos Ruiz', 'cruiz', '8bqX8OojVohCsCk0ZrbJlA=='), /*pass1*/
('Ana Torres', 'atorres', 'bg3fnlK11GIq2xZmwDSbPw=='); /*pass2*/      

INSERT INTO Autores (nombre, nacionalidad) VALUES
('Gabriel García Márquez', 'Colombia'),
('J. K. Rowling', 'Reino Unido'),
('Stephen King', 'Estados Unidos'),
('Isabel Allende', 'Chile'),
('Haruki Murakami', 'Japón'),
('George R. R. Martin', 'Estados Unidos'),
('J.R.R. Tolkien', 'Reino Unido'),
('Miguel de Cervantes', 'España'),
('Paulo Coelho', 'Brasil'),
('Dan Brown', 'Estados Unidos');

INSERT INTO Categorias (nombre) VALUES
('Novela'),
('Fantasía'),
('Terror'),
('Ciencia Ficción'),
('Romance'),
('Histórico'),
('Autoayuda'),
('Aventura');

INSERT INTO Libros (titulo, año, idAutor, idCategoria, stock) VALUES
('Cien años de soledad', 1967, 1, 1, 12),
('El amor en los tiempos del cólera', 1985, 1, 1, 8),
('Harry Potter y la piedra filosofal', 1997, 2, 2, 20),
('Harry Potter y la cámara secreta', 1998, 2, 2, 15),
('It', 1986, 3, 3, 5),
('El resplandor', 1977, 3, 3, 6),
('La casa de los espíritus', 1982, 4, 1, 10),
('Tokio Blues', 1987, 5, 1, 7),
('Juego de Tronos', 1996, 6, 2, 18),
('El Señor de los Anillos - La Comunidad del Anillo', 1954, 7, 2, 14),
('El Alquimista', 1988, 9, 7, 30),
('El Código Da Vinci', 2003, 10, 8, 12),
('Don Quijote de la Mancha', 1605, 8, 6, 11),
('Crónicas Marcianas', 1950, 3, 4, 4),
('After Dark', 2004, 5, 1, 6),
('Fuego y Sangre', 2018, 6, 6, 9);

INSERT INTO Clientes (nombre, usuario, password) VALUES
('María López', 'mlopez', 'X1gJjFUPAIxYH1WbVNkHQg=='), /*cliente1*/
('Juan Hernández', 'jhernandez', 'YI6Cwbr+lpQGqsHllcXHQg=='), /*cliente2*/
('Sofía Ríos', 'srios', '8bqX8OojVohCsCk0ZrbJlA=='), /*pass1*/
('Pedro Salas', 'psalas', 'bg3fnlK11GIq2xZmwDSbPw=='), /*pass2*/
('Lucía Vega', 'lvega', 'dzU6bQ+1L0e/aGqzcehVug=='); /*pass3*/

INSERT INTO Prestamos (idLibro, idCliente, idEmpleado, fechaPrestamo, fechaDevolucion, devuelto) VALUES
(1, 1, 2, '2024-01-10', '2024-01-20', 1),
(3, 2, 1, '2024-01-12', NULL, 0),
(5, 3, 3, '2024-01-05', '2024-01-15', 1),
(9, 4, 2, '2024-01-16', NULL, 0),
(11, 5, 4, '2024-01-01', '2024-01-10', 1),
(14, 5, 2, '2024-01-18', NULL, 0),
(7, 3, 1, '2024-01-03', '2024-01-12', 1),
(10, 2, 3, '2024-01-07', NULL, 0);

