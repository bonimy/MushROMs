using Helper;

namespace SnesXM.Emulator
{
    public interface IC4
    {
        void Initialize();
        Pointer<byte> GetMemPointer(int index);
        Pointer<byte> GetBasePointer(int index);
    }
}
