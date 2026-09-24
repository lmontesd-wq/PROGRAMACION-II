
/**
 * Write a description of class Consulta here.
 * 
 * @author (your name) 
 * @version (a version number or a date)
 */
public class Consulta {
    // Atributos
    private String motivo;
    private String diagnostico;
    private Mascota mascota; // Relación con la clase Mascota

    // Constructor
    public Consulta(String motivo, String diagnostico, Mascota mascota) {
        this.motivo = motivo;
        this.diagnostico = diagnostico;
        this.mascota = mascota;
    }

    // Método para mostrar la consulta
    public void mostrarConsulta() {
        System.out.println("\n--- CONSULTA ---");
        System.out.println("Mascota: " + mascota.getNombre());
        System.out.println("Motivo: " + motivo);
        System.out.println("Diagnóstico: " + diagnostico);
    }

    // Método para obtener la mascota
    public Mascota getMascota() {
        return mascota;
    }
}
