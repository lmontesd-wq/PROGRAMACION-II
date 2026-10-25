using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using VeterinariaV2.Modelos;

namespace VeterinariaV2.Datos
{
    public class VacunacionRepositorio
    {
        private static string S(SqliteDataReader r, int i) => r.IsDBNull(i) ? "" : r.GetString(i);

        public int Crear(Vacunacion v)
        {
            using var cn = Conexion.Abrir();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"INSERT INTO Vacunacion (IdMascota, Vacuna, FechaAplicacion, ProximaDosis)
                                VALUES ($m, $v, $f, $p);
                                SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("$m", v.IdMascota);
            cmd.Parameters.AddWithValue("$v", v.Vacuna);
            cmd.Parameters.AddWithValue("$f", v.FechaAplicacion);
            cmd.Parameters.AddWithValue("$p", v.ProximaDosis);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public List<Vacunacion> Listar()
        {
            var lista = new List<Vacunacion>();
            using var cn = Conexion.Abrir();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"SELECT va.IdVacunacion, va.IdMascota, va.Vacuna, va.FechaAplicacion,
                                       va.ProximaDosis, m.Nombre
                                FROM Vacunacion va
                                INNER JOIN Mascota m ON m.IdMascota = va.IdMascota
                                ORDER BY va.FechaAplicacion DESC";
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                lista.Add(new Vacunacion
                {
                    IdVacunacion = r.GetInt32(0),
                    IdMascota = r.GetInt32(1),
                    Vacuna = S(r, 2),
                    FechaAplicacion = S(r, 3),
                    ProximaDosis = S(r, 4),
                    NombreMascota = S(r, 5)
                });
            }
            return lista;
        }

        public void Actualizar(Vacunacion v)
        {
            using var cn = Conexion.Abrir();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"UPDATE Vacunacion
                                SET IdMascota=$m, Vacuna=$v, FechaAplicacion=$f, ProximaDosis=$p
                                WHERE IdVacunacion=$id";
            cmd.Parameters.AddWithValue("$m", v.IdMascota);
            cmd.Parameters.AddWithValue("$v", v.Vacuna);
            cmd.Parameters.AddWithValue("$f", v.FechaAplicacion);
            cmd.Parameters.AddWithValue("$p", v.ProximaDosis);
            cmd.Parameters.AddWithValue("$id", v.IdVacunacion);
            cmd.ExecuteNonQuery();
        }

        public void Eliminar(int id)
        {
            using var cn = Conexion.Abrir();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "DELETE FROM Vacunacion WHERE IdVacunacion=$id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
