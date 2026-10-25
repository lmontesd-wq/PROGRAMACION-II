namespace VeterinariaV2.Modelos
{
    public class Mascota
    {
        public int IdMascota { get; set; }
        public int IdPropietario { get; set; }
        public string NombrePropietario { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string Especie { get; set; } = "";
        public string Raza { get; set; } = "";
        public string Sexo { get; set; } = "";
        public string FechaNacimiento { get; set; } = "";

        public override string ToString() => $"{Nombre} ({Especie})  |  Dueño: {NombrePropietario}";
    }
}
