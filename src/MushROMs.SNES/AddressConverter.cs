namespace MushROMs.SNES
{
    public class AddressConverter
    {
        public static int MemoryToLoRom(int memoryAddress)
        {
            if (memoryAddress < 0)
                return -1;

            if (memoryAddress >= 0x1000000)
                return -1;

            var bank = memoryAddress & 0xFF0000;

            var offset = memoryAddress & 0xFFFF;

            if (offset < 0x8000)
                return -1;

            if (bank >= 0x800000)
                bank &= 0x7F0000;
            else if (bank >= 0x7E0000)
                return -1;

            return (bank >> 1) | (offset & 0x7FFF);
        }
    }
}
