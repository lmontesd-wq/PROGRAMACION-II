namespace PrestamoBiblioteca.Dominio;

public sealed class Lector
{
    public Lector(string codigo, string correo, string telefono)
    {
        Codigo = Validar(codigo, "El codigo del lector es obligatorio.");
        Correo = Validar(correo, "El correo es obligatorio.");
        Telefono = Validar(telefono, "El telefono es obligatorio.");
    }

    public string Codigo { get; }
    public string Correo { get; }
    public string Telefono { get; }

    private static string Validar(string valor, string mensaje) =>
        string.IsNullOrWhiteSpace(valor) ? throw new ArgumentException(mensaje) : valor.Trim();
}
