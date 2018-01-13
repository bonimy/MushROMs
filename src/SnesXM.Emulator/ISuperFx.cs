using System;
using System.Collections.Generic;
using System.Text;

namespace SnesXM.Emulator
{
    public interface ISuperFx
    {
        bool OneLineDone { get; set; }

        void Initialize();

        void Execute();
    }
}
