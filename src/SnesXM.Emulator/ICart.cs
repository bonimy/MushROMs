namespace SnesXM.Emulator
{
    public interface ICart
    {
        int Offset { get; }

        void Initialize();
    }
}
