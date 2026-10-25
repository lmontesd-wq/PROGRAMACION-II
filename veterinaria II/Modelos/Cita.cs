namespace VeterinariaV2.Modelos
{
    public class Cita
    {
        public int IdCita { get; set; }
        public int IdMascota { get; set; }
        public int IdVeterinario { get; set; }
        public string NombreMascota { get; set; } = "";
        public string NombreVeterinario { get; set; } = "";
        public string FechaHora { get; set; } = "";
        public string Motivo { get; set; } = "";
        public string Estado { get; set; } = "Pendiente";

        public override string ToString() =>
            $"{FechaHora}  |  {NombreMascota}  |  Vet: {NombreVeterinario}  |  {Estado}";
    }
}
