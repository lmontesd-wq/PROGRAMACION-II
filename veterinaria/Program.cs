
using System;

// Clase Mascota
class Mascota
{
    // Atributos
    public string Nombre { get; set; }
    public string Especie { get; set; }
    public string Raza { get; set; }

    // Constructor
    public Mascota(string nombre, string especie, string raza)
    {
        Nombre = nombre;
        Especie = especie;
        Raza = raza;
    }

    // Método para mostrar los datos
    public void MostrarDatos()
    {
        Console.WriteLine("\n--- DATOS DE LA MASCOTA ---");
        Console.WriteLine("Nombre: " + Nombre);
        Console.WriteLine("Especie: " + Especie);
        Console.WriteLine("Raza: " + Raza);
    }
}


// Clase Consulta
class Consulta
{
    public string Motivo { get; set; }
    public string Diagnostico { get; set; }

    // Constructor
    public Consulta(string motivo, string diagnostico)
    {
        Motivo = motivo;
        Diagnostico = diagnostico;
    }

    // Método para mostrar la consulta
    public void MostrarConsulta()
    {
        Console.WriteLine("\n--- CONSULTA ---");
        Console.WriteLine("Motivo: " + Motivo);
        Console.WriteLine("Diagnóstico: " + Diagnostico);
    }
}


// Clase principal
class Program
{
    static Mascota mascota;
    static Consulta consulta;

    static void Main()
    {
        int opcion;

        do
        {
            Console.Clear();

            Console.WriteLine("=================================");
            Console.WriteLine(" SISTEMA CLINICO VETERINARIO");
            Console.WriteLine("=================================");
            Console.WriteLine("1. Registrar mascota");
            Console.WriteLine("2. Mostrar mascota");
            Console.WriteLine("3. Registrar consulta");
            Console.WriteLine("4. Mostrar consulta");
            Console.WriteLine("5. Salir");
            Console.WriteLine("=================================");

            Console.Write("Seleccione una opcion: ");
            opcion = int.Parse(Console.ReadLine());

            switch (opcion)
            {
                case 1:
                    RegistrarMascota();
                    break;

                case 2:
                    if (mascota != null)
                        mascota.MostrarDatos();
                    else
                        Console.WriteLine("No hay una mascota registrada.");
                    break;

                case 3:
                    RegistrarConsulta();
                    break;

                case 4:
                    if (consulta != null)
                        consulta.MostrarConsulta();
                    else
                        Console.WriteLine("No hay una consulta registrada.");
                    break;

                case 5:
                    Console.WriteLine("Gracias por utilizar el sistema.");
                    break;

                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }

            if (opcion != 5)
            {
                Console.WriteLine("\nPresione ENTER para continuar...");
                Console.ReadLine();
            }

        } while (opcion != 5);
    }


    static void RegistrarMascota()
    {
        Console.WriteLine("\n--- REGISTRO DE MASCOTA ---");

        Console.Write("Nombre: ");
        string nombre = Console.ReadLine();

        Console.Write("Especie: ");
        string especie = Console.ReadLine();

        Console.Write("Raza: ");
        string raza = Console.ReadLine();

        // Crear un objeto de la clase Mascota
        mascota = new Mascota(nombre, especie, raza);

        Console.WriteLine("\nMascota registrada correctamente.");
    }


    static void RegistrarConsulta()
    {
        Console.WriteLine("\n--- REGISTRO DE CONSULTA ---");

        Console.Write("Motivo de consulta: ");
        string motivo = Console.ReadLine();

        Console.Write("Diagnóstico: ");
        string diagnostico = Console.ReadLine();

        // Crear un objeto de la clase Consulta
        consulta = new Consulta(motivo, diagnostico);

        Console.WriteLine("\nConsulta registrada correctamente.");
    }
}


