using System.IO;
using System.Security.Cryptography;

namespace LoRaWAN.Packet.Decoder.Lib
{
    internal class AES
    {
        private readonly Aes aesAlg;
        private readonly ICryptoTransform encryptor;
        private readonly ICryptoTransform decryptor;
        public AES(string HexKey)
        {
            var key = HexKey.StringToByteArray();

             aesAlg = Aes.Create();

            aesAlg.KeySize = 128;
            aesAlg.Key = key;
            aesAlg.BlockSize = 128;
            aesAlg.Mode = CipherMode.ECB;
            aesAlg.Padding = PaddingMode.Zeros;
            aesAlg.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            this.encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            this.decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
        }           
        public byte[] Encrypt(byte[] bytesToBeEncrypted)
        {
            using (var msEncrypt = new MemoryStream())
            {
                using (var cs = new CryptoStream(msEncrypt, this.encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                    cs.Close();
                }
               return msEncrypt.ToArray();
            }
        }
    }
}
