using System;
using LoRaWAN.Packet.Decoder.Lib.Model;

namespace LoRaWAN.Packet.Decoder.Lib
{
    public class DataMessage : ILoraMessage
    {

        protected byte[] _packet;
        protected MHDR mhdr;
        public DataMessage(byte[] packet, MHDR mhdr)
        {
            this._packet = packet;
            this.mhdr = mhdr;
        }

        private byte[] _MACPayload { get => _packet.GetRange(1, _packet.Length - 5); }

        private byte[] _MIC
        {
            get => _packet.GetRange(_packet.Length - 4);
        }

        public byte[] MACPayload
        {
            get => _MACPayload;
        }

        public byte[] MIC
        {
            get => _MIC;
        }

        public byte[] FCtrl { get => MACPayload.GetRange(4, 8); }

        private sbyte FOptsLen
        {
            get => (sbyte)(BitConverter.ToInt32(FCtrl, 0) & 0x0f);
        }
        private int FHDR_length
        {
            get => 7 + FOptsLen;
        }

        public byte[] FOpts
        {
            get => MACPayload.GetRange(7, 7 + FOptsLen);
        }

        public byte[] Fport
        {
            get => FHDR_length != MACPayload.Length ? MACPayload.GetRange(FHDR_length, FHDR_length) : new byte[] { };
        }


        public byte[] FRMPayload
        {
            get => FHDR_length != MACPayload.Length ? MACPayload.GetRange(FHDR_length + 1) : new byte[] { };
        }






        public FHDR FHDR { get => new FHDR(MACPayload, MACPayload.GetRange(0, FHDR_length - 1)); }

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

                ( PHYPayload = MHDR[1] | MACPayload[..] | MIC[4] )
                        MHDR =   {BitConverter.ToString(new byte[] { _packet[0] })}
                  MACPayload =   {MACPayload.ToHexString()}
                         MIC =   {MIC.ToHexString()}

                ( MACPayload =  FHDR | FPort | FRMPayload )
                      FHDR = {FHDR.ToString()}
                      FPort = {Fport.ToHexString()}
                    FRMPayload = {FRMPayload.ToHexString()}
                ( FHDR = DevAddr[4] | FCtrl[1] | FCnt[2] | FOpts[0..15] )
                     DevAddr = {FHDR.DevAddr.ToHexString()} (Big Endian)
                       FCtrl = {FHDR.FCTRL.ToString()}
                        FCnt = {FHDR.FCnt.ToHexString()} (Big Endian)
                       FOpts = {FHDR.FOpts.ToHexString()}

                    Message Type = {mhdr.MType.ToString()}
                       Direction = {((mhdr.MType == MType.UnconfirmedDataUp || mhdr.MType == MType.ConfirmedDataUp)?"Up":"Down") }
                            FCnt = {FHDR.FCnt.ToInt16()}
                       FCtrl.ACK = {FHDR.FCTRL.ACK}
                       FCtrl.ADR = {FHDR.FCTRL.ADR}

";
        }
    }
}