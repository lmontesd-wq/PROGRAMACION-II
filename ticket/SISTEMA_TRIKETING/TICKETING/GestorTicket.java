import java.util.ArrayList;
import java.util.List;
 
public class GestorTicket
{
    public List<Tecnico> lstTecnicos;
    public List<Solicitante> lstSolicitantes;
    public List<Ticket> lstTickets;
 
    public GestorTicket()
    {
        lstTecnicos = new ArrayList<>();       // inicializa la lista de tecnicos
        lstSolicitantes = new ArrayList<>();   // inicializa la lista de solicitantes
        lstTickets = new ArrayList<>();        // inicializa la lista de tickets
    }
 
    // valida los datos y crea un nuevo ticket, agregandolo a la lista de tickets
    public Ticket crearTicket(int intNumero, String strAsunto, String strDescripcion,
                               String strCategoria, String strPrioridad, Solicitante objSolicitante)
    {
        Ticket objTicket = new Ticket(intNumero, strAsunto, strDescripcion, strCategoria, strPrioridad, objSolicitante);
        lstTickets.add(objTicket); // agrega el ticket a la lista de tickets
        return objTicket;
    }
 
    public Ticket buscarTicket(int intNumero)
    {
        for (Ticket objTicket : lstTickets)
        {
            if (objTicket.intNumero == intNumero)
            {
                return objTicket;
            }
        }
        System.out.println("Ticket no encontrado.");
        return null;
    }
}