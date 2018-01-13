using System;
using Helper;

namespace SnesXM.Emulator.Apu
{
    partial class SpcDsp
    {
        public class Voice
        {
            public const int BrrBufferSize = 12;

            public const int VolLeftIndex = 0x00;
            public const int VolRightIndex = 0x01;
            public const int PitchLowIndex = 0x02;
            public const int PitchHighIndex = 0x03;
            public const int SrcnIndex = 0x04;
            public const int Adsr0Index = 0x05;
            public const int Adsr1Index = 0x06;
            public const int GainIndex = 0x07;
            public const int EnvXIndex = 0x08;
            public const int OutXIndex = 0x09;

            private SpcDsp SpcDsp
            {
                get;
                set;
            }
            private State M => SpcDsp.M;
            private Voice[] Voices => M.Voices;

            internal int[] Buffer
            {
                get;
                set;
            }
            internal int BufferIndex
            {
                get;
                set;
            }
            internal int InterpIndex
            {
                get;
                set;
            }
            internal int BrrAddress
            {
                get;
                set;
            }
            internal int BrrIndex
            {
                get;
                set;
            }
            internal Pointer<byte> Registers
            {
                get;
                set;
            }
            internal int Bitmask
            {
                get;
                set;
            }
            internal int KOnDelay
            {
                get;
                set;
            }
            internal EnvelopeMode EnvelopeMode
            {
                get;
                set;
            }
            internal int Envelope
            {
                get;
                set;
            }
            internal int HiddenEnvelope
            {
                get;
                set;
            }
            internal byte EnvXOut
            {
                get;
                set;
            }
            internal int VoiceNumber
            {
                get;
                set;
            }

            public Voice(SpcDsp spcDsp)
            {
                SpcDsp = spcDsp ?? throw new ArgumentNullException(nameof(spcDsp));

                Buffer = new int[BrrBufferSize << 1];
            }

            public int Interpolate()
            {
                var offset = (byte)(InterpIndex >> 4);

                var fwd = 0x100 - offset;
                var rev = offset;

                var src = (InterpIndex >> 12) + BufferIndex;
                int result;
                result  = (Guass[fwd + 0x000] * Buffer[src + 0])  >> 11;
                result += (Guass[fwd + 0x100] * Buffer[src + 1]) >> 11;
                result += (Guass[rev + 0x100] * Buffer[src + 2]) >> 11;
                result  = (short)result;
                result += (Guass[rev + 0x000] * Buffer[src + 3]) >> 11;

                result = ClampInt16(result);
                result &= ~1;
                return result;
            }

            public void RunEnvelope()
            {
                var env = Envelope;
                if (EnvelopeMode == EnvelopeMode.Release)
                {
                    if ((env -= 8) < 0)
                        env = 0;
                    Envelope = env;
                }
                else
                {
                    int rate;
                    var envData = Registers[Adsr1Index];
                    if ((M.Adsr0 & 0x80) != 0)
                    {
                        if (EnvelopeMode >= EnvelopeMode.Decay)
                        {
                            env--;
                            env -= env >> 8;
                            rate = envData & 0x1F;
                            if (EnvelopeMode == EnvelopeMode.Decay)
                                rate = ((M.Adsr0 >> 3) & 0x0E) | 0x10;
                        }
                        else
                        {
                            rate = ((M.Adsr0 & 0x0F) << 1) | 1;
                            env += rate < 0x1F ? 0x20 : 0x400;
                        }
                    }
                    else
                    {
                        envData = Registers[GainIndex];
                        switch (envData >> 5)
                        {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            env = envData << 4;
                            rate = 0x1F;
                            break;
                        case 5:
                            env--;
                            env -= env >> 8;
                            rate = envData & 0x1F;
                            break;
                        case 4:
                            env -= 0x20;
                            rate = envData & 0x1F;
                            break;
                        case 6:
                            env += 0x20;
                            rate = envData & 0x1F;
                            break;
                        case 7:
                            env += HiddenEnvelope >= 0x600 ?
                                0x08 : 0x20;
                            rate = envData & 0x1F;
                            break;
                        default:
                            throw new Exception("Gauranteed to never call.");
                        }
                    }

                    if (env >> 8 == envData >> 5 && EnvelopeMode == EnvelopeMode.Decay)
                        EnvelopeMode = EnvelopeMode.Sustain;

                    HiddenEnvelope = env;

                    if ((uint)env > 0x7FF)
                    {
                        env = env < 0 ? 0 : 0x7FF;
                        if (EnvelopeMode == EnvelopeMode.Attack)
                            EnvelopeMode = EnvelopeMode.Decay;
                    }

                    if (M.ReadCounter(rate) == 0)
                        Envelope = env;
                }
            }

            public void DecodeBrr()
            {
                var nybbles = (M.BrrByte << 8) | M.Ram[(ushort)(BrrAddress + BrrIndex + 1)];

                var header = M.BrrHeader;

                var pos = BufferIndex;
                if ((BufferIndex += 4) >= BrrBufferSize)
                    BufferIndex = 0;

                for (var end = pos + 4; pos < end; pos++, nybbles <<= 4)
                {
                    int s = (short)nybbles >> 12;

                    var shift = header >> 4;
                    s = (s << shift) >> 1;
                    if (shift > 0x0D)
                        s = s < 0 ? -0x800 : 0;

                    var filter = header & 0x0C;
                    var p1 = Buffer[pos + BrrBufferSize - 1];
                    var p2 = Buffer[pos + BrrBufferSize - 2] >> 1;
                    if (filter >= 8)
                    {
                        s += p1;
                        s -= p2;
                        if (filter == 8)
                        {
                            s += p2 >> 4;
                            s += (p1 * -3) >> 6;
                        }
                        else
                        {
                            s += (p1 * -13) >> 7;
                            s += (p2 * 3) >> 4;
                        }
                    }
                    else if (filter != 0)
                    {
                        s += p1 >> 1;
                        s += (-p1) >> 5;
                    }

                    s = ClampInt16(s);
                    s = (short)(s << 1);
                    Buffer[pos + BrrBufferSize] = Buffer[pos] = s;
                }
            }

            internal void VoiceClockV1()
            {
                M.DirAddress = (M.Dir << 8) + (M.Srcn << 2);
                M.Srcn = Registers[SrcnIndex];
            }
            internal void VoiceClockV2()
            {
                var entry = M.DirAddress;
                if (KOnDelay == 0)
                    entry += 2;
                M.BrrNextAddress = BitConverter.ToInt16(M.Ram, entry);

                M.Adsr0 = Registers[Adsr0Index];

                M.Pitch = Registers[PitchLowIndex];
            }
            internal void VoiceClockV3a()
            {
                M.Pitch += (Registers[PitchHighIndex] & 0x3F) << 8;
            }
            internal void VoiceClockV3b()
            {
                M.BrrByte = M.Ram[(ushort)(BrrAddress + BrrIndex)];
                M.BrrHeader = M.Ram[BrrAddress];
            }
            internal void VoiceClockV3c()
            {
                if ((M.Pmon & Bitmask) != 0)
                    M.Pitch += ((M.Output >> 5) * M.Pitch) >> 10;

                if (KOnDelay != 0)
                {
                    if (KOnDelay == 5)
                    {
                        BrrAddress = M.BrrNextAddress;
                        BrrIndex = 1;
                        BufferIndex = 0;
                        M.BrrHeader = 0;
                        M.KOnCheck = true;

                        SpcDsp.CallSnapshot();
                    }

                    Envelope = 0;
                    HiddenEnvelope = 0;

                    InterpIndex = 0;
                    if ((--KOnDelay & 3) != 0)
                        InterpIndex = 0x4000;

                    M.Pitch = 0;
                }

                {
                    var output = Interpolate();

                    if ((M.Non & Bitmask) != 0)
                        output = (short)(M.Noise << 1);

                    M.Output = ((output * Envelope) >> 11) & ~1;
                    EnvXOut = (byte)(Envelope >> 4);
                }

                if ((Registers[FlgIndex] & 0x80) != 0 || (M.BrrHeader & 3) == 1)
                {
                    EnvelopeMode = EnvelopeMode.Release;
                    Envelope = 0;
                }

                if (M.EveryOtherSample)
                {
                    if ((M.KOff & Bitmask) != 0)
                        EnvelopeMode = EnvelopeMode.Release;

                    if ((M.KOn & Bitmask) != 0)
                    {
                        KOnDelay = 5;
                        EnvelopeMode = EnvelopeMode.Attack;
                    }
                }

                if (KOnDelay == 0)
                    RunEnvelope();
            }
            internal void VoiceClockV3()
            {
                VoiceClockV3a();
                VoiceClockV3b();
                VoiceClockV3c();
            }
            internal void VoiceClockV4()
            {
                M.Looped = 0;
                if (InterpIndex >= 0x4000)
                {
                    DecodeBrr();

                    if ((BrrIndex += 2) >= BrrBlockSize)
                    {
                        BrrAddress = (ushort)(BrrAddress + BrrBlockSize);
                        if ((M.BrrHeader & 1) != 0)
                        {
                            BrrAddress = M.BrrNextAddress;
                            M.Looped = Bitmask;
                        }
                        BrrIndex = 1;
                    }
                }

                InterpIndex &= 0x3FFF;
                InterpIndex += M.Pitch;

                if (InterpIndex > 0x7FFF)
                    InterpIndex = 0x7FFF;

                VoiceOutput(Channel.Left);
            }
            internal void VoiceClockV5()
            {
                VoiceOutput(Channel.Right);

                var endxBuf = M.Registers[EndXIndex] | M.Looped;

                if (KOnDelay == 5)
                    endxBuf &= ~Bitmask;
                M.EndXBuf = (byte)endxBuf;
            }
            internal void VoiceClockV6()
            {
                M.OutXBuf = (byte)(M.Output >> 8);
            }
            internal void VoiceClockV7()
            {
                M.Registers[EndXIndex] = M.EndXBuf;

                M.EnvXBuf = EnvXOut;
            }
            internal void VoiceClockV8()
            {
                Registers[OutXIndex] = (byte)M.OutXBuf;
            }
            internal void VoiceClockV9()
            {
                Registers[EnvXIndex] = (byte)M.EnvXBuf;
            }

            private void VoiceOutput(Channel channel)
            {
                var vShift = VoiceNumber;
                vShift += (int)channel * VoiceCount;

                var amp = (SpcDsp.StereoSwitch & (1 << vShift)) == 0 ?
                    0 : (M.Output * (sbyte)Registers[VolLeftIndex + (int)channel]) >> 7;

                var total = M._mainOut[channel];
                total = ClampInt16(total + amp);
                M._mainOut[channel] = total;

                if ((M.Eon & Bitmask) != 0)
                {
                    total = M._echoOut[channel];
                    total = ClampInt16(total + amp);
                    M._echoOut[channel] = total;
                }
            }
        }
    }
}
