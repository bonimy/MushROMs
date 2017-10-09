namespace SnesXM.Emulator
{
    public interface ISettings
    {
        bool TraceDma
        {
            get;
            set;
        }

        bool TraceHdma
        {
            get;
            set;
        }

        bool TraceVram
        {
            get;
            set;
        }

        bool TraceUnknownRegisters
        {
            get;
            set;
        }

        bool TraceDsp
        {
            get;
            set;
        }

        bool TraceHcEvent
        {
            get;
            set;
        }

        bool SuperFx
        {
            get;
            set;
        }

        byte Dsp
        {
            get;
            set;
        }

        bool Sa1
        {
            get;
            set;
        }

        bool C4
        {
            get;
            set;
        }

        bool Sdd1
        {
            get;
            set;
        }

        bool Spc7110
        {
            get;
            set;
        }

        bool Spc7110Rtc
        {
            get;
            set;
        }

        bool Obc1
        {
            get;
            set;
        }

        byte Seta
        {
            get;
            set;
        }

        bool Srtc
        {
            get;
            set;
        }

        bool Bs
        {
            get;
            set;
        }

        bool BsxItself
        {
            get;
            set;
        }

        bool BsxBootup
        {
            get;
            set;
        }

        bool Msu1
        {
            get;
            set;
        }

        bool MouseMaster
        {
            get;
            set;
        }

        bool SuperScopeMaster
        {
            get;
            set;
        }

        bool JustifierMaster
        {
            get;
            set;
        }

        bool MultiPlayer5Master
        {
            get;
            set;
        }

        bool ForceLoRom
        {
            get;
            set;
        }

        bool ForceHiRom
        {
            get;
            set;
        }

        bool ForceHeader
        {
            get;
            set;
        }

        bool ForceNoHeader
        {
            get;
            set;
        }

        bool ForceInterleaved
        {
            get;
            set;
        }

        bool ForceInterleaved2
        {
            get;
            set;
        }

        bool ForceInterleavedGd24
        {
            get;
            set;
        }

        bool ForceNotInterleaved
        {
            get;
            set;
        }

        bool ForcePal
        {
            get;
            set;
        }

        bool ForceNtsc
        {
            get;
            set;
        }

        bool Pal
        {
            get;
            set;
        }

        int FrameTimePal
        {
            get;
            set;
        }

        int FrameTimeNtsc
        {
            get;
            set;
        }

        int FrameTime
        {
            get;
            set;
        }

        bool SoundSync
        {
            get;
            set;
        }

        bool SixteenBitSound
        {
            get;
            set;
        }

        int SoundPlaybackRate
        {
            get;
            set;
        }

        int SoundInputRate
        {
            get;
            set;
        }

        bool Stereo
        {
            get;
            set;
        }

        bool ReverseStereo
        {
            get;
            set;
        }

        bool Mute
        {
            get;
            set;
        }

        bool SupportHiRes
        {
            get;
            set;
        }

        bool Transparency
        {
            get;
            set;
        }

        byte BgForced
        {
            get;
            set;
        }

        bool DisabledGraphicWindows
        {
            get;
            set;
        }

        bool DisplayFrameRate
        {
            get;
            set;
        }

        bool DisplayWatchedAddresses
        {
            get;
            set;
        }

        bool DisplayPressedKeys
        {
            get;
            set;
        }

        bool DisplayMovieFrame
        {
            get;
            set;
        }

        bool AutoDisplayMessages
        {
            get;
            set;
        }

        int InitialInfoStringTimeout
        {
            get;
            set;
        }

        int DisplayColor
        {
            get;
            set;
        }

        bool Multi
        {
            get;
            set;
        }

        string CartAName
        {
            get;
            set;
        }

        string CartBName
        {
            get;
            set;
        }

        bool DisableGameSpecificHacks
        {
            get;
            set;
        }

        bool BlockInvalidVramAccessMaster
        {
            get;
            set;
        }

        bool BlockInvalidVramAccess
        {
            get;
            set;
        }

        int HmdaTimingHack
        {
            get;
            set;
        }

        int SkipFrames
        {
            get;
            set;
        }

        int TurboSkipFrames
        {
            get;
            set;
        }

        int AutoMaxSkipFrames
        {
            get;
            set;
        }

        bool TurboMode
        {
            get;
            set;
        }

        int HighSpeedSeek
        {
            get;
            set;
        }

        bool FrameAdvance
        {
            get;
            set;
        }

        bool NetPlay
        {
            get;
            set;
        }

        bool NetPlayServer
        {
            get;
            set;
        }

        string ServerName
        {
            get;
            set;
        }

        int Port
        {
            get;
            set;
        }

        bool MovieTruncate
        {
            get;
            set;
        }

        bool MovieNotifyIgnored
        {
            get;
            set;
        }

        bool WrongMovieStateProtection
        {
            get;
            set;
        }

        bool DumpStreams
        {
            get;
            set;
        }

        int DumpStreamsMaxFrames
        {
            get;
            set;
        }

        bool TakeScreenshot
        {
            get;
            set;
        }

        int StretchScreenshot
        {
            get;
            set;
        }

        bool SnapshotScreenshots
        {
            get;
            set;
        }

        bool ApplyCheats
        {
            get;
            set;
        }

        bool NoPatch
        {
            get;
            set;
        }

        int AutoSaveDelay
        {
            get;
            set;
        }

        bool DontSaveOopsSnapshot
        {
            get;
            set;
        }

        bool UpAndDown
        {
            get;
            set;
        }

        bool OpenGlEnable
        {
            get;
            set;
        }
    }
}
