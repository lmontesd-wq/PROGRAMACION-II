
/**
 * Write a description of class Program here.
 * 
 * @author (your name) 
 * @version (a version number or a date)
 */
import java.util.Scanner;

public class Program {
    static Mascota mascota;
    static Consulta consulta;
    static Scanner sc = new Scanner(System.in);

    public static void main(String[] args) {
        int opcion;

        do {
            System.out.println("=================================");
            System.out.println(" SISTEMA CLÍNICO VETERINARIO");
            System.out.println("=================================");
            System.out.println("1. Registrar mascota");
            System.out.println("2. Mostrar mascota");
            System.out.println("3. Registrar consulta");
            System.out.println("4. Mostrar consulta");
            System.out.println("5. Salir");
            System.out.println("=================================");
            System.out.print("Seleccione una opción: ");

            if (!sc.hasNextInt()) {
                System.out.println("Debe ingresar un número válido.");
                sc.nextLine();
                continue;
            }

            opcion = sc.nextInt();
            sc.nextLine();

            switch (opcion) {
                case 1:
                    registrarMascota();
                    break;
                case 2:
                    if (mascota != null)
                        mascota.mostrarDatos();
                    else
                        System.out.println("No hay una mascota registrada.");
                    break;
                case 3:
                    registrarConsulta();
                    break;
                case 4:
                    if (consulta != null)
                        consulta.mostrarConsulta();
                    else
                        System.out.println("No hay una consulta registrada.");
                    break;
                case 5:
                    System.out.println("Gracias por utilizar el sistema.");
                    break;
                default:
                    System.out.println("Opción no válida.");
                    break;
            }

            if (opcion != 5) {
                System.out.println("\nPresione ENTER para continuar...");
                sc.nextLine();
            }

        } while (opcion != 5);
    }

    static void registrarMascota() {
        System.out.println("\n--- REGISTRO DE MASCOTA ---");
        System.out.print("Nombre: ");
        String nombre = sc.nextLine();
        System.out.print("Especie: ");
        String especie = sc.nextLine();
        System.out.print("Raza: ");
        String raza = sc.nextLine();

        mascota = new Mascota(nombre, especie, raza);
        System.out.println("\nMascota registrada correctamente.");
    }

    static void registrarConsulta() {
        if (mascota == null) {
            System.out.println("Debe registrar una mascota antes de crear una consulta.");
            return;
        }

        System.out.println("\n--- REGISTRO DE CONSULTA ---");
        System.out.print("Motivo de consulta: ");
        String motivo = sc.nextLine();
        System.out.print("Diagnóstico: ");
        String diagnostico = sc.nextLine();

        consulta = new Consulta(motivo, diagnostico, mascota);
        System.out.println("\nConsulta registrada correctamente.");
    }
}
