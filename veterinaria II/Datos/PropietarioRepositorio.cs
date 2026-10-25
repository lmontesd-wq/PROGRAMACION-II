using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using VeterinariaV2.Modelos;

namespace VeterinariaV2.Datos
{
    public class PropietarioRepositorio
    {
        public int Crear(Propietario p)
        {
            using var cn = Conexion.Abrir();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"INSERT INTO Propietario (Nombre, Telefono, Correo, Direccion)
                                VALUES ($n, $t, $c, $d);
                                SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("$n", p.Nombre);
            cmd.Parameters.AddWithValue("$t", p.Telefono);
            cmd.Parameters.AddWithValue("$c", p.Correo);
            cmd.Parameters.AddWithValue("$d", p.Direccion);
            return System.Convert.ToInt32(cmd.ExecuteScalar());
        }

        public List<Propietario> Listar()
        {
            var lista = new List<Propietario>();
            using var cn = Conexion.Abrir();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT IdPropietario, Nombre, Telefono, Correo, Direccion FROM Propietario ORDER BY Nombre";
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                lista.Add(new Propietario
                {
                    IdPropietario = r.GetInt32(0),
                    Nombre = r.GetString(1),
                    Telefono = r.IsDBNull(2) ? "" : r.GetString(2),
                    Correo = r.IsDBNull(3) ? "" : r.GetString(3),
                    Direccion = r.IsDBNull(4) ? "" : r.GetString(4)
                });
            }
            return lista;
        }

        public void Actualizar(Propietario p)
        {
            using var cn = Conexion.Abrir();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"UPDATE Propietario
                                SET Nombre=$n, Telefono=$t, Correo=$c, Direccion=$d
                                WHERE IdPropietario=$id";
            cmd.Parameters.AddWithValue("$n", p.Nombre);
            cmd.Parameters.AddWithValue("$t", p.Telefono);
            cmd.Parameters.AddWithValue("$c", p.Correo);
            cmd.Parameters.AddWithValue("$d", p.Direccion);
            cmd.Parameters.AddWithValue("$id", p.IdPropietario);
            cmd.ExecuteNonQuery();
        }

        public void Eliminar(int id)
        {
            using var cn = Conexion.Abrir();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "DELETE FROM Propietario WHERE IdPropietario=$id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
