namespace VeterinariaV2.Modelos
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = "";
        public string NombreCompleto { get; set; } = "";
        public string Rol { get; set; } = "";
        public bool Activo { get; set; } = true;

        public override string ToString() =>
            $"{NombreCompleto} ({NombreUsuario})  |  {Rol}" + (Activo ? "" : "  [inactivo]");
    }
}
