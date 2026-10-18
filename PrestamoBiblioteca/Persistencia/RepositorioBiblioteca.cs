using System.Data;
using Microsoft.Data.Sqlite;
using PrestamoBiblioteca.Dominio;

namespace PrestamoBiblioteca.Persistencia;

internal sealed class RepositorioBiblioteca
{
    private const string FormatoFecha = "yyyy-MM-dd";
    private readonly string _cadenaConexion;

    public RepositorioBiblioteca(string cadenaConexion)
    {
        _cadenaConexion = cadenaConexion;
    }

    // CRUD de lectores
    public DataTable ObtenerLectores() => Consultar(
        "SELECT Id, Codigo, Correo, Telefono FROM Lector ORDER BY Codigo;");

    public void CrearLector(Lector lector) => Ejecutar(
        "INSERT INTO Lector (Codigo, Correo, Telefono) VALUES ($codigo, $correo, $telefono);",
        ("$codigo", lector.Codigo), ("$correo", lector.Correo), ("$telefono", lector.Telefono));

    public int ActualizarLector(int id, Lector lector) => Ejecutar(
        "UPDATE Lector SET Codigo = $codigo, Correo = $correo, Telefono = $telefono WHERE Id = $id;",
        ("$codigo", lector.Codigo), ("$correo", lector.Correo),
        ("$telefono", lector.Telefono), ("$id", id));

    public int EliminarLector(int id) => Ejecutar(
        "DELETE FROM Lector WHERE Id = $id;", ("$id", id));

    // CRUD de libros
    public DataTable ObtenerLibros() => Consultar(
        "SELECT Id, Codigo, Titulo, Autor, Stock FROM Libro ORDER BY Titulo;");

    public void CrearLibro(Libro libro) => Ejecutar(
        "INSERT INTO Libro (Codigo, Titulo, Autor, Stock) VALUES ($codigo, $titulo, $autor, $stock);",
        ("$codigo", libro.Codigo), ("$titulo", libro.Titulo),
        ("$autor", libro.Autor), ("$stock", libro.Stock));

    public int ActualizarLibro(int id, Libro libro) => Ejecutar(
        "UPDATE Libro SET Codigo = $codigo, Titulo = $titulo, Autor = $autor, Stock = $stock WHERE Id = $id;",
        ("$codigo", libro.Codigo), ("$titulo", libro.Titulo),
        ("$autor", libro.Autor), ("$stock", libro.Stock), ("$id", id));

    public int EliminarLibro(int id) => Ejecutar(
        "DELETE FROM Libro WHERE Id = $id;", ("$id", id));

    // Prestamos (el stock lo controlan los triggers de la base de datos)
    public DataTable ObtenerPrestamos(bool soloActivos = false) => Consultar($@"SELECT p.Id,
        l.Codigo AS CodigoLector, b.Codigo AS CodigoLibro, b.Titulo,
        p.FechaPrestamo, p.FechaLimite, p.FechaDevolucion
        FROM Prestamo p
        INNER JOIN Lector l ON l.Id = p.LectorId
        INNER JOIN Libro b ON b.Id = p.LibroId
        {(soloActivos ? "WHERE p.FechaDevolucion IS NULL" : "")}
        ORDER BY p.FechaPrestamo DESC, p.Id DESC;");

    public void CrearPrestamo(Prestamo prestamo) => Ejecutar(
        "INSERT INTO Prestamo (LectorId, LibroId, FechaPrestamo, FechaLimite) VALUES ($lector, $libro, $prestamo, $limite);",
        ("$lector", prestamo.LectorId), ("$libro", prestamo.LibroId),
        ("$prestamo", prestamo.FechaPrestamo.ToString(FormatoFecha)),
        ("$limite", prestamo.FechaLimite.ToString(FormatoFecha)));

    public int RegistrarDevolucion(int id, DateTime fecha) => Ejecutar(
        "UPDATE Prestamo SET FechaDevolucion = $fecha WHERE Id = $id;",
        ("$fecha", fecha.ToString(FormatoFecha)), ("$id", id));

    public int EliminarPrestamo(int id) => Ejecutar(
        "DELETE FROM Prestamo WHERE Id = $id;", ("$id", id));

    private DataTable Consultar(string sql)
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = sql;
        using var lector = comando.ExecuteReader();
        var tabla = new DataTable();
        tabla.Load(lector);
        return tabla;
    }

    private int Ejecutar(string sql, params (string Nombre, object Valor)[] parametros)
    {
        try
        {
            using var conexion = AbrirConexion();
            using var comando = conexion.CreateCommand();
            comando.CommandText = sql;
            foreach (var (nombre, valor) in parametros)
                comando.Parameters.AddWithValue(nombre, valor ?? DBNull.Value);
            return comando.ExecuteNonQuery();
        }
        catch (SqliteException error) when (error.SqliteErrorCode == 19)
        {
            // Mensajes de los triggers (RAISE) y de las restricciones de SQLite
            throw new InvalidOperationException(LimpiarMensaje(error.Message));
        }
    }

    private static string LimpiarMensaje(string mensaje)
    {
        if (mensaje.Contains("FOREIGN KEY constraint failed"))
            return "Operacion no permitida: el registro esta relacionado con otros datos (por ejemplo, prestamos) o no existe.";
        if (mensaje.Contains("UNIQUE constraint failed"))
            return "Ya existe un registro con ese codigo.";

        var inicio = mensaje.IndexOf('\'');
        var fin = mensaje.LastIndexOf('\'');
        return inicio >= 0 && fin > inicio ? mensaje.Substring(inicio + 1, fin - inicio - 1) : mensaje;
    }

    private SqliteConnection AbrirConexion()
    {
        var conexion = new SqliteConnection(_cadenaConexion);
        conexion.Open();
        return conexion;
    }
}
