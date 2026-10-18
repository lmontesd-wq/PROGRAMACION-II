PRAGMA foreign_keys = ON;

-- ============================================================
-- TABLAS
-- ============================================================
CREATE TABLE IF NOT EXISTS Lector (
    Id       INTEGER PRIMARY KEY AUTOINCREMENT,
    Codigo   TEXT NOT NULL UNIQUE,
    Correo   TEXT NOT NULL,
    Telefono TEXT NOT NULL,
    CHECK (length(trim(Codigo)) > 0)
);

CREATE TABLE IF NOT EXISTS Libro (
    Id     INTEGER PRIMARY KEY AUTOINCREMENT,
    Codigo TEXT NOT NULL UNIQUE,
    Titulo TEXT NOT NULL,
    Autor  TEXT NOT NULL,
    Stock  INTEGER NOT NULL,
    CHECK (Stock >= 0)
);

CREATE TABLE IF NOT EXISTS Prestamo (
    Id              INTEGER PRIMARY KEY AUTOINCREMENT,
    LectorId        INTEGER NOT NULL,
    LibroId         INTEGER NOT NULL,
    FechaPrestamo   TEXT NOT NULL,
    FechaLimite     TEXT NOT NULL,
    FechaDevolucion TEXT NULL,
    CHECK (date(FechaLimite) >= date(FechaPrestamo)),
    FOREIGN KEY (LectorId) REFERENCES Lector(Id) ON DELETE CASCADE,
    FOREIGN KEY (LibroId)  REFERENCES Libro(Id)  ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS IX_Prestamo_LectorId ON Prestamo(LectorId);
CREATE INDEX IF NOT EXISTS IX_Prestamo_LibroId  ON Prestamo(LibroId);

-- ============================================================
-- TRIGGERS
-- ============================================================

-- 1) Validaciones antes de registrar un prestamo
CREATE TRIGGER IF NOT EXISTS trg_prestamo_validar_insert
BEFORE INSERT ON Prestamo
BEGIN
    SELECT RAISE(ABORT, 'Fechas invalidas: use el formato yyyy-MM-dd.')
    WHERE date(NEW.FechaPrestamo) IS NULL OR date(NEW.FechaLimite) IS NULL;

    SELECT RAISE(ABORT, 'No hay stock disponible del libro.')
    WHERE (SELECT Stock FROM Libro WHERE Id = NEW.LibroId) <= 0;

    SELECT RAISE(ABORT, 'El lector ya tiene este libro prestado sin devolver.')
    WHERE EXISTS (SELECT 1 FROM Prestamo
                  WHERE LectorId = NEW.LectorId
                    AND LibroId = NEW.LibroId
                    AND FechaDevolucion IS NULL);
END;

-- 2) Descontar stock al prestar
CREATE TRIGGER IF NOT EXISTS trg_prestamo_descontar_stock
AFTER INSERT ON Prestamo
WHEN NEW.FechaDevolucion IS NULL
BEGIN
    UPDATE Libro SET Stock = Stock - 1 WHERE Id = NEW.LibroId;
END;

-- 3) Validar la devolucion
CREATE TRIGGER IF NOT EXISTS trg_prestamo_validar_devolucion
BEFORE UPDATE OF FechaDevolucion ON Prestamo
WHEN NEW.FechaDevolucion IS NOT NULL
BEGIN
    SELECT RAISE(ABORT, 'Fecha de devolucion invalida: use el formato yyyy-MM-dd.')
    WHERE date(NEW.FechaDevolucion) IS NULL;

    SELECT RAISE(ABORT, 'La devolucion no puede ser anterior al prestamo.')
    WHERE date(NEW.FechaDevolucion) < date(OLD.FechaPrestamo);

    SELECT RAISE(ABORT, 'El prestamo ya fue devuelto.')
    WHERE OLD.FechaDevolucion IS NOT NULL;
END;

-- 4) Reponer stock al devolver
CREATE TRIGGER IF NOT EXISTS trg_prestamo_reponer_stock
AFTER UPDATE OF FechaDevolucion ON Prestamo
WHEN OLD.FechaDevolucion IS NULL AND NEW.FechaDevolucion IS NOT NULL
BEGIN
    UPDATE Libro SET Stock = Stock + 1 WHERE Id = NEW.LibroId;
END;

-- 5) Reponer stock si se borra un prestamo activo (tambien por cascada al borrar un lector)
CREATE TRIGGER IF NOT EXISTS trg_prestamo_borrar_reponer
AFTER DELETE ON Prestamo
WHEN OLD.FechaDevolucion IS NULL
BEGIN
    UPDATE Libro SET Stock = Stock + 1 WHERE Id = OLD.LibroId;
END;

-- 6) No cambiar lector ni libro de un prestamo existente
CREATE TRIGGER IF NOT EXISTS trg_prestamo_bloquear_cambio_llaves
BEFORE UPDATE OF LectorId, LibroId ON Prestamo
BEGIN
    SELECT RAISE(ABORT, 'No se puede cambiar el lector o el libro de un prestamo.');
END;
