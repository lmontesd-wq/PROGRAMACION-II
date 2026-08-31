using System;
using System.Collections.Generic;
using System.Linq;

namespace CarritoLogica.Logica
{
    public class Producto
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public int Stock { get; set;}
        
        //Constructor

        public Producto (String Codigo, string Nombre, double Precio, int Stock)
        {
            this.Codigo = Codigo;
            this.Nombre = Nombre;
            this.Precio = Precio;
            this.Stock = Stock;
        }

        public bool verificarStock (int Cantidad)
        {
            return Cantidad > 0 && this.Stock >= Cantidad;
        }

        public void mostrarProducto()
        {
            Console.WriteLine($"{Nombre}");
        }
    }
}