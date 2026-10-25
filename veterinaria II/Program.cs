using System;
using Avalonia;
using VeterinariaV2.Datos;

namespace VeterinariaV2;

class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        InicializadorBD.Crear();
        new UsuarioRepositorio().CrearAdminPorDefectoSiNoHay();
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
