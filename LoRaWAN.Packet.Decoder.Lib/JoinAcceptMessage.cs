using System;
using LoRaWAN.Packet.Decoder.Lib.Model;
using System.Linq;
namespace LoRaWAN.Packet.Decoder.Lib
{
    public class JoinAcceptMessage : ILoraMessage
    {
        private byte[] _packet;
        public JoinAcceptMessage(byte[] packet)
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



        public byte[] AppNonce { get => _MACPayload.GetRange(0, 2).Reverse().ToArray(); }

        public byte[] NetID { get => _MACPayload.GetRange(3, 5).Reverse().ToArray(); }

        public byte[] DevAddr { get => _MACPayload.GetRange(6, 9).Reverse().ToArray(); }

        public byte[] DLSettings { get => _MACPayload.GetRange(10, 10).Reverse().ToArray(); }

        public byte[] RxDelay { get => _MACPayload.GetRange(11, 11).Reverse().ToArray(); }
        public byte[] CFList { get => _packet.Length == 33 ? _MACPayload.GetRange(12, 28).ToArray() : new byte[] { }; }

        public int DLSettingsRxOneDRoffset
        {
            get => (DLSettings[0] & Constants.DLSETTINGS_RXONEDROFFSET_MASK) >> Constants.DLSETTINGS_RXONEDROFFSET_POS;
        }

        public int DLSettingsRxTwoDataRate
        {
            get => (DLSettings[0] & Constants.DLSETTINGS_RXTWODATARATE_MASK) >> Constants.DLSETTINGS_RXTWODATARATE_POS;
        }

        public int RxDelayDel
        {
            get => (RxDelay[0] & Constants.RXDELAY_DEL_MASK) >> Constants.RXDELAY_DEL_POS;
        }


        public byte[] CFListFreqChFour { get => CFList.Length == 16 ? CFList.GetRange(0, 2).ToArray() : new byte[] { }; }
        public byte[] CFListFreqChFive { get => CFList.Length == 16 ? CFList.GetRange(3, 5).ToArray() : new byte[] { }; }
        public byte[] CFListFreqChSix { get => CFList.Length == 16 ? CFList.GetRange(6, 8).ToArray() : new byte[] { }; }
        public byte[] CFListFreqChSeven { get => CFList.Length == 16 ? CFList.GetRange(9, 11).ToArray() : new byte[] { }; }
        public byte[] CFListFreqChEight { get => CFList.Length == 16 ? CFList.GetRange(12, 14).ToArray() : new byte[] { }; }
        public ILoraMessage Get()
        {
            return this;
        }

        public string Pirnt()
        {
            return $@"
Assuming base64-encoded packet
{_packet.Base64String()}

                Message Type = Join Accept 
                  PHYPayload =   {_packet.ToHexString()}

                ( PHYPayload = MHDR[1] | MACPayload[..] | MIC[4] )
                        MHDR =   {BitConverter.ToString(new byte[] { _packet[0] })}
                  MACPayload =   {MACPayload.ToHexString()}
                         MIC =   {MIC.ToHexString()}

                  ( MACPayload = AppNonce[3] | NetID[3] | DevAddr[4] | DLSettings[1] | RxDelay[1] | CFList[0|15] )
              AppNonce = {AppNonce.ToHexString()}
                 NetID = {NetID.ToHexString()}
               DevAddr = {DevAddr.ToHexString()}
            DLSettings = {DLSettings.ToHexString()}
               RxDelay = {RxDelay.ToHexString()}
                CFList = {CFList.ToHexString()}
                     {(CFList.Length == 15 ? $@"
                                                ( CFList = FreqCh4[3] | FreqCh5[3] | FreqCh6[3] | FreqCh7[3] | FreqCh8[3] )

                               FreqCh4 = {CFListFreqChFour.ToHexString()}
                              FreqCh5 = {CFListFreqChFive.ToHexString()} 
                              FreqCh6 =  {CFListFreqChSix.ToHexString()}
                              FreqCh7 =  {CFListFreqChSeven.ToHexString()} 
                              FreqCh8 =  {CFListFreqChEight.ToHexString()} 

                    " : "")}


DLSettings.RX1DRoffset =  {DLSettingsRxOneDRoffset}
DLSettings.RX2DataRate =  {DLSettingsRxTwoDataRate}
           RxDelay.Del =  {RxDelayDel}
";
        }
    }
}
