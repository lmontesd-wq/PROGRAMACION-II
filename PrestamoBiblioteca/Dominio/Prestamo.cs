namespace PrestamoBiblioteca.Dominio;

public sealed class Prestamo
{
    public Prestamo(int lectorId, int libroId, DateTime fechaPrestamo, DateTime fechaLimite)
    {
        if (lectorId <= 0) throw new ArgumentException("Debe seleccionar un lector.");
        if (libroId <= 0) throw new ArgumentException("Debe seleccionar un libro.");
        if (fechaLimite.Date < fechaPrestamo.Date)
            throw new ArgumentException("La fecha limite no puede ser anterior a la fecha de prestamo.");
        LectorId = lectorId;
        LibroId = libroId;
        FechaPrestamo = fechaPrestamo;
        FechaLimite = fechaLimite;
    }

    public int Id { get; set; }
    public int LectorId { get; }
    public int LibroId { get; }
    public DateTime FechaPrestamo { get; }
    public DateTime FechaLimite { get; }
    public DateTime? FechaDevolucion { get; set; }
}
