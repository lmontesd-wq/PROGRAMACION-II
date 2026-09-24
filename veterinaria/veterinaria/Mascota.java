
/**
 * Write a description of class Mascota here.
 * 
 * @author (your name) 
 * @version (a version number or a date)
 */
public class Mascota {
    // Atributos
    private String nombre;
    private String especie;
    private String raza;

    // Constructor
    public Mascota(String nombre, String especie, String raza) {
        this.nombre = nombre;
        this.especie = especie;
        this.raza = raza;
    }

    // Método para mostrar los datos
    public void mostrarDatos() {
        System.out.println("\n--- DATOS DE LA MASCOTA ---");
        System.out.println("Nombre: " + nombre);
        System.out.println("Especie: " + especie);
        System.out.println("Raza: " + raza);
    }
}
