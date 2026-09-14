import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.List;
 
public class Ticket
{
    public int intNumero;
    public String strTitulo;
    public String strDescripcion;
    public String strCategoria;
    public String strPrioridad;
    public String strEstado;
    public boolean blnEscalado;
    public Solicitante objSolicitante;
    public Tecnico objTecnicoAsignado;
    public List<String> lstBitacora;
    public List<String> lstErrores;
 
    private static final DateTimeFormatter FORMATO_FECHA = DateTimeFormatter.ofPattern("dd/MM/yyyy HH:mm");
 
    public Ticket(int intNumero, String strTitulo, String strDescripcion, String strCategoria,
                  String strPrioridad, Solicitante objSolicitante)
    {
        this.intNumero = intNumero;
        this.strTitulo = strTitulo;
        this.strDescripcion = strDescripcion;
        this.strCategoria = strCategoria;
        this.strPrioridad = strPrioridad;
        this.objSolicitante = objSolicitante;
        this.strEstado = "Abierto";
        this.blnEscalado = false;
        this.lstBitacora = new ArrayList<>();
        this.lstErrores = new ArrayList<>();
        registrarBitacora("Ticket creado");
    }
 
    public void registrarBitacora(String strEvento)
    {
        String strFecha = LocalDateTime.now().format(FORMATO_FECHA);
        lstBitacora.add(strFecha + " - " + strEvento);
    }
 
    public void asignarTecnico(Tecnico objTecnico)
    {
        if (objTecnico == null)
        {
            throw new IllegalStateException("No se puede asignar un tecnico nulo.");
        }
 
        if (!objTecnico.puedeAtender(strCategoria))
        {
            throw new IllegalStateException("El tecnico no esta disponible o no atiende esta categoria.");
        }
 
        objTecnicoAsignado = objTecnico;
        objTecnico.aumentarCarga();
        strEstado = "Asignado";
        registrarBitacora("Asignado a " + objTecnico.strNombre);
    }
 
    public void registrarError(String strTipo, String strDescripcion, String strImpacto)
    {
        String strError = strTipo + ": " + strDescripcion + " | Impacto: " + strImpacto;
        lstErrores.add(strError);
        registrarBitacora("Error registrado - " + strError);
 
        if (strImpacto.equalsIgnoreCase("Alto") || strImpacto.equalsIgnoreCase("Critico"))
        {
            escalar("Error de alto impacto");
        }
    }
 
    public void resolver(String strSolucion)
    {
        if (objTecnicoAsignado == null || !strEstado.equals("Asignado"))
        {
            throw new IllegalStateException("Solo se puede resolver un ticket asignado.");
        }
 
        strEstado = "Resuelto";
        registrarBitacora("Solucion registrada: " + strSolucion);
    }
 
    public void cerrar()
    {
        if (!strEstado.equals("Resuelto"))
        {
            throw new IllegalStateException("Solo se puede cerrar un ticket resuelto.");
        }
 
        strEstado = "Cerrado";
        if (objTecnicoAsignado != null)
        {
            objTecnicoAsignado.liberarCarga();
        }
        registrarBitacora("Ticket cerrado");
    }
 
    public void escalar(String strMotivo)
    {
        blnEscalado = true;
        if (!strPrioridad.equalsIgnoreCase("Critica"))
        {
            strPrioridad = "Critica";
        }
        registrarBitacora("Ticket escalado: " + strMotivo);
    }
 
    public void mostrarResumen()
    {
        System.out.println("#" + intNumero + " | " + strTitulo + " | Estado: " + strEstado +
                            " | Prioridad: " + strPrioridad + " | Categoria: " + strCategoria);
        System.out.println(" Solicitante: " + objSolicitante.strNombre);
        System.out.println(" Tecnico: " + (objTecnicoAsignado != null ? objTecnicoAsignado.strNombre : "Sin asignar") +
                            " | Escalado: " + blnEscalado);
    }
 
    public void mostrarBitacora()
    {
        System.out.println("\nBitacora del ticket #" + intNumero);
        for (String strEvento : lstBitacora)
        {
            System.out.println("- " + strEvento);
        }
    }
}