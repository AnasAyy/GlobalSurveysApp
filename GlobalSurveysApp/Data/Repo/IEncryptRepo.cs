using System.Security.Cryptography;
using System.Text;

namespace GlobalSurveysApp.Data.Repos
{
    public interface IEncryptRepo
    {
        public string EncryptPassword(string clearText);
        public string DecryptPassword(string cipherText);
    }

    public class EncryptRepo : IEncryptRepo
    {
        private readonly IConfiguration _configuration;

        public EncryptRepo(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string EncryptPassword(string clearText)
        {
            var keyhash = _configuration["JWT:Key"];
            if (keyhash == null)
            {
                return string.Empty;
            }
            string EncryptionKey = keyhash;
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new())
                {
                    using (CryptoStream cs = new(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public string DecryptPassword(string cipherText)
        {
            string EncryptionKey = _configuration["JWT:Key"] ?? "DefultKey" ;
            if (EncryptionKey == null)
            {
                // Handle the case when the encryption key is null
                throw new Exception("Encryption key is null.");
            }

            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

    }
}
