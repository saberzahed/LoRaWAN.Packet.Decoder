using System;
using System.Collections.Generic;
using System.Linq;
using LoRaWAN.Packet.Decoder.Lib.Model;

namespace LoRaWAN.Packet.Decoder.Lib
{
    public class DataMessageWithKey : DataMessage, ILoraMessage
    {
        private string NwkSKey;
        private string AppSKey;
        private MICChecker MICChecker;
        public DataMessageWithKey(byte[] packet, MHDR mhdr, string NwkSKey, string AppSKey) : base(packet, mhdr)
        {
            this.NwkSKey = NwkSKey;
            this.AppSKey = AppSKey;
            this.MICChecker = new MICChecker(this, mhdr, NwkSKey, AppSKey);
        }

        private (bool, string, byte[]) DecryptFRMPayload()
        {
            var packet = base.FRMPayload;


            var key = int.Parse(base.Fport.ToHexString()) == 0 ? NwkSKey : AppSKey;

            var blocks = Math.Ceiling(base.FRMPayload.Length / 16d);
            var pLen = packet.Length;

            List<byte> plain_S = new List<byte>();

            for (var block = 0; block < blocks; block++)
            {
                var Ai = A(block);
                plain_S.AddRange(Ai);
            }

            var cipherstream = new AES(key).Encrypt(plain_S.ToArray());

            var plaintextPayload = new List<byte>();

            for (var j = 0; j < pLen; j++)
            {
                byte r = (byte)(packet[j] ^ cipherstream[j]);
                plaintextPayload.Add(r);
            }
            var resultBytes = plaintextPayload.ToArray();
            return (true, resultBytes.ToHexString(), resultBytes);
        }

        public ILoraMessage Get()
        {
            return this;
        }

        public string Pirnt()
        {

            return $@"
                Assuming base64-encoded packet
                {_packet.Base64String()}

                Message Type = Data
                  PHYPayload =   {_packet.ToHexString()}

                ( PHYPayload   = MHDR[1] | MACPayload[..] | MIC[4] )
                        MHDR   =   {BitConverter.ToString(new byte[] { _packet[0] })}
                  MACPayload   =   {MACPayload.ToHexString()}
                         MIC   =    {MIC.ToHexString()} (from packet)  {(MIC.ToHexString() != MICChecker.MIC.ToHexString()?"INVALID":"") }
                               =    {MICChecker.MIC.ToHexString()} (expected, assuming 32 bits frame counter below 65,536)

                ( MACPayload   =  FHDR | FPort | FRMPayload )
                      FHDR     = {FHDR.ToString()}
                      FPort    = {Fport.ToHexString()}
                    FRMPayload = {FRMPayload.ToHexString()} (from packet, encrypted)
                               = {DecryptFRMPayload().Item2} (decrypted)

                ( FHDR = DevAddr[4] | FCtrl[1] | FCnt[2] | FOpts[0..15] )
                     DevAddr = {FHDR.DevAddr.ToHexString()} (Big Endian)
                       FCtrl = {FHDR.FCTRL.ToString()}
                        FCnt = {FHDR.FCnt.ToHexString()} (Big Endian)
                       FOpts = {FHDR.FOpts.ToHexString()}

                        Message Type = {mhdr.MType.ToString()}
                           Direction = {((mhdr.MType == MType.UnconfirmedDataUp || mhdr.MType == MType.ConfirmedDataUp) ? "Up" : "Down") }
                                FCnt = {FHDR.FCnt.ToInt16()}
                           FCtrl.ACK = {FHDR.FCTRL.ACK}
                           FCtrl.ADR = {FHDR.FCTRL.ADR}

";
        }

        private byte[] A(int index)
        {

            // Encrypt stream mixes in metadata blocks, as Ai =
            //   0x01
            //   0x00 0x00 0x00 0x00
            //   direction-uplink/downlink [1]
            //   DevAddr [4]
            //   FCnt as 32-bit, lsb first [4]
            //   0x00
            //   counter = i [1]

            uint dir = (uint)((base.mhdr.MType == MType.ConfirmedDataUp || base.mhdr.MType == MType.UnconfirmedDataUp) ? 0 : 1);

            List<byte> result_A = new List<byte>();

            result_A.AddRange(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00 });    //   0x01 0x00 0x00 0x00 0x00
            result_A.Add(Convert.ToByte(dir));  //   direction-uplink/downlink [1]

            result_A.AddRange(base.FHDR.DevAddr.Reverse());    //   DevAddr [4]
            result_A.AddRange(base.FHDR.FCnt.Reverse());
            result_A.AddRange(new byte[] { 0x00, 0x00 });   //   FCnt as 32-bit, lsb first [4]
            result_A.AddRange(new byte[] { 0x00 });   //   0x00
            result_A.Add(Convert.ToByte(index + 1));       //   counter = i [1]

            return result_A.ToArray().ToArray();
        }
    }
}