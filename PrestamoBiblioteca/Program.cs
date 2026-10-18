using PrestamoBiblioteca.Aplicacion;
using PrestamoBiblioteca.Persistencia;

namespace PrestamoBiblioteca;

internal static class Program
{
    private static void Main()
    {
        try
        {
            var inicializador = new InicializadorBaseDatos();
            inicializador.Inicializar();
            var repositorio = new RepositorioBiblioteca(inicializador.CadenaConexion);
            new MenuBiblioteca(repositorio).Ejecutar();
        }
        catch (Exception error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"No fue posible iniciar el sistema: {error.Message}");
            Console.ResetColor();
        }
    }
}
