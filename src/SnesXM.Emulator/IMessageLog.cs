namespace SnesXM.Emulator
{
    public interface IMessageLog
    {
        void Message(MessageType messageType, string text);
    }
}
