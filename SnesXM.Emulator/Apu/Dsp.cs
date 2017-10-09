using System;
using Helper;

namespace SnesXM.Emulator.Apu
{
    public delegate void DspCopyFunction(SharedArray<byte> buffer, Array state);

    public class Dsp
    {
        internal static void FromDspToState(SharedArray<byte> buffer, Array state)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            var size = Buffer.ByteLength(state);
            Buffer.BlockCopy(state, 0, buffer.GetArray(), buffer.Offset, size);
            buffer.Offset += size;
        }

        internal static void ToDspFromState(SharedArray<byte> buffer, Array state)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            var size = Buffer.ByteLength(state);
            Buffer.BlockCopy(buffer.GetArray(), buffer.Offset, state, 0, size);
            buffer.Offset += size;
        }
    }
}
