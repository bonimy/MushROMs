namespace SnesXM.Emulator
{
    public enum CpuEvent
    {
        HBlankStart = 1,
        HdmaStart,
        HCounterMax,
        HdmaInit,
        Render,
        WramRefresh
    }
}
