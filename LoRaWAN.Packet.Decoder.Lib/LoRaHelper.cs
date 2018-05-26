namespace LoRaWAN.Packet.Decoder.Lib
{
    public static class LoRaHelper
    {


        //public static (bool, string, byte[]) Decrypt(this PHYPayload phyPayload, string appSkey, string NwkSKey)
        //{
        //    var packet = phyPayload.MACPayload.FRMPayload;
        //    var key = int.Parse(phyPayload.MACPayload.FPort.ToHexString()) == 0 ? NwkSKey : appSkey;

        //    var blocks = Math.Ceiling(phyPayload.MACPayload.FRMPayload.Length / 16d);
        //    var pLen = packet.Length;

        //    List<byte> plain_S = new List<byte>();

        //    for (var block = 0; block < blocks; block++)
        //    {
        //        var Ai = new AES(key).Encrypt(A(phyPayload, block));
        //        plain_S.AddRange(Ai);
        //    }

        //    for (var j = 0; j < plain_S.Count; j++)
        //    {
        //        packet[j] ^= plain_S[j];
        //    }
        //    var resultBytes = packet.Take(pLen).ToArray();
        //    return (true, resultBytes.ToHexString(), resultBytes);
        //}

      
    }
}
