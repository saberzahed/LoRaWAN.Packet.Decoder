using System.Collections.Generic;
using System.Linq;
using LoRaWAN.Packet.Decoder.Lib.Model;

namespace LoRaWAN.Packet.Decoder.Lib
{
    public class MICChecker
    {

        protected MHDR Mhdr;
        protected DataMessageWithKey packet;
        protected string NwkSKey;
        protected string AppKey;

        public  byte[] MIC { get; private set; }
        public MICChecker(DataMessageWithKey packet,MHDR Mhdr,string NwkSKey,string AppKey)
        {
            this.Mhdr = Mhdr;
            this.packet = packet;
            this.NwkSKey = NwkSKey;
            this.AppKey = AppKey;
            //this.FCntMSBytes = FCntMSBytes;
            CalculateMIC();
        }


        

        private void CalculateMIC()
        {
            //if (FCntMSBytes.Length  == 0)
            //    FCntMSBytes = new byte[]{0,0,0,0};

            byte Dir = (byte)(Mhdr.IsUpLink() ? 0 : 1);


            var msglen = Mhdr.mhdr.Length + packet.MACPayload.Length ;

            var result_B0= B0(msglen);

            List<byte> cmac_input =new List<byte>();
            cmac_input.AddRange(result_B0);
            cmac_input.AddRange(Mhdr.mhdr);
            cmac_input.AddRange(packet.MACPayload);

            var fullCAMC = new AESCMAC(NwkSKey).Encrypt(cmac_input.ToArray());

            MIC = fullCAMC.GetRange(0, 3);
        }


        /*
                * where B0 =
                *   0x49
                *   0x00 0x00 0x00 0x00
                *   direction-uplink/downlink [1]
                *   DevAddr [4]
                *   FCnt as 32-bit, lsb first [4]
                *   0x00
                *   message length [1]
         */
        private byte[] B0(int msglen)
        {
            var b0 = new List<byte>();
            byte Dir = (byte)(Mhdr.IsUpLink() ? 0 : 1);
            b0.AddRange(new byte[]{ 0x49, 0x00, 0x00, 0x00, 0x00 });
            b0.Add(Dir);
            b0.AddRange(packet.FHDR.DevAddr.Reverse());
            b0.AddRange(packet.FHDR.FCnt.Reverse());
            b0.AddRange(new byte[] { 0x00, 0x00 });
            b0.Add(0x00);
            b0.Add((byte)msglen);
            return b0.ToArray();

        }
    }
}
