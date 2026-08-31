using System;
using System.Collections.Generic;

namespace CarritoLogica.Logica;

public class Detalle
{
    public int Cantidad { get; set; }
    public Producto? Producto { get; set; }
}

public class Carrito
{
    public List<Detalle> LstDetalle { get; set; }

    // Constructor
    public Carrito()
    {
        LstDetalle = new List<Detalle>();
    }

    public void AgregarCarrito(int Cantidad, Producto objProducto)
    {
        // Validaciones
        if (!objProducto.verificarStock(Cantidad))
        {
            Console.WriteLine("No hay stock suficiente");
            return;
        }
        
        LstDetalle.Add(new Detalle { Cantidad = Cantidad, Producto = objProducto });

        if (LstDetalle.Count == 0)
        {
            //Error
            return;
        }

        foreach (var detalle in LstDetalle)
        {
            double subtotal = detalle.Cantidad * detalle.Producto.Precio;
            Console.WriteLine($"Producto: {detalle.Producto.Nombre}, Cantidad: {detalle.Cantidad}, Subtotal: {subtotal}");
        }

    }

}