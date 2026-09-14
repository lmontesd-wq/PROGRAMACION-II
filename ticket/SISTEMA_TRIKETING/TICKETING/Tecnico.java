public class Tecnico extends Usuario
{
    public String strEspecialidad;
    public int intCargaActual;
    public int intCapacidadMaxima;
 
    public Tecnico(String strCodigo, String strNombre, String strCorreo,
                    String strEspecialidad, int intCapacidadMaxima)
    {
        super(strCodigo, strNombre, strCorreo);
        this.strEspecialidad = strEspecialidad;
        this.intCargaActual = 0;
        this.intCapacidadMaxima = intCapacidadMaxima;
    }
 
    @Override
    public String obtenerRol()
    {
        return "Tecnico";
    }
 
    public boolean estaDisponible()
    {
        return blnActivo && intCargaActual < intCapacidadMaxima;
    }
 
    public boolean puedeAtender(String strCategoria)
    {
        return estaDisponible() &&
               (strEspecialidad.equalsIgnoreCase(strCategoria) ||
                strEspecialidad.equalsIgnoreCase("General"));
    }
 
    public void aumentarCarga()
    {
        if (intCargaActual < intCapacidadMaxima)
        {
            intCargaActual++;
        }
    }
 
    public void liberarCarga()
    {
        if (intCargaActual > 0)
        {
            intCargaActual--;
        }
    }
 
    @Override
    public void mostrarInformacion()
    {
        super.mostrarInformacion();
        System.out.println(" Especialidad: " + strEspecialidad + " | Carga: " +
                            intCargaActual + "/" + intCapacidadMaxima);
    }
}