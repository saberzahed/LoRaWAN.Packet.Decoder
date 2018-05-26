namespace LoRaWAN.Packet.Decoder.Lib.Model
{
    public interface ILoraMessage
    {

        byte[] MACPayload { get; }
        byte[] MIC { get; }
        ILoraMessage Get();

        string Pirnt();
    }
}
