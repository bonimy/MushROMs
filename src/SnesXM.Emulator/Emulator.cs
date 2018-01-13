using System;
using System.Collections.Generic;
using System.Text;

namespace SnesXM.Emulator
{
    public class Emulator : IEmulator
    {
        public int OpenBus { get; set; }
        public IMessageLog MessageLog { get; private set; }
        public ISettings Settings { get; private set; }
        public IDisplay Display { get; private set; }
        public IMultiCart MultiCart { get; private set; }
        public IMemoryMap MemoryMap { get; private set; }
        public IInternalPpu InternalPpu { get; private set; }
        public ISuperFx SuperFx { get; private set; }
        public IControls Controls { get; private set; }
        public ITimings Timings { get; private set; }
        public ICpu Cpu { get; private set; }
        public IGfx Gfx { get; private set; }
        public IPort Port { get; private set; }
        public ICheats Cheats { get; private set; }
        public byte SramInitialValue { get; set; }
        public bool Uniracers { get; set; }

        public Emulator()
        {
            Settings = new Settings();
            Display = new Display();
            //MemoryMap = new MemoryMap(this);
        }
    }
}
