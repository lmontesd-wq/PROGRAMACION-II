PRAGMA foreign_keys = ON;

CREATE TABLE IF NOT EXISTS Usuario (
    IdUsuario INTEGER PRIMARY KEY AUTOINCREMENT,
    NombreUsuario TEXT NOT NULL UNIQUE,
    Clave TEXT NOT NULL,
    NombreCompleto TEXT NOT NULL,
    Rol TEXT NOT NULL CHECK (Rol IN ('Administrador','Recepcionista','Veterinario')),
    Activo INTEGER NOT NULL DEFAULT 1
);

CREATE TABLE IF NOT EXISTS Propietario (
    IdPropietario INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT NOT NULL,
    Telefono TEXT,
    Correo TEXT,
    Direccion TEXT
);

CREATE TABLE IF NOT EXISTS Mascota (
    IdMascota INTEGER PRIMARY KEY AUTOINCREMENT,
    IdPropietario INTEGER NOT NULL,
    Nombre TEXT NOT NULL,
    Especie TEXT NOT NULL,
    Raza TEXT,
    Sexo TEXT,
    FechaNacimiento TEXT,
    FOREIGN KEY (IdPropietario) REFERENCES Propietario(IdPropietario)
);

CREATE TABLE IF NOT EXISTS Cita (
    IdCita INTEGER PRIMARY KEY AUTOINCREMENT,
    IdMascota INTEGER NOT NULL,
    IdVeterinario INTEGER NOT NULL,
    FechaHora TEXT NOT NULL,
    Motivo TEXT,
    Estado TEXT NOT NULL DEFAULT 'Pendiente',
    FOREIGN KEY (IdMascota) REFERENCES Mascota(IdMascota),
    FOREIGN KEY (IdVeterinario) REFERENCES Usuario(IdUsuario)
);

CREATE TABLE IF NOT EXISTS Consulta (
    IdConsulta INTEGER PRIMARY KEY AUTOINCREMENT,
    IdCita INTEGER NOT NULL,
    Diagnostico TEXT,
    Tratamiento TEXT,
    Observaciones TEXT,
    FOREIGN KEY (IdCita) REFERENCES Cita(IdCita)
);

CREATE TABLE IF NOT EXISTS Vacunacion (
    IdVacunacion INTEGER PRIMARY KEY AUTOINCREMENT,
    IdMascota INTEGER NOT NULL,
    Vacuna TEXT NOT NULL,
    FechaAplicacion TEXT NOT NULL,
    ProximaDosis TEXT,
    FOREIGN KEY (IdMascota) REFERENCES Mascota(IdMascota)
);
