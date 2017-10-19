namespace MushROMs.NES
{
    public static class AddressConverter
    {
        public static int NesToPc(int nes)
        {
            return nes - 0x7FF0;
        }

        public static int PcToNes(int pc)
        {
            return pc + 0x7FF0;
        }
    }
}
