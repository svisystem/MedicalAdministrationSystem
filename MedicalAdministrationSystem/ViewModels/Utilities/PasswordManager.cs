using System;
using System.Security.Cryptography;
using System.Text;

namespace MedicalAdministrationSystem.ViewModels.Utilities
{
    class PasswordManager
    {
        private static RNGCryptoServiceProvider m_cryptoServiceProvider = null;
        protected internal string GetSaltString()
        {
            m_cryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] saltBytes = new byte[32];
            m_cryptoServiceProvider.GetNonZeroBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }
        protected internal string GenerateHashWithSalt(string password, string salt)
        {
            string sHashWithSalt = password + salt;
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(sHashWithSalt);
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] hash = algorithm.ComputeHash(saltedHashBytes);
            return Convert.ToBase64String(hash);
        }
        protected internal bool IsPasswordMatch(string password, string salt, string hash)
        {
            return hash.Equals(GenerateHashWithSalt(password, salt));
        }
    }
}
