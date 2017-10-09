namespace SnesXM.Emulator
{
    public interface IEmulator
    {
        IMessageLog MessageLog { get; }

        ISettings Settings { get; }

        IMultiCart MultiCart { get; }

        IMemoryMap MemoryMap { get; }

        IInternalPpu InternalPpu { get; }

        ISuperFx SuperFx { get; }
    }
}
