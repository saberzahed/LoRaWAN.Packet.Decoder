using System;
using System.Collections;

namespace LoRaWAN.Packet.Decoder.Lib.Model
{
    public class MHDR
    {
        public byte[] mhdr;
        public MHDR(byte mhdr)
        {
            this.mhdr = new byte[] {mhdr};


            BitArray bitArray = new BitArray(this.mhdr);
            MType = SetMType(bitArray);
            RFU = SetRFU(bitArray);
            Major = SetMajor(bitArray);
            Hex = BitConverter.ToString(this.mhdr);
        }

        public MType MType { get; set; }
        public string RFU { get; set; }
        public Major Major { get; set; }

        public string Hex { get; set; }

        private MType SetMType(BitArray bitArray)
        {
            return (MType)Convert.ToInt32($"{(bitArray.Get(7) ? 1 : 0)}{(bitArray.Get(6) ? 1 : 0)}{(bitArray.Get(5) ? 1 : 0)}", 2);

        }


        private Major SetMajor(BitArray bitArray)
        {
            return (Major)Convert.ToInt32($"{(bitArray.Get(1) ? 1 : 0)}{(bitArray.Get(0) ? 1 : 0)}", 2);
        }

        private string SetRFU(BitArray bitArray)
        {
            return $"{(bitArray.Get(4) ? 1 : 0)}{(bitArray.Get(3) ? 1 : 0)}{(bitArray.Get(2) ? 1 : 0)}";
        }


        public bool IsJoinRequestMessage()
        {
            return MType == MType.JoinRequest;
        }

        public bool IsJoinAcceptMessage()
        {
            return MType == MType.JoinAccept;
        }

        public bool IsDataMessage()
        {
            return MType == MType.ConfirmedDataDown || MType == MType.ConfirmedDataUp || MType == MType.UnconfirmedDataDown || MType == MType.UnconfirmedDataUp;
        }


        public bool IsUpLink()
        {
            return MType == MType.ConfirmedDataUp || MType == MType.UnconfirmedDataUp;
        }
    }
}
