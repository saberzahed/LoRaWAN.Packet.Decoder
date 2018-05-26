namespace LoRaWAN.Packet.Decoder.Lib.Model
{
    public class MACPayload
    {
        public MACPayload(byte[] macPayload)
        {

            //var FCtrl = BitConverter.ToInt16(macPayload.GetRange(4, 5), 0);
            //var FOptsLen = FCtrl & 0x0f;
            //var FHDR_length = 7 + FOptsLen;

            //FHDR = new FHDR(macPayload.GetRange(0, 0 + FHDR_length));

            //if (FHDR_length == macPayload.Length)
            //{
            //    FPort = new byte[0];
            //    FRMPayload = new byte[0];
            //}
            //else
            //{
            //    FPort = macPayload.GetRange(FHDR_length, FHDR_length);
            //    FRMPayload = macPayload.GetRange(FHDR_length + 1);
            //}
        }
        public FHDR FHDR { get; private set; }
        public byte[] FPort { get; private set; }
        public byte[] FRMPayload { get; private set; }

        public string FPortHex { get => FPort.ToHexString(); }
        public string FRMPayloadHex { get => FRMPayload.ToHexString(); }




    }
}
