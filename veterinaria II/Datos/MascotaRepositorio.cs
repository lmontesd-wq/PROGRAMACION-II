using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using VeterinariaV2.Modelos;

namespace VeterinariaV2.Datos
{
    public class MascotaRepositorio
    {
        private static string S(SqliteDataReader r, int i) => r.IsDBNull(i) ? "" : r.GetString(i);

        public int Crear(Mascota m)
        {
            using var cn = Conexion.Abrir();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"INSERT INTO Mascota (IdPropietario, Nombre, Especie, Raza, Sexo, FechaNacimiento)
                                VALUES ($p, $n, $e, $r, $s, $f);
                                SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("$p", m.IdPropietario);
            cmd.Parameters.AddWithValue("$n", m.Nombre);
            cmd.Parameters.AddWithValue("$e", m.Especie);
            cmd.Parameters.AddWithValue("$r", m.Raza);
            cmd.Parameters.AddWithValue("$s", m.Sexo);
            cmd.Parameters.AddWithValue("$f", m.FechaNacimiento);
            return System.Convert.ToInt32(cmd.ExecuteScalar());
        }

        public List<Mascota> Listar()
        {
            var lista = new List<Mascota>();
            using var cn = Conexion.Abrir();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"SELECT m.IdMascota, m.IdPropietario, m.Nombre, m.Especie,
                                       m.Raza, m.Sexo, m.FechaNacimiento, p.Nombre
                                FROM Mascota m
                                INNER JOIN Propietario p ON p.IdPropietario = m.IdPropietario
                                ORDER BY m.Nombre";
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                lista.Add(new Mascota
                {
                    IdMascota = r.GetInt32(0),
                    IdPropietario = r.GetInt32(1),
                    Nombre = S(r, 2),
                    Especie = S(r, 3),
                    Raza = S(r, 4),
                    Sexo = S(r, 5),
                    FechaNacimiento = S(r, 6),
                    NombrePropietario = S(r, 7)
                });
            }
            return lista;
        }

        public void Actualizar(Mascota m)
        {
            using var cn = Conexion.Abrir();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"UPDATE Mascota
                                SET IdPropietario=$p, Nombre=$n, Especie=$e,
                                    Raza=$r, Sexo=$s, FechaNacimiento=$f
                                WHERE IdMascota=$id";
            cmd.Parameters.AddWithValue("$p", m.IdPropietario);
            cmd.Parameters.AddWithValue("$n", m.Nombre);
            cmd.Parameters.AddWithValue("$e", m.Especie);
            cmd.Parameters.AddWithValue("$r", m.Raza);
            cmd.Parameters.AddWithValue("$s", m.Sexo);
            cmd.Parameters.AddWithValue("$f", m.FechaNacimiento);
            cmd.Parameters.AddWithValue("$id", m.IdMascota);
            cmd.ExecuteNonQuery();
        }

        public void Eliminar(int id)
        {
            using var cn = Conexion.Abrir();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "DELETE FROM Mascota WHERE IdMascota=$id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
