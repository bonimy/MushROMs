namespace SnesXM.Emulator
{
    public enum AccessMode
    {
        None = 0,
        Read,
        Write,
        Modify,
        Jump = 5,
        Jsr = 8
    }
}
