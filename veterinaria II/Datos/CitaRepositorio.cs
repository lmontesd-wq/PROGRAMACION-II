using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using VeterinariaV2.Modelos;

namespace VeterinariaV2.Datos
{
    public class CitaRepositorio
    {
        private static string S(SqliteDataReader r, int i) => r.IsDBNull(i) ? "" : r.GetString(i);

        private const string SelectBase = @"SELECT c.IdCita, c.IdMascota, c.IdVeterinario, c.FechaHora,
                                                   c.Motivo, c.Estado, m.Nombre, u.NombreCompleto
                                            FROM Cita c
                                            INNER JOIN Mascota m ON m.IdMascota = c.IdMascota
                                            INNER JOIN Usuario u ON u.IdUsuario = c.IdVeterinario";

        public int Crear(Cita c)
        {
            using var cn = Conexion.Abrir();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"INSERT INTO Cita (IdMascota, IdVeterinario, FechaHora, Motivo, Estado)
                                VALUES ($m, $v, $f, $mo, $e);
                                SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("$m", c.IdMascota);
            cmd.Parameters.AddWithValue("$v", c.IdVeterinario);
            cmd.Parameters.AddWithValue("$f", c.FechaHora);
            cmd.Parameters.AddWithValue("$mo", c.Motivo);
            cmd.Parameters.AddWithValue("$e", c.Estado);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public List<Cita> Listar(int? idVeterinario = null)
        {
            var lista = new List<Cita>();
            using var cn = Conexion.Abrir();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = SelectBase
                + (idVeterinario.HasValue ? " WHERE c.IdVeterinario = $v" : "")
                + " ORDER BY c.FechaHora DESC";
            if (idVeterinario.HasValue)
                cmd.Parameters.AddWithValue("$v", idVeterinario.Value);

            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                lista.Add(new Cita
                {
                    IdCita = r.GetInt32(0),
                    IdMascota = r.GetInt32(1),
                    IdVeterinario = r.GetInt32(2),
                    FechaHora = S(r, 3),
                    Motivo = S(r, 4),
                    Estado = S(r, 5),
                    NombreMascota = S(r, 6),
                    NombreVeterinario = S(r, 7)
                });
            }
            return lista;
        }

        public void Actualizar(Cita c)
        {
            using var cn = Conexion.Abrir();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"UPDATE Cita
                                SET IdMascota=$m, IdVeterinario=$v, FechaHora=$f, Motivo=$mo, Estado=$e
                                WHERE IdCita=$id";
            cmd.Parameters.AddWithValue("$m", c.IdMascota);
            cmd.Parameters.AddWithValue("$v", c.IdVeterinario);
            cmd.Parameters.AddWithValue("$f", c.FechaHora);
            cmd.Parameters.AddWithValue("$mo", c.Motivo);
            cmd.Parameters.AddWithValue("$e", c.Estado);
            cmd.Parameters.AddWithValue("$id", c.IdCita);
            cmd.ExecuteNonQuery();
        }

        public void Eliminar(int id)
        {
            using var cn = Conexion.Abrir();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "DELETE FROM Cita WHERE IdCita=$id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }

        public bool ExisteConflicto(int idVeterinario, string fechaHora, int idCitaExcluir)
        {
            using var cn = Conexion.Abrir();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"SELECT COUNT(*) FROM Cita
                                WHERE IdVeterinario=$v AND FechaHora=$f
                                  AND IdCita<>$id AND Estado<>'Cancelada'";
            cmd.Parameters.AddWithValue("$v", idVeterinario);
            cmd.Parameters.AddWithValue("$f", fechaHora);
            cmd.Parameters.AddWithValue("$id", idCitaExcluir);
            long n = (long)(cmd.ExecuteScalar() ?? 0L);
            return n > 0;
        }
    }
}
