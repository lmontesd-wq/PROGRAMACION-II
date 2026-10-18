namespace PrestamoBiblioteca.Dominio;

public sealed class Libro
{
    public Libro(string codigo, string titulo, string autor, int stock)
    {
        Codigo = Validar(codigo, "El codigo del libro es obligatorio.");
        Titulo = Validar(titulo, "El titulo es obligatorio.");
        Autor = Validar(autor, "El autor es obligatorio.");
        if (stock < 0) throw new ArgumentException("El stock no puede ser negativo.");
        Stock = stock;
    }

    public string Codigo { get; }
    public string Titulo { get; }
    public string Autor { get; }
    public int Stock { get; }

    private static string Validar(string valor, string mensaje) =>
        string.IsNullOrWhiteSpace(valor) ? throw new ArgumentException(mensaje) : valor.Trim();
}
