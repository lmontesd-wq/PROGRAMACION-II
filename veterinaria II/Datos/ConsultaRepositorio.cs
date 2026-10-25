using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using VeterinariaV2.Modelos;

namespace VeterinariaV2.Datos
{
    public class ConsultaRepositorio
    {
        private static string S(SqliteDataReader r, int i) => r.IsDBNull(i) ? "" : r.GetString(i);

        public List<Consulta> Listar(int? idVeterinario = null)
        {
            var lista = new List<Consulta>();
            using var cn = Conexion.Abrir();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"SELECT co.IdConsulta, co.IdCita, co.Diagnostico, co.Tratamiento,
                                       co.Observaciones, c.FechaHora, m.Nombre, u.NombreCompleto
                                FROM Consulta co
                                INNER JOIN Cita c ON c.IdCita = co.IdCita
                                INNER JOIN Mascota m ON m.IdMascota = c.IdMascota
                                INNER JOIN Usuario u ON u.IdUsuario = c.IdVeterinario"
                + (idVeterinario.HasValue ? " WHERE c.IdVeterinario = $v" : "")
                + " ORDER BY c.FechaHora DESC";
            if (idVeterinario.HasValue)
                cmd.Parameters.AddWithValue("$v", idVeterinario.Value);

            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                lista.Add(new Consulta
                {
                    IdConsulta = r.GetInt32(0),
                    IdCita = r.GetInt32(1),
                    Diagnostico = S(r, 2),
                    Tratamiento = S(r, 3),
                    Observaciones = S(r, 4),
                    FechaHora = S(r, 5),
                    NombreMascota = S(r, 6),
                    NombreVeterinario = S(r, 7)
                });
            }
            return lista;
        }

        public bool ExistePorCita(int idCita, int idConsultaExcluir)
        {
            using var cn = Conexion.Abrir();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM Consulta WHERE IdCita=$c AND IdConsulta<>$id";
            cmd.Parameters.AddWithValue("$c", idCita);
            cmd.Parameters.AddWithValue("$id", idConsultaExcluir);
            long n = (long)(cmd.ExecuteScalar() ?? 0L);
            return n > 0;
        }

        public int Crear(Consulta c)
        {
            using var cn = Conexion.Abrir();
            using var tx = cn.BeginTransaction();

            using var ins = cn.CreateCommand();
            ins.Transaction = tx;
            ins.CommandText = @"INSERT INTO Consulta (IdCita, Diagnostico, Tratamiento, Observaciones)
                                VALUES ($c, $d, $t, $o);
                                SELECT last_insert_rowid();";
            ins.Parameters.AddWithValue("$c", c.IdCita);
            ins.Parameters.AddWithValue("$d", c.Diagnostico);
            ins.Parameters.AddWithValue("$t", c.Tratamiento);
            ins.Parameters.AddWithValue("$o", c.Observaciones);
            int id = Convert.ToInt32(ins.ExecuteScalar());

            using var upd = cn.CreateCommand();
            upd.Transaction = tx;
            upd.CommandText = "UPDATE Cita SET Estado='Atendida' WHERE IdCita=$c";
            upd.Parameters.AddWithValue("$c", c.IdCita);
            upd.ExecuteNonQuery();

            tx.Commit();
            return id;
        }

        public void Actualizar(Consulta c)
        {
            using var cn = Conexion.Abrir();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"UPDATE Consulta
                                SET Diagnostico=$d, Tratamiento=$t, Observaciones=$o
                                WHERE IdConsulta=$id";
            cmd.Parameters.AddWithValue("$d", c.Diagnostico);
            cmd.Parameters.AddWithValue("$t", c.Tratamiento);
            cmd.Parameters.AddWithValue("$o", c.Observaciones);
            cmd.Parameters.AddWithValue("$id", c.IdConsulta);
            cmd.ExecuteNonQuery();
        }

        public void Eliminar(int idConsulta)
        {
            using var cn = Conexion.Abrir();
            using var tx = cn.BeginTransaction();

            using var sel = cn.CreateCommand();
            sel.Transaction = tx;
            sel.CommandText = "SELECT IdCita FROM Consulta WHERE IdConsulta=$id";
            sel.Parameters.AddWithValue("$id", idConsulta);
            object? idCita = sel.ExecuteScalar();

            using var del = cn.CreateCommand();
            del.Transaction = tx;
            del.CommandText = "DELETE FROM Consulta WHERE IdConsulta=$id";
            del.Parameters.AddWithValue("$id", idConsulta);
            del.ExecuteNonQuery();

            if (idCita != null)
            {
                using var upd = cn.CreateCommand();
                upd.Transaction = tx;
                upd.CommandText = "UPDATE Cita SET Estado='Pendiente' WHERE IdCita=$c";
                upd.Parameters.AddWithValue("$c", Convert.ToInt32(idCita));
                upd.ExecuteNonQuery();
            }

            tx.Commit();
        }
    }
}
