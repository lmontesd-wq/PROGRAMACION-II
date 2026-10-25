using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using VeterinariaV2.Modelos;

namespace VeterinariaV2.Datos
{
    public class UsuarioRepositorio
    {
        private static Usuario Leer(SqliteDataReader r) => new Usuario
        {
            IdUsuario = r.GetInt32(0),
            NombreUsuario = r.GetString(1),
            NombreCompleto = r.GetString(2),
            Rol = r.GetString(3),
            Activo = r.GetInt32(4) == 1
        };

        public int Crear(Usuario u, string clave)
        {
            using var cn = Conexion.Abrir();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"INSERT INTO Usuario (NombreUsuario, Clave, NombreCompleto, Rol, Activo)
                                VALUES ($u, $c, $n, $r, $a);
                                SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("$u", u.NombreUsuario);
            cmd.Parameters.AddWithValue("$c", Seguridad.Hashear(clave));
            cmd.Parameters.AddWithValue("$n", u.NombreCompleto);
            cmd.Parameters.AddWithValue("$r", u.Rol);
            cmd.Parameters.AddWithValue("$a", u.Activo ? 1 : 0);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public List<Usuario> Listar()
        {
            var lista = new List<Usuario>();
            using var cn = Conexion.Abrir();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"SELECT IdUsuario, NombreUsuario, NombreCompleto, Rol, Activo
                                FROM Usuario ORDER BY NombreCompleto";
            using var r = cmd.ExecuteReader();
            while (r.Read()) lista.Add(Leer(r));
            return lista;
        }

        public void Actualizar(Usuario u, string? nuevaClave)
        {
            using var cn = Conexion.Abrir();
            using var cmd = cn.CreateCommand();
            if (string.IsNullOrEmpty(nuevaClave))
            {
                cmd.CommandText = @"UPDATE Usuario
                                    SET NombreUsuario=$u, NombreCompleto=$n, Rol=$r, Activo=$a
                                    WHERE IdUsuario=$id";
            }
            else
            {
                cmd.CommandText = @"UPDATE Usuario
                                    SET NombreUsuario=$u, NombreCompleto=$n, Rol=$r, Activo=$a, Clave=$c
                                    WHERE IdUsuario=$id";
                cmd.Parameters.AddWithValue("$c", Seguridad.Hashear(nuevaClave));
            }
            cmd.Parameters.AddWithValue("$u", u.NombreUsuario);
            cmd.Parameters.AddWithValue("$n", u.NombreCompleto);
            cmd.Parameters.AddWithValue("$r", u.Rol);
            cmd.Parameters.AddWithValue("$a", u.Activo ? 1 : 0);
            cmd.Parameters.AddWithValue("$id", u.IdUsuario);
            cmd.ExecuteNonQuery();
        }

        public void Eliminar(int id)
        {
            using var cn = Conexion.Abrir();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "DELETE FROM Usuario WHERE IdUsuario=$id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }

        public Usuario? Autenticar(string nombreUsuario, string clave)
        {
            using var cn = Conexion.Abrir();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"SELECT IdUsuario, NombreUsuario, NombreCompleto, Rol, Activo, Clave
                                FROM Usuario WHERE NombreUsuario=$u AND Activo=1";
            cmd.Parameters.AddWithValue("$u", nombreUsuario);
            using var r = cmd.ExecuteReader();
            if (r.Read() && Seguridad.Verificar(clave, r.GetString(5)))
                return Leer(r);
            return null;
        }

        public void CrearAdminPorDefectoSiNoHay()
        {
            using var cn = Conexion.Abrir();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM Usuario";
            long total = (long)(cmd.ExecuteScalar() ?? 0L);
            if (total > 0) return;

            Crear(new Usuario
            {
                NombreUsuario = "admin",
                NombreCompleto = "Administrador",
                Rol = "Administrador",
                Activo = true
            }, "admin123");
        }
    }
}
