using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace LoRaWAN.Packet.Decoder.Lib
{
    internal class AESCMAC
    {
        private readonly AesCryptoServiceProvider aesAlg;
        private readonly ICryptoTransform encryptor;
        private readonly ICryptoTransform decryptor;
        public AESCMAC(string HexKey)
        {
            var key = HexKey.StringToByteArray();

            aesAlg = new AesCryptoServiceProvider();
            aesAlg.Key = key;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.None;
        }
        private byte[] AESEncrypt(byte[] iv, byte[] data)
        {
            using (MemoryStream ms = new MemoryStream())
            {

                using (CryptoStream cs = new CryptoStream(ms, aesAlg.CreateEncryptor(aesAlg.Key, iv), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }
        public byte[] Encrypt(byte[] bytesToBeEncrypted)
        {
            // SubKey generation
            // step 1, AES-128 with key K is applied to an all-zero input block.
            byte[] L = AESEncrypt(new byte[16], new byte[16]);

            // step 2, K1 is derived through the following operation:
            byte[] FirstSubkey = Rol(L); //If the most significant bit of L is equal to 0, K1 is the left-shift of L by 1 bit.
            if ((L[0] & 0x80) == 0x80)
                FirstSubkey[15] ^= 0x87; // Otherwise, K1 is the exclusive-OR of const_Rb and the left-shift of L by 1 bit.

            // step 3, K2 is derived through the following operation:
            byte[] SecondSubkey = Rol(FirstSubkey); // If the most significant bit of K1 is equal to 0, K2 is the left-shift of K1 by 1 bit.
            if ((FirstSubkey[0] & 0x80) == 0x80)
                SecondSubkey[15] ^= 0x87; // Otherwise, K2 is the exclusive-OR of const_Rb and the left-shift of K1 by 1 bit.

            // MAC computing
            if (((bytesToBeEncrypted.Length != 0) && (bytesToBeEncrypted.Length % 16 == 0)) == true)
            {
                // If the size of the input message block is equal to a positive multiple of the block size (namely, 128 bits),
                // the last block shall be exclusive-OR'ed with K1 before processing
                for (int j = 0; j < FirstSubkey.Length; j++)
                    bytesToBeEncrypted[bytesToBeEncrypted.Length - 16 + j] ^= FirstSubkey[j];
            }
            else
            {
                // Otherwise, the last block shall be padded with 10^i
                byte[] padding = new byte[16 - bytesToBeEncrypted.Length % 16];
                padding[0] = 0x80;

                bytesToBeEncrypted = bytesToBeEncrypted.Concat<byte>(padding.AsEnumerable()).ToArray();

                // and exclusive-OR'ed with K2
                for (int j = 0; j < SecondSubkey.Length; j++)
                    bytesToBeEncrypted[bytesToBeEncrypted.Length - 16 + j] ^= SecondSubkey[j];
            }

            // The result of the previous process will be the input of the last encryption.
            byte[] encResult = AESEncrypt(new byte[16], bytesToBeEncrypted);

            byte[] HashValue = new byte[16];
            Array.Copy(encResult, encResult.Length - HashValue.Length, HashValue, 0, HashValue.Length);

            return HashValue;
        }

        private byte[] Rol(byte[] b)
        {
            byte[] r = new byte[b.Length];
            byte carry = 0;

            for (int i = b.Length - 1; i >= 0; i--)
            {
                ushort u = (ushort)(b[i] << 1);
                r[i] = (byte)((u & 0xff) + carry);
                carry = (byte)((u & 0xff00) >> 8);
            }

            return r;
        }
    }
}
