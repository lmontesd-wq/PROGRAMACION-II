namespace VeterinariaV2.Modelos
{
    public class Vacunacion
    {
        public int IdVacunacion { get; set; }
        public int IdMascota { get; set; }
        public string NombreMascota { get; set; } = "";
        public string Vacuna { get; set; } = "";
        public string FechaAplicacion { get; set; } = "";
        public string ProximaDosis { get; set; } = "";

        public override string ToString() =>
            $"{FechaAplicacion}  |  {NombreMascota}  |  {Vacuna}" +
            (string.IsNullOrEmpty(ProximaDosis) ? "" : $"  |  Próxima: {ProximaDosis}");
    }
}
