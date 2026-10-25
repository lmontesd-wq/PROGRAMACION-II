using System;
using System.Security.Cryptography;

namespace VeterinariaV2.Datos
{
    public static class Seguridad
    {
        private const int Iteraciones = 100000;

        public static string Hashear(string clave)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(clave, salt, Iteraciones, HashAlgorithmName.SHA256, 32);
            return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
        }

        public static bool Verificar(string clave, string almacenado)
        {
            var partes = almacenado.Split(':');
            if (partes.Length != 2) return false;
            byte[] salt = Convert.FromBase64String(partes[0]);
            byte[] esperado = Convert.FromBase64String(partes[1]);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(clave, salt, Iteraciones, HashAlgorithmName.SHA256, 32);
            return CryptographicOperations.FixedTimeEquals(hash, esperado);
        }
    }
}
