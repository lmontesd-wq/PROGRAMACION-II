namespace VeterinariaV2.Modelos
{
    public class Propietario
    {
        public int IdPropietario { get; set; }
        public string Nombre { get; set; } = "";
        public string Telefono { get; set; } = "";
        public string Correo { get; set; } = "";
        public string Direccion { get; set; } = "";

        public override string ToString() => $"{Nombre}  |  {Telefono}";
    }
}
