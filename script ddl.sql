
-- Usar la base de datos recién creada
USE persona_db;
GO

-- Tabla: persona
CREATE TABLE persona (
    cc INT NOT NULL PRIMARY KEY,
    nombre VARCHAR(45),
    apellido VARCHAR(45),
    edad INT,
    genero CHAR(1)
);
GO

-- Tabla: profesion
CREATE TABLE profesion (
    id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    nom VARCHAR(90),
    des TEXT
);
GO

-- Tabla: estudios (relación muchos a muchos entre persona y profesion)
CREATE TABLE estudios (
    id_prof INT NOT NULL,
    cc_per INT NOT NULL,
    fecha DATE,
    univer VARCHAR(50),
    PRIMARY KEY (id_prof, cc_per),
    CONSTRAINT FK_estudios_cc_per FOREIGN KEY (cc_per) REFERENCES persona(cc) ON DELETE CASCADE,
    CONSTRAINT FK_estudios_id_prof FOREIGN KEY (id_prof) REFERENCES profesion(id) ON DELETE CASCADE
);
GO

-- Tabla: telefono
CREATE TABLE telefono (
    num VARCHAR(15) NOT NULL PRIMARY KEY,
    oper VARCHAR(45),
    duenio INT,
    CONSTRAINT FK_telefono_duenio FOREIGN KEY (duenio) REFERENCES persona(cc) ON DELETE CASCADE
);
GO
