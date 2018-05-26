using System.Linq;

namespace LoRaWAN.Packet.Decoder.Lib.Model
{
    public sealed class FHDR
    {

        private byte[] _fhdr;
        private byte[] MacPayload;
        public FHDR(byte[] MacPayload,byte[] fhdr)
        {
            this._fhdr = fhdr;
              this.MacPayload= MacPayload;
        }

        public byte[] DevAddr
        {
            get => _fhdr.GetRange(0, 3).Reverse().ToArray();
        }
        public FCTRL FCTRL
        {
            get => new FCTRL(_fhdr.Get(4)); }
        public byte[] FCnt
        {
            get => _fhdr.GetRange(5, 6).Reverse().ToArray();
        }
        public byte[] FOpts
        {
            get => MacPayload.GetRange(7, (7 + FCTRL.FOptsLen)-1); }


        public override string ToString()
        {
            return _fhdr.ToHexString();
        }
    }
}
