namespace LoRaWAN.Packet.Decoder.Lib
{
    public   class Constants
    {
       public static byte FCTRL_ADR= 0x80;
       public static byte FCTRL_ADRACKREQ= 0x40;
       public static byte FCTRL_ACK= 0x20;
       public static byte FCTRL_FPENDING= 0x10;
       public static byte DLSETTINGS_RXONEDROFFSET_MASK= 0x70;
       public static byte DLSETTINGS_RXONEDROFFSET_POS= 4;
       public static byte DLSETTINGS_RXTWODATARATE_MASK= 0x0F;
       public static byte DLSETTINGS_RXTWODATARATE_POS= 0;
       public static byte RXDELAY_DEL_MASK= 0x0F;
        public static byte RXDELAY_DEL_POS = 0;
    }
}
