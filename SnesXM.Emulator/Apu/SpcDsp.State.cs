using System;
using Helper;
using Debug = System.Diagnostics.Debug;

namespace SnesXM.Emulator.Apu
{
    partial class SpcDsp
    {
        public class State
        {
            private delegate void PhaseOperation();

            private SpcDsp SpcDsp
            {
                get;
                set;
            }

            internal byte[] Registers
            {
                get;
                set;
            }

            internal Stereo[] EchoHistory
            {
                get;
                set;
            }
            internal int EchoHistoryIndex
            {
                get;
                set;
            }

            internal bool EveryOtherSample
            {
                get;
                set;
            }
            internal int KOn
            {
                get;
                set;
            }
            internal int Noise
            {
                get;
                set;
            }
            internal int Counter
            {
                get;
                set;
            }
            internal int EchoOffset
            {
                get;
                set;
            }
            internal int EchoLength
            {
                get;
                set;
            }
            public int Phase
            {
                get;
                set;
            }
            internal bool KOnCheck
            {
                get;
                set;
            }

            public int NewKOn
            {
                get;
                set;
            }
            public byte EndXBuf
            {
                get;
                set;
            }
            public int EnvXBuf
            {
                get;
                set;
            }
            public int OutXBuf
            {
                get;
                set;
            }

            internal int Pmon
            {
                get;
                set;
            }
            internal int Non
            {
                get;
                set;
            }
            internal int Eon
            {
                get;
                set;
            }
            internal int Dir
            {
                get;
                set;
            }
            internal int KOff
            {
                get;
                set;
            }

            internal int BrrNextAddress
            {
                get;
                set;
            }
            internal int Adsr0
            {
                get;
                set;
            }
            internal int BrrHeader
            {
                get;
                set;
            }
            internal int BrrByte
            {
                get;
                set;
            }
            internal int Srcn
            {
                get;
                set;
            }
            internal int Esa
            {
                get;
                set;
            }
            internal int EchoEnabled
            {
                get;
                set;
            }

            internal int DirAddress
            {
                get;
                set;
            }
            internal int Pitch
            {
                get;
                set;
            }
            internal int Output
            {
                get;
                set;
            }
            internal int Looped
            {
                get;
                set;
            }
            internal int EchoPtr
            {
                get;
                set;
            }

            internal Stereo _mainOut;
            internal Stereo _echoOut;
            internal Stereo _echoIn;

            internal Voice[] Voices
            {
                get;
                set;
            }

            internal byte[] Ram
            {
                get;
                set;
            }
            public int MuteMask
            {
                get;
                set;
            }
            private SharedArray<short> Out
            {
                get;
                set;
            }
            private int OutEnd
            {
                get;
                set;
            }
            private int OutBegin
            {
                get;
                set;
            }
            private short[] Extra
            {
                get;
                set;
            }

            private PhaseOperation[] PhaseOperations
            {
                get;
                set;
            }

            public int SampleCount => Out.Offset - OutBegin;

            public int this[int index]
            {
                get => Registers[index];
                set
                {
                    Registers[index] = (byte)value;
                    switch (index & 0x0F)
                    {
                    case Voice.EnvXIndex:
                        EnvXBuf = (byte)value;
                        break;

                    case Voice.OutXIndex:
                        OutXBuf = (byte)value;
                        break;

                    case 0x0C:
                        if (index == KOn)
                            NewKOn = (byte)value;

                        if (index == EndXIndex)
                        {
                            EndXBuf = 0;
                            Registers[EndXIndex] = 0;
                        }
                        break;
                    }
                }
            }

            public State(SpcDsp spcDsp, byte[] ram)
            {
                SpcDsp = spcDsp ?? throw new ArgumentNullException(nameof(spcDsp));

                if (ram == null)
                    throw new ArgumentNullException(nameof(ram));
                if (ram.Length != 0x40 * 0x400)
                    throw new ArgumentException();

                Ram = ram;

                Registers = new byte[RegisterCount];
                EchoHistory = new Stereo[EchoHistorySize << 1];
                Voices = new Voice[VoiceCount];
                Extra = new short[ExtraSize];

                InitializePhaseOperations();
            }

            private void Run(int clocksRemain)
            {
                if (clocksRemain < 0)
                    throw new ArgumentOutOfRangeException(nameof(clocksRemain));

                var phase = Phase;
                Phase += clocksRemain;
                Phase &= 0x1F;

                do PhaseOperations[phase++ & 0x1F]();
                while (--clocksRemain >= 0);
            }

            internal void SetOutput(SharedArray<short> output, int size)
            {
                Debug.Assert((size & 1) == 0);
                if (output == null)
                {
                    output = new SharedArray<short>(Extra);
                    size = ExtraSize;
                }

                Out = output;
                OutBegin = output.Offset;
                OutEnd = OutBegin + size;
            }

            internal void MuteVoices(int mask)
            {
                MuteMask = mask;
            }

            internal void Reset()
            {
                Load(InitialRegisters);
            }

            private void SoftReset()
            {
                Registers[FlgIndex] = 0xE0;
                SoftResetCommon();
            }

            private void SoftResetCommon()
            {
                Noise = 0x4000;
                EchoHistoryIndex = 0;
                EveryOtherSample = true;
                EchoOffset = 0;
                Phase = 0;

                InitCounter();

                for (int i = VoiceCount; --i >= 0;)
                    Voices[i].VoiceNumber = i;
            }

            private void Load(byte[] data)
            {
                Array.Clear(Registers, 0, RegisterCount);

                for (int i = VoiceCount; --i >= 0;)
                {
                    var v = Voices[i];
                    v.BrrIndex = 1;
                    v.Bitmask = 1 << i;
                    v.Registers = new SharedArray<byte>(Registers, i << 4);
                }

                NewKOn = Registers[KOn];
                Dir = Registers[Dir];
                Esa = Registers[Esa];

                SoftResetCommon();
            }

            private int RegValue(int voice, int address)
            {
                return Voices[voice].Registers[address];
            }

            private int EnvxValue(int voice)
            {
                return Voices[voice].Envelope;
            }

            private void InitCounter()
            {
                Counter = 0;
            }

            private void RunCounters()
            {
                if (--Counter < 0)
                    Counter = SimpleCounterRange - 1;
            }

            internal int ReadCounter(int rate)
            {
                rate &= 0x1F;
                return (Counter + CounterOffsets[rate]) % CounterRates[rate];
            }

            private void MiscClock27()
            {
                Pmon = Registers[PmonIndex] & ~1;
            }
            private void MiscClock28()
            {
                Non = Registers[NonIndex];
                Eon = Registers[EonIndex];
                Dir = Registers[DirIndex];
            }
            private void MiscClock29()
            {
                if (EveryOtherSample ^= true)
                    NewKOn &= ~KOn;
            }
            private void MiscClock30()
            {
                if (EveryOtherSample)
                {
                    KOn = NewKOn;
                    KOff = Registers[KOffIndex] | MuteMask;
                }

                RunCounters();

                var flg = Registers[FlgIndex];
                if (ReadCounter(flg) == 0)
                {
                    var feedback = (Noise << 13) ^ (Noise << 14);
                    Noise = (feedback & 0x4000) ^ (Noise >> 1);
                }
            }

            private int GetEcho(Channel channel)
            {
                return BitConverter.ToInt16(Ram, EchoPtr + ((int)channel << 1));
            }
            private void SetEcho(Channel channel, int value)
            {
                Ram[EchoPtr + ((int)(channel) << 1)] = (byte)value;
                Ram[EchoPtr + ((int)(channel) << 1) + 1] = (byte)(value >> 8);
            }

            private void EchoRead(Channel channel)
            {
                var s = GetEcho(channel);
                EchoHistory[EchoHistoryIndex][channel] =
                EchoHistory[EchoHistoryIndex + 8][channel] = s >> 1;
            }

            private Stereo CalcFir(int index)
            {
                var fir = EchoHistory[EchoHistoryIndex + index + 1];
                fir *= (sbyte)Registers[FirIndex + index * 0x10];
                fir >>= 6;
                return fir;
            }

            private void EchoClock22()
            {
                if (++EchoHistoryIndex > EchoHistorySize)
                    EchoHistoryIndex = 0;

                EchoPtr = (ushort)(Esa * 0x100 + EchoOffset);
                EchoRead(Channel.Left);

                _echoIn = CalcFir(0);
            }
            private void EchoClock23()
            {
                _echoIn += CalcFir(1);
                _echoIn += CalcFir(2);

                EchoRead(Channel.Right);
            }
            private void EchoClock24()
            {
                _echoIn += CalcFir(3);
                _echoIn += CalcFir(4);
                _echoIn += CalcFir(5);
            }
            private void EchoClock25()
            {
                var echo = _echoIn + CalcFir(6);
                echo = echo.AsInt16();
                echo += CalcFir(7).AsInt16();

                _echoIn = echo & ~1;
            }
            private int EchoOutput(Channel channel)
            {
                var mVol = (sbyte)Registers[MVolLeftIndex + ((int)channel << 4)];
                var eVol = (sbyte)Registers[EVolLeftIndex + ((int)channel << 4)];

                var main = (short)_mainOut[channel];
                var echo = (short)_echoIn[channel];

                main *= mVol;
                echo *= eVol;

                return ClampInt16(main + echo);
            }
            private void EchoClock26()
            {
                _mainOut[Channel.Left] = EchoOutput(Channel.Left);

                var echo = _echoOut + ((_echoIn * (sbyte)Registers[EfbIndex]) >> 7).AsInt16();

                echo = new Stereo(
                    ClampInt16(echo.Left),
                    ClampInt16(echo.Right));

                _echoOut = echo & ~1;
            }
            private void EchoClock27()
            {
                var echo = new Stereo(
                    _mainOut[Channel.Left],
                    EchoOutput(Channel.Right)
                    );

                _mainOut = 0;

                if ((Registers[FlgIndex] & 0x40) != 0)
                    echo = 0;

                Out[0] = (short)echo.Left;
                Out[1] = (short)echo.Right;
                Out.Offset += 2;
                if (Out.Offset >= OutEnd)
                {
                    Out = new SharedArray<short>(Extra);
                    OutEnd = ExtraSize;
                }
            }
            private void EchoClock28()
            {
                EchoEnabled = Registers[FlgIndex];
            }

            private void EchoWrite(Channel channel)
            {
                if ((EchoEnabled & 0x20) == 0)
                    SetEcho(channel, _echoOut[channel]);

                _echoOut[channel] = 0;
            }

            private void EchoClock29()
            {
                Esa = Registers[EsaIndex];

                if (EchoOffset == 0)
                    EchoLength = (Registers[EdlIndex] & 0x0F) * 0x800;

                EchoOffset += 4;
                if (EchoOffset >= EchoLength)
                    EchoOffset = 0;

                EchoWrite(Channel.Left);

                EchoEnabled = Registers[FlgIndex];
            }
            private void EchoClock30()
            {
                EchoWrite(Channel.Right);
            }

            private void Phase0()
            {
                Voices[0].VoiceClockV5();
                Voices[1].VoiceClockV2();
            }
            private void Phase1()
            {
                Voices[0].VoiceClockV6();
                Voices[1].VoiceClockV3();
            }
            private void Phase2()
            {
                Voices[0].VoiceClockV7();
                Voices[3].VoiceClockV1();
                Voices[1].VoiceClockV4();
            }
            private void Phase3()
            {
                Voices[0].VoiceClockV8();
                Voices[1].VoiceClockV5();
                Voices[2].VoiceClockV2();
            }
            private void Phase4()
            {
                Voices[0].VoiceClockV9();
                Voices[1].VoiceClockV6();
                Voices[2].VoiceClockV3();
            }
            private void Phase5()
            {
                Voices[1].VoiceClockV7();
                Voices[4].VoiceClockV1();
                Voices[2].VoiceClockV4();
            }
            private void Phase6()
            {
                Voices[1].VoiceClockV8();
                Voices[2].VoiceClockV5();
                Voices[3].VoiceClockV2();
            }
            private void Phase7()
            {
                Voices[1].VoiceClockV9();
                Voices[2].VoiceClockV6();
                Voices[3].VoiceClockV3();
            }
            private void Phase8()
            {
                Voices[2].VoiceClockV7();
                Voices[5].VoiceClockV1();
                Voices[3].VoiceClockV4();
            }
            private void Phase9()
            {
                Voices[2].VoiceClockV8();
                Voices[3].VoiceClockV5();
                Voices[4].VoiceClockV2();
            }
            private void Phase10()
            {
                Voices[2].VoiceClockV9();
                Voices[3].VoiceClockV6();
                Voices[4].VoiceClockV3();
            }
            private void Phase11()
            {
                Voices[3].VoiceClockV7();
                Voices[6].VoiceClockV1();
                Voices[4].VoiceClockV4();
            }
            private void Phase12()
            {
                Voices[3].VoiceClockV8();
                Voices[4].VoiceClockV5();
                Voices[5].VoiceClockV2();
            }
            private void Phase13()
            {
                Voices[3].VoiceClockV9();
                Voices[4].VoiceClockV6();
                Voices[5].VoiceClockV3();
            }
            private void Phase14()
            {
                Voices[4].VoiceClockV7();
                Voices[7].VoiceClockV1();
                Voices[5].VoiceClockV4();
            }
            private void Phase15()
            {
                Voices[4].VoiceClockV8();
                Voices[5].VoiceClockV5();
                Voices[6].VoiceClockV2();
            }
            private void Phase16()
            {
                Voices[4].VoiceClockV9();
                Voices[5].VoiceClockV6();
                Voices[6].VoiceClockV3();
            }
            private void Phase17()
            {
                Voices[5].VoiceClockV7();
                Voices[0].VoiceClockV1();
                Voices[6].VoiceClockV4();
            }
            private void Phase18()
            {
                Voices[5].VoiceClockV8();
                Voices[6].VoiceClockV5();
                Voices[7].VoiceClockV2();
            }
            private void Phase19()
            {
                Voices[5].VoiceClockV9();
                Voices[6].VoiceClockV6();
                Voices[7].VoiceClockV3();
            }
            private void Phase20()
            {
                Voices[6].VoiceClockV7();
                Voices[1].VoiceClockV1();
                Voices[7].VoiceClockV4();
            }
            private void Phase21()
            {
                Voices[6].VoiceClockV8();
                Voices[7].VoiceClockV5();
                Voices[0].VoiceClockV2();
            }
            private void Phase22()
            {
                Voices[7].VoiceClockV3a();
                Voices[5].VoiceClockV9();
                Voices[6].VoiceClockV6();
                EchoClock22();
            }
            private void Phase23()
            {
                Voices[7].VoiceClockV7();
                EchoClock23();
            }
            private void Phase24()
            {
                Voices[7].VoiceClockV8();
                EchoClock24();
            }
            private void Phase25()
            {
                Voices[0].VoiceClockV3b();
                Voices[7].VoiceClockV9();
                EchoClock25();
            }
            private void Phase26()
            {
                EchoClock26();
            }
            private void Phase27()
            {
                MiscClock27();
                EchoClock27();
            }
            private void Phase28()
            {
                MiscClock28();
                EchoClock28();
            }
            private void Phase29()
            {
                MiscClock29();
                EchoClock29();
            }
            private void Phase30()
            {
                MiscClock30();
                Voices[0].VoiceClockV3c();
            }
            private void Phase31()
            {
                Voices[0].VoiceClockV4();
                Voices[2].VoiceClockV1();
            }

            private void InitializePhaseOperations()
            {
                PhaseOperations = new PhaseOperation[0x20]
                {
                    Phase0,
                    Phase1,
                    Phase2,
                    Phase3,
                    Phase4,
                    Phase5,
                    Phase6,
                    Phase7,
                    Phase8,
                    Phase9,
                    Phase10,
                    Phase11,
                    Phase12,
                    Phase13,
                    Phase14,
                    Phase15,
                    Phase16,
                    Phase17,
                    Phase18,
                    Phase19,
                    Phase20,
                    Phase21,
                    Phase22,
                    Phase23,
                    Phase24,
                    Phase25,
                    Phase26,
                    Phase27,
                    Phase28,
                    Phase29,
                    Phase30,
                    Phase31
                };
            }

            public void CopyState(SharedArray<byte> buffer, DspCopyFunction copy)
            {
                var copier = new SpcStateCopier(buffer, copy);

                copier.CopyArray(Registers);

                for (int i = 0; i < VoiceCount; i++)
                {
                    var v = Voices[i];

                    for (int j = 0; j < BrrBufferSize; j++)
                    {
                        var s = v.Buffer[i];
                        s = copier.CopyInt16((short)s);
                        v.Buffer[i] = v.Buffer[i + BrrBufferSize] = s;
                    }

                    v.InterpIndex = copier.CopyUInt16(v.InterpIndex);
                    v.BrrAddress = copier.CopyUInt16(v.BrrAddress);
                    v.Envelope = copier.CopyUInt16(v.Envelope);
                    v.HiddenEnvelope = copier.CopyInt16(v.HiddenEnvelope);
                    v.BufferIndex = copier.CopyByte(v.BufferIndex);
                    v.BrrIndex = copier.CopyByte(v.BrrIndex);
                    v.KOnDelay = copier.CopyByte(v.KOnDelay);
                    {
                        var m = (int)v.EnvelopeMode;
                        m = copier.CopyByte(m);
                        v.EnvelopeMode = (EnvelopeMode)m;
                    }
                    v.EnvXOut = copier.CopyByte(v.EnvXOut);

                    copier.Extra();
                }

                for (int i = 0; i < EchoHistoryIndex; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        var s = EchoHistory[i][j];
                        s = copier.CopyInt16(s);
                        EchoHistory[i][j] = s;
                    }
                }
                EchoHistoryIndex = 0;
                Array.Copy(EchoHistory, 0, EchoHistory, EchoHistorySize, EchoHistorySize);

                EveryOtherSample = copier.CopyBool(EveryOtherSample);
                KOn = copier.CopyByte(KOn);

                Noise = copier.CopyUInt16(Noise);
                Counter = copier.CopyUInt16(Counter);
                EchoOffset = copier.CopyUInt16(EchoOffset);
                EchoLength = copier.CopyUInt16(EchoLength);
                Phase = copier.CopyByte(Phase);

                NewKOn = copier.CopyByte(NewKOn);
                EndXBuf = copier.CopyByte(EndXBuf);
                EnvXBuf = copier.CopyByte(EnvXBuf);
                OutXBuf = copier.CopyByte(OutXBuf);

                Pmon = copier.CopyByte(Pmon);
                Non = copier.CopyByte(Non);
                Eon = copier.CopyByte(Eon);
                Dir = copier.CopyByte(Dir);
                KOff = copier.CopyByte(KOff);

                BrrNextAddress = copier.CopyUInt16(BrrNextAddress);
                Adsr0 = copier.CopyByte(Adsr0);
                BrrHeader = copier.CopyByte(BrrHeader);
                BrrByte = copier.CopyByte(BrrByte);
                Srcn = copier.CopyByte(Srcn);
                Esa = copier.CopyByte(Esa);
                EchoEnabled = copier.CopyByte(EchoEnabled);

                _mainOut[0] = copier.CopyInt16(_mainOut[0]);
                _mainOut[1] = copier.CopyInt16(_mainOut[1]);
                _echoOut[0] = copier.CopyInt16(_echoOut[0]);
                _echoOut[1] = copier.CopyInt16(_echoOut[1]);
                _echoIn[0] = copier.CopyInt16(_echoIn[0]);
                _echoIn[1] = copier.CopyInt16(_echoIn[1]);

                DirAddress = copier.CopyUInt16(DirAddress);
                Pitch = copier.CopyUInt16(Pitch);
                Output = copier.CopyInt16(Output);
                EchoPtr = copier.CopyUInt16(EchoPtr);
                Looped = copier.CopyByte(Looped);

                copier.Extra();
            }
        }
    }
}
