using PrestamoBiblioteca.Dominio;
using PrestamoBiblioteca.Persistencia;

namespace PrestamoBiblioteca.Aplicacion;

internal sealed class MenuBiblioteca
{
    private readonly RepositorioBiblioteca _repositorio;
    private bool _continuar = true;

    public MenuBiblioteca(RepositorioBiblioteca repositorio)
    {
        _repositorio = repositorio;
    }

    public void Ejecutar()
    {
        while (_continuar)
        {
            MostrarMenu();
            try
            {
                var entrada = Console.ReadLine();
                if (entrada is null)
                {
                    _continuar = false;
                    continue;
                }

                switch (entrada.Trim())
                {
                    case "1": CrearLector(); break;
                    case "2": MostrarTabla(_repositorio.ObtenerLectores(), "LECTORES"); break;
                    case "3": ActualizarLector(); break;
                    case "4": EliminarLector(); break;
                    case "5": CrearLibro(); break;
                    case "6": MostrarTabla(_repositorio.ObtenerLibros(), "LIBROS"); break;
                    case "7": ActualizarLibro(); break;
                    case "8": EliminarLibro(); break;
                    case "9": CrearPrestamo(); break;
                    case "10": MostrarTabla(_repositorio.ObtenerPrestamos(), "PRESTAMOS"); break;
                    case "11": MostrarTabla(_repositorio.ObtenerPrestamos(true), "PRESTAMOS ACTIVOS"); break;
                    case "12": RegistrarDevolucion(); break;
                    case "13": EliminarPrestamo(); break;
                    case "0": _continuar = false; break;
                    default: Console.WriteLine("Seleccione una opcion valida del menu."); break;
                }
            }
            catch (Exception error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Operacion controlada con try-catch: {error.Message}");
                Console.ResetColor();
            }

            if (_continuar) Pausar();
        }
    }

    private static void MostrarMenu()
    {
        if (!Console.IsOutputRedirected) Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("====================================================");
        Console.WriteLine("       PRESTAMO DE BIBLIOTECA - SQLITE");
        Console.WriteLine("====================================================");
        Console.ResetColor();
        Console.WriteLine("LECTORES");
        Console.WriteLine(" 1. Crear lector   2. Listar lectores");
        Console.WriteLine(" 3. Actualizar lector   4. Eliminar lector");
        Console.WriteLine("LIBROS");
        Console.WriteLine(" 5. Crear libro   6. Listar libros");
        Console.WriteLine(" 7. Actualizar libro   8. Eliminar libro");
        Console.WriteLine("PRESTAMOS");
        Console.WriteLine(" 9. Registrar prestamo   10. Listar prestamos");
        Console.WriteLine("11. Listar prestamos activos   12. Registrar devolucion");
        Console.WriteLine("13. Eliminar prestamo");
        Console.WriteLine(" 0. Salir");
        Console.Write("Opcion: ");
    }

    private void CrearLector()
    {
        _repositorio.CrearLector(LeerLector());
        Console.WriteLine("Lector creado correctamente.");
    }

    private void ActualizarLector()
    {
        var id = LeerEntero("Id del lector a actualizar");
        var filas = _repositorio.ActualizarLector(id, LeerLector());
        Console.WriteLine(filas > 0 ? "Lector actualizado correctamente." : "No existe un lector con ese Id.");
    }

    private void EliminarLector()
    {
        var filas = _repositorio.EliminarLector(LeerEntero("Id del lector a eliminar"));
        Console.WriteLine(filas > 0
            ? "Lector eliminado. Sus prestamos tambien se eliminaron y el stock se repuso."
            : "No existe un lector con ese Id.");
    }

    private void CrearLibro()
    {
        _repositorio.CrearLibro(LeerLibro());
        Console.WriteLine("Libro creado correctamente.");
    }

    private void ActualizarLibro()
    {
        var id = LeerEntero("Id del libro a actualizar");
        var filas = _repositorio.ActualizarLibro(id, LeerLibro());
        Console.WriteLine(filas > 0 ? "Libro actualizado correctamente." : "No existe un libro con ese Id.");
    }

    private void EliminarLibro()
    {
        var filas = _repositorio.EliminarLibro(LeerEntero("Id del libro a eliminar"));
        Console.WriteLine(filas > 0 ? "Libro eliminado." : "No existe un libro con ese Id.");
    }

    private void CrearPrestamo()
    {
        MostrarTabla(_repositorio.ObtenerLectores(), "LECTORES DISPONIBLES");
        MostrarTabla(_repositorio.ObtenerLibros(), "LIBROS DISPONIBLES");
        var lectorId = LeerEntero("Id del lector");
        var libroId = LeerEntero("Id del libro");
        var dias = LeerEntero("Dias de prestamo");
        var hoy = DateTime.Today;
        _repositorio.CrearPrestamo(new Prestamo(lectorId, libroId, hoy, hoy.AddDays(dias)));
        Console.WriteLine("Prestamo registrado. El stock del libro se desconto automaticamente.");
    }

    private void RegistrarDevolucion()
    {
        MostrarTabla(_repositorio.ObtenerPrestamos(true), "PRESTAMOS ACTIVOS");
        var filas = _repositorio.RegistrarDevolucion(LeerEntero("Id del prestamo a devolver"), DateTime.Today);
        Console.WriteLine(filas > 0
            ? "Devolucion registrada. El stock del libro se repuso automaticamente."
            : "No existe un prestamo con ese Id.");
    }

    private void EliminarPrestamo()
    {
        var filas = _repositorio.EliminarPrestamo(LeerEntero("Id del prestamo a eliminar"));
        Console.WriteLine(filas > 0 ? "Prestamo eliminado." : "No existe un prestamo con ese Id.");
    }

    private static Lector LeerLector() => new(
        LeerTexto("Codigo"), LeerTexto("Correo electronico"), LeerTexto("Telefono"));

    private static Libro LeerLibro() => new(
        LeerTexto("Codigo"), LeerTexto("Titulo"), LeerTexto("Autor"), LeerEntero("Stock"));

    private static int LeerEntero(string mensaje)
    {
        Console.Write($"{mensaje}: ");
        if (!int.TryParse(Console.ReadLine(), out var valor))
            throw new ArgumentException("Ingrese un numero entero valido.");
        return valor;
    }

    private static string LeerTexto(string mensaje)
    {
        Console.Write($"{mensaje}: ");
        var valor = Console.ReadLine();
        return string.IsNullOrWhiteSpace(valor)
            ? throw new ArgumentException($"El campo '{mensaje}' no puede quedar vacio.")
            : valor.Trim();
    }

    private static void MostrarTabla(System.Data.DataTable tabla, string titulo)
    {
        Console.WriteLine($"\n=== {titulo} ===");
        if (tabla.Rows.Count == 0)
        {
            Console.WriteLine("No hay registros.");
            return;
        }

        foreach (System.Data.DataColumn columna in tabla.Columns)
            Console.Write($"{columna.ColumnName,-18}");
        Console.WriteLine();
        foreach (System.Data.DataRow fila in tabla.Rows)
        {
            foreach (var valor in fila.ItemArray)
                Console.Write($"{valor,-18}");
            Console.WriteLine();
        }
    }

    private static void Pausar()
    {
        Console.WriteLine("\nPresione Enter para volver al menu...");
        Console.ReadLine();
    }
}
