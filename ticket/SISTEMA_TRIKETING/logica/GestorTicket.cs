using System;

namespace TicketsSoporte.logica
{
    public class GestiorTicket
    {
        public List<Tecnico> lstTecnicos { get; set; }
        public List<Solicitante> lstSolicitantes { get; set; }
        public List<Ticket> lstTickets { get; set; }

        public GestorTickets()
        {
            lstTecnicos = new List<Tecnico>(); //inicializa la lista de técnicos
            lstSolicitantes = new List<Solicitante>(); //inicializa la lista de solicitantes
            lstTickets = new List<Ticket>(); //inicializa la lista de tickets
        }
        public Ticket crearTicket(int intNumero, string strAsunto, string strDescripcion, string strCategoria, string strPrioridad, Solicitante objSolicitante)
        // valida los datos y crea un nuevo ticket, agregándolo a la lista de tickets
        {
            Ticket objTicket = new Ticket(intNumero, strAsunto, strDescripcion, strCategoria, strPrioridad, objSolicitante); //
            objTicket.asignarTecnico(objTecnico); 
            lstTickets.Add(objTicket);//agrega el ticket a la lista de tickets
            return objTicket;
        }

        public Ticket buscarTicket(int intNumero)
        {
            Ticket objTicket = lstTickets.Find(t => t.intNumero == intNumero);// busca un ticket en la lista de tickets por su número
            if (objTicket == null)
            {
                console.WriteLine("Ticket no encontrado.");
            }
            return objTicket;
        }
    }
}
