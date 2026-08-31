using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using CarritoLogica.Logica;

namespace Detalle.Logica
{
    public class Detalle
    {
        public Producto objProducto { get; set; }
        public int Cantidad { get; set;}

        //Construtor

        public Detalle (Producto objProducto, int Cantidad)
        {
            this.objProducto = objProducto;
            this.Cantidad = Cantidad;
        }

        public double CalcularSubtotal()
        {
            if(objProducto == null) return 0.0;
            {
                return objProducto.Precio*Cantidad;
            }
        }

        public void MostrarDetalle()
        {
            double subtotal = CalcularSubtotal();
            Console.WriteLine($"{objProducto.Nombre}{subtotal}");
        }
    }
}