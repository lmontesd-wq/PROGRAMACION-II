public abstract class Usuario
{
    public String strCodigo;
    public String strNombre;
    public String strCorreo;
    public boolean blnActivo;
 
    public Usuario(String strCodigo, String strNombre, String strCorreo)
    {
        this.strCodigo = strCodigo;
        this.strNombre = strNombre;
        this.strCorreo = strCorreo;
        this.blnActivo = true;
    }
 
    public abstract String obtenerRol();
 
    public void mostrarInformacion()
    {
        System.out.println("Codigo: " + strCodigo + " | Nombre: " + strNombre +
                            " | Correo: " + strCorreo + " | Rol: " + obtenerRol());
    }
}
 