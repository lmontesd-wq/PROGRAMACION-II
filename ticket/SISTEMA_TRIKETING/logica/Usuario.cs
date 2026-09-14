using System;

namespace TicketsSoporte.logica
{
    public class Usuario
    {
        public string strCodigo { get; set; }
        public string strNombre { get; set; }
        public string strCorreo { get; set; }


        public Usuario(string strCodigo, string strNombre, string strCorreo)
        {
            this.strCodigo = strCodigo;
            this.strNombre = strNombre;
            this.strCorreo = strCorreo;
        }
        public abstract string obtenerRol();
        {
            return "Usuario";

        }

        

    }
    }
