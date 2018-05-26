using System.Collections;

namespace LoRaWAN.Packet.Decoder.Lib.Model
{
    public class FCTRL
    {
        private byte[] fctrl;
        private BitArray bitArray;
        public FCTRL(byte fctrl)
        {

            this.fctrl = new byte[] { fctrl };
            bitArray = new BitArray(this.fctrl);
        }
        public bool ADR
        {
            get => bitArray.Get(7);
        }
        public bool RFU
        {
            get => bitArray.Get(6);
        }
        public bool ADRACKReq
        {
            get => bitArray.Get(6);
        }
        public bool ACK { get => bitArray.Get(5); }
        public bool FPending { get => bitArray.Get(4); }
        public bool ClassB { get => bitArray.Get(4); }
        public int FOptsLen { get => fctrl[0] & 0x0f; }

        public override string ToString()
        {
            return fctrl.ToHexString();
        }
    }
}