namespace LoRaWAN.Packet.Decoder.Lib.Model
{
    public sealed class PHYPayload
    {
        public PHYPayload(byte[] pHYPayload)
        {
            // byte zero is MHDR
            MHDR = new MHDR(pHYPayload[0]);
            if (MHDR.IsJoinRequestMessage())
                Message = new JoinRequestMessage(pHYPayload);// MACPayload(pHYPayload.GetRange(1, pHYPayload.Length - 5));
                                                             // MIC = pHYPayload.GetRange(pHYPayload.Length - 4);
            else if (MHDR.IsJoinAcceptMessage())
                Message = new JoinAcceptMessage(pHYPayload);

            else if (MHDR.IsDataMessage())
                Message = new DataMessage(pHYPayload, MHDR);
        }


        public PHYPayload(byte[] pHYPayload, string NwkSKey, string AppSKey)
        {
            // byte zero is MHDR
            MHDR = new MHDR(pHYPayload[0]);
            Message = new DataMessageWithKey(pHYPayload, MHDR, NwkSKey,  AppSKey);
        }

        public MHDR MHDR { get; private set; }
        public ILoraMessage Message { get; private set; }


    }
}
