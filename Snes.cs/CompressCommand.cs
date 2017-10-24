namespace Snes
{
    public enum CompressCommand
    {
        DirectCopy = 0,
        RepeatedByte = 1,
        RepeatedWord = 2,
        IncrementingByte = 3,
        CopySection = 4,
        LongCommand = 7
    }
}
