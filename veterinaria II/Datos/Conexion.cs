using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace VeterinariaV2.Datos
{
    public static class Conexion
    {
        private static readonly string _ruta =
            Path.Combine(AppContext.BaseDirectory, "veterinaria.db");

        public static SqliteConnection Abrir()
        {
            var cn = new SqliteConnection($"Data Source={_ruta}");
            cn.Open();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "PRAGMA foreign_keys = ON;";
            cmd.ExecuteNonQuery();
            return cn;
        }
    }
}
