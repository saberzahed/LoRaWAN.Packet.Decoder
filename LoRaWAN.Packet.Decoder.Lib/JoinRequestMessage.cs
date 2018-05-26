using System;
using System.Linq;
using LoRaWAN.Packet.Decoder.Lib.Model;

namespace LoRaWAN.Packet.Decoder.Lib
{
    public class JoinRequestMessage : ILoraMessage
    {
        private byte[] _packet;
        public JoinRequestMessage(byte[] packet)
        {
            _packet = packet;
            _MACPayload = packet.GetRange(1, packet.Length - 5);
            _MIC = packet.GetRange(packet.Length - 4);
        }

        private byte[] _MACPayload;

        private byte[] _MIC;

        public byte[] MACPayload
        {
            get => _MACPayload;
        }

        public byte[] MIC
        {
            get => _MIC;
        }



        public byte[] AppEUI { get => _MACPayload.GetRange(0, 7).Reverse().ToArray(); }

        public byte[] DevEUI { get => _MACPayload.GetRange(8, 15).Reverse().ToArray(); }
        public byte[] DevNonce
        {
            get => _MACPayload.GetRange(16, 18).Reverse().ToArray();
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

                Message Type = Join Request
                  PHYPayload =   {_packet.ToHexString()}

                ( PHYPayload = MHDR[1] | MACPayload[..] | MIC[4] )
                        MHDR =   {BitConverter.ToString(new byte[] { _packet[0] })}
                  MACPayload =   {MACPayload.ToHexString()}
                         MIC =   {MIC.ToHexString()}

                ( MACPayload = AppEUI[8] | DevEUI[8] | DevNonce[2] )
                      AppEUI = {AppEUI.ToHexString()}
                      DevEUI = {DevEUI.ToHexString()}
                    DevNonce = {DevNonce.ToHexString()} ";
        }
    }
}
