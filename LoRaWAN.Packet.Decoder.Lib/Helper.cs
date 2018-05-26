using System;
using System.Linq;

namespace LoRaWAN.Packet.Decoder.Lib
{
    public static class Helper
    {

        public  static byte[] GetRange(this byte[] bts ,int start, int finish)
        {
            return bts.Skip(start).Take((finish - start) + 1).ToArray();
        }

        public static byte[] GetRange(this byte[] bts, int start)
        {
            return bts.Skip(start).ToArray();
        }

        public static byte Get(this byte[] bts, int index)
        {
            return bts[index];
        }

        public static string ToHexString(this byte[] bts)
        {
            return BitConverter.ToString(bts).Replace("-","");
        }

        public static byte[] StringToByteArray(this string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        public static string Base64String(this byte[] bts)
        {
            return Convert.ToBase64String(bts);
        }


        public static byte[] ToByteFromHex(this string text)
        {
            return Convert.FromBase64String(text);
        }


        public static int ToInt16(this byte[] value)
        {

            if (BitConverter.IsLittleEndian)
                Array.Reverse(value);

           return  BitConverter.ToInt16(value, 0);
        }
    }
}
