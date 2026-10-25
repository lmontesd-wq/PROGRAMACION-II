namespace VeterinariaV2.Modelos
{
    public class Consulta
    {
        public int IdConsulta { get; set; }
        public int IdCita { get; set; }
        public string Diagnostico { get; set; } = "";
        public string Tratamiento { get; set; } = "";
        public string Observaciones { get; set; } = "";
        public string FechaHora { get; set; } = "";
        public string NombreMascota { get; set; } = "";
        public string NombreVeterinario { get; set; } = "";

        public override string ToString() =>
            $"{FechaHora}  |  {NombreMascota}  |  {Diagnostico}  |  Vet: {NombreVeterinario}";
    }
}
