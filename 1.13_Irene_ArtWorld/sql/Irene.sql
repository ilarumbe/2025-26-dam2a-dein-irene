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
('La noche estrellada', 'Vincent van Gogh',
'“La noche estrellada” fue pintada por Vincent van Gogh en 1889 desde su habitación del asilo de Saint-Rémy-de-Provence. La obra plasma un paisaje nocturno cargado de emoción y dinamismo, donde las estrellas giran como remolinos de luz en un cielo vibrante y agitado. A través de sus trazos ondulantes y su paleta intensa, Van Gogh logra transmitir su visión interior del mundo, entre la esperanza y la tormenta emocional.',
'images/cuadro1.jpg'),

('El Guernica', 'Pablo Picasso',
'“El Guernica” fue pintado por Pablo Picasso en 1937 como respuesta al bombardeo de la ciudad vasca de Guernica durante la Guerra Civil Española. Con un estilo cubista monocromático, la obra representa la tragedia, el sufrimiento y el caos de la guerra. Los gritos de las figuras humanas y animales simbolizan el dolor universal frente a la violencia y la destrucción. Hoy es considerado uno de los más poderosos alegatos antibélicos de la historia del arte.',
'images/cuadro2.jpg'),

('El jardín de las delicias', 'El Bosco',
'“El jardín de las delicias” es un tríptico monumental creado por El Bosco alrededor de 1500. En el panel izquierdo se representa el Paraíso, en el central la humanidad entregada al placer y la lujuria, y en el derecho el infierno con visiones de castigo y desesperación. La obra combina lo religioso, lo fantástico y lo simbólico en un despliegue de imaginación que sigue fascinando a historiadores y espectadores siglos después.',
'images/cuadro3.jpg'),

('La ronda de noche', 'Rembrandt van Rijn',
'“La ronda de noche”, terminada en 1642, es una de las pinturas más célebres del Siglo de Oro neerlandés. Encargada por la compañía de arcabuceros de Ámsterdam, la escena retrata a los milicianos en movimiento, en un juego magistral de luces y sombras. Rembrandt rompe con la rigidez del retrato de grupo tradicional y convierte la composición en una obra teatral llena de energía, vida y profundidad psicológica.',
'images/cuadro4.jpg');
GO