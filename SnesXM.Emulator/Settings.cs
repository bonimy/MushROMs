namespace SnesXM.Emulator
{
    public class Settings : ISettings
    {
        public bool TraceDma
        {
            get;
            set;
        }

        public bool TraceHdma
        {
            get;
            set;
        }

        public bool TraceVram
        {
            get;
            set;
        }

        public bool TraceUnknownRegisters
        {
            get;
            set;
        }

        public bool TraceDsp
        {
            get;
            set;
        }

        public bool TraceHcEvent
        {
            get;
            set;
        }

        public bool SuperFx
        {
            get;
            set;
        }

        public byte Dsp
        {
            get;
            set;
        }

        public bool Sa1
        {
            get;
            set;
        }

        public bool C4
        {
            get;
            set;
        }

        public bool Sdd1
        {
            get;
            set;
        }

        public bool Spc7110
        {
            get;
            set;
        }

        public bool Spc7110Rtc
        {
            get;
            set;
        }

        public bool Obc1
        {
            get;
            set;
        }

        public byte Seta
        {
            get;
            set;
        }

        public bool Srtc
        {
            get;
            set;
        }

        public bool Bs
        {
            get;
            set;
        }

        public bool BsxItself
        {
            get;
            set;
        }

        public bool BsxBootup
        {
            get;
            set;
        }

        public bool Msu1
        {
            get;
            set;
        }

        public bool MouseMaster
        {
            get;
            set;
        }

        public bool SuperScopeMaster
        {
            get;
            set;
        }

        public bool JustifierMaster
        {
            get;
            set;
        }

        public bool MultiPlayer5Master
        {
            get;
            set;
        }

        public bool ForceLoRom
        {
            get;
            set;
        }

        public bool ForceHiRom
        {
            get;
            set;
        }

        public bool ForceHeader
        {
            get;
            set;
        }

        public bool ForceNoHeader
        {
            get;
            set;
        }

        public bool ForceInterleaved
        {
            get;
            set;
        }

        public bool ForceInterleaved2
        {
            get;
            set;
        }

        public bool ForceInterleavedGd24
        {
            get;
            set;
        }

        public bool ForceNotInterleaved
        {
            get;
            set;
        }

        public bool ForcePal
        {
            get;
            set;
        }

        public bool ForceNtsc
        {
            get;
            set;
        }

        public bool Pal
        {
            get;
            set;
        }

        public int FrameTimePal
        {
            get;
            set;
        }

        public int FrameTimeNtsc
        {
            get;
            set;
        }

        public int FrameTime
        {
            get;
            set;
        }

        public bool SoundSync
        {
            get;
            set;
        }

        public bool SixteenBitSound
        {
            get;
            set;
        }

        public int SoundPlaybackRate
        {
            get;
            set;
        }

        public int SoundInputRate
        {
            get;
            set;
        }

        public bool Stereo
        {
            get;
            set;
        }

        public bool ReverseStereo
        {
            get;
            set;
        }

        public bool Mute
        {
            get;
            set;
        }

        public bool SupportHiRes
        {
            get;
            set;
        }

        public bool Transparency
        {
            get;
            set;
        }

        public byte BgForced
        {
            get;
            set;
        }

        public bool DisabledGraphicWindows
        {
            get;
            set;
        }

        public bool DisplayFrameRate
        {
            get;
            set;
        }

        public bool DisplayWatchedAddresses
        {
            get;
            set;
        }

        public bool DisplayPressedKeys
        {
            get;
            set;
        }

        public bool DisplayMovieFrame
        {
            get;
            set;
        }

        public bool AutoDisplayMessages
        {
            get;
            set;
        }

        public int InitialInfoStringTimeout
        {
            get;
            set;
        }

        public int DisplayColor
        {
            get;
            set;
        }

        public bool Multi
        {
            get;
            set;
        }

        public string CartAName
        {
            get;
            set;
        }

        public string CartBName
        {
            get;
            set;
        }

        public bool DisableGameSpecificHacks
        {
            get;
            set;
        }

        public bool BlockInvalidVramAccessMaster
        {
            get;
            set;
        }

        public bool BlockInvalidVramAccess
        {
            get;
            set;
        }

        public int HmdaTimingHack
        {
            get;
            set;
        }

        public int SkipFrames
        {
            get;
            set;
        }

        public int TurboSkipFrames
        {
            get;
            set;
        }

        public int AutoMaxSkipFrames
        {
            get;
            set;
        }

        public bool TurboMode
        {
            get;
            set;
        }

        public int HighSpeedSeek
        {
            get;
            set;
        }

        public bool FrameAdvance
        {
            get;
            set;
        }

        public bool NetPlay
        {
            get;
            set;
        }

        public bool NetPlayServer
        {
            get;
            set;
        }

        public string ServerName
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

        public bool MovieTruncate
        {
            get;
            set;
        }

        public bool MovieNotifyIgnored
        {
            get;
            set;
        }

        public bool WrongMovieStateProtection
        {
            get;
            set;
        }

        public bool DumpStreams
        {
            get;
            set;
        }

        public int DumpStreamsMaxFrames
        {
            get;
            set;
        }

        public bool TakeScreenshot
        {
            get;
            set;
        }

        public int StretchScreenshot
        {
            get;
            set;
        }

        public bool SnapshotScreenshots
        {
            get;
            set;
        }

        public bool ApplyCheats
        {
            get;
            set;
        }

        public bool NoPatch
        {
            get;
            set;
        }

        public int AutoSaveDelay
        {
            get;
            set;
        }

        public bool DontSaveOopsSnapshot
        {
            get;
            set;
        }

        public bool UpAndDown
        {
            get;
            set;
        }

        public bool OpenGlEnable
        {
            get;
            set;
        }
    }
}
