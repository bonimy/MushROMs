namespace SnesXM.Emulator
{
    public interface IEmulator
    {
        int OpenBus { get; set; }

        IMessageLog MessageLog { get; }

        ISettings Settings { get; }

        IDisplay Display { get; }

        IMultiCart MultiCart { get; }

        IMemoryMap MemoryMap { get; }

        IInternalPpu InternalPpu { get; }

        ISuperFx SuperFx { get; }

        IControls Controls { get; }

        ITimings Timings { get; }

        ICpu Cpu { get; }

        IGfx Gfx { get; }

        IPort Port { get; }

        ICheats Cheats { get; }

        byte SramInitialValue { get; set; }

        bool Uniracers { get; set; }
    }
}
