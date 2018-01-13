namespace SnesXM.Emulator
{
    public interface IMultiCart
    {
        int CartType { get; }
        ICart A { get; }
        ICart B { get; }

        void Initialize();
    }
}
