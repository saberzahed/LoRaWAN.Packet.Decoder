using System;
using LoRaWAN.Packet.Decoder.Lib;
using LoRaWAN.Packet.Decoder.Lib.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoRaWAN.PacketDecoder.Test
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void JoinRequestMessage()
        {
            var phyLoadData = Convert.FromBase64String("ANwAANB+1bNwHm/t9XzurwDIhgMK8sk=");
            var phyPayload = new PHYPayload(phyLoadData);
            var m = (JoinRequestMessage)phyPayload.Message;
            m.Pirnt();
        }

        [TestMethod]
        public void JoinAcceptMessage()
        {
            var phyLoadData = Convert.FromBase64String("IIE/R/UI/6JnC24j4B+EueJdnEEV8C7qCz3T4gs+ypLa");
            var phyPayload = new PHYPayload(phyLoadData);
            var m = (JoinAcceptMessage)phyPayload.Message;
            var ttt = m.Pirnt();
        }

        [TestMethod]
        public void DataMessage()
        {
            var phyLoadData = Convert.FromBase64String("QCkuASaAAAAByFaF53Iu+vzmwQ==");
            var phyPayload = new PHYPayload(phyLoadData);
            var m = (DataMessage)phyPayload.Message;
            var ttt = m.Pirnt();
        }


        [TestMethod]
        public void DataMessageWithKey()
        {
            var phyLoadData = Convert.FromBase64String("QG8UASYAKAAC7J3iMMFbFD/yefYg");
            var NwkSKey = "5ED438E5C86EDD00CE0ED6222A99E684";
            var AppSkey = "29CBD05A4CB9FBC5166A89E671C0EFCE";

            var phyPayload = new PHYPayload(phyLoadData, NwkSKey, AppSkey);
            var m = (DataMessageWithKey)phyPayload.Message;
            var ttt = m.Pirnt();
        }
    }
}
