using System;
using System.Collections.Generic;
using System.Text;

namespace SnesXM.Emulator
{
    public interface IGui
    {
        int hFrameTimer { get; set; }
        int hHotkeyTimer { get; set; }

        byte Language { get; set; }

        //Graphic Settings
        WindowMode FullscreenMode { get; set; }
        RenderFilter Scale { get; set; }
        RenderFilter ScaleHiRes { get; set; }
        bool BlendHiRes { get; set; }
        bool AVIHiRes { get; set; }
        bool DoubleBuffered { get; set; }
        bool FullScreen { get; set; }
        bool Stretch { get; set; }
        bool HeightExtend { get; set; }
        bool AspectRatio { get; set; }
        OutputMethod outputMethod { get; set; }
        int AspectWidth { get; set; }
        bool AlwaysCenterImage { get; set; }
        bool EmulateFullscreen { get; set; }
        bool EmulatedFullscreen { get; set; }
        bool BilinearFilter { get; set; }
        bool LocalVidMem { get; set; }
        bool Vsync { get; set; }
        bool shaderEnabled { get; set; }
        string D3DshaderFileName { get; set; }
        string OGLshaderFileName { get; set; }

        bool OGLdisablePBOs { get; set; }

        bool IgnoreNextMouseMove { get; set; }
        //RECT window_size{ get; set; }
        bool window_maximized { get; set; }
        int MouseX { get; set; }
        int MouseY { get; set; }
        int MouseButtons { get; set; }
        int superscope_turbo { get; set; }
        int superscope_pause { get; set; }
        int FrameAdvanceJustPressed { get; set; }
        //HCURSOR Blank{ get; set; }
        //HCURSOR GunSight{ get; set; }
        //HCURSOR Arrow{ get; set; }
        int CursorTimer { get; set; }
        //HDC hDC{ get; set; }
        //HACCEL Accelerators{ get; set; }
        bool NeedDepthConvert { get; set; }
        bool DepthConverted { get; set; }

        bool InactivePause { get; set; }
        bool CustomRomOpen { get; set; }
        bool FASkipsNonInput { get; set; }
        bool FAMute { get; set; }
        int ScreenDepth { get; set; }
        int RedShift { get; set; }
        int GreenShift { get; set; }
        int BlueShift { get; set; }
        int ControlForced { get; set; }
        int CurrentSaveSlot { get; set; }
        int MaxRecentGames { get; set; }
        int ControllerOption { get; set; }
        int ValidControllerOptions { get; set; }
        int SoundChannelEnable { get; set; }
        bool BackgroundInput { get; set; }
        bool JoystickHotkeys { get; set; }
        bool MovieClearSRAM { get; set; }
        bool MovieStartFromReset { get; set; }
        bool MovieReadOnly { get; set; }
        bool NetplayUseJoypad1 { get; set; }
        int FlipCounter { get; set; }
        int NumFlipFrames { get; set; }

        int SoundDriver { get; set; }
        int SoundBufferSize { get; set; }
        bool Mute { get; set; }
        // used for sync sound synchronization
        //CRITICAL_SECTION SoundCritSect{ get; set; }
        //HANDLE SoundSyncEvent{ get; set; }

        string RomDir { get; set; }
        string ScreensDir { get; set; }
        string MovieDir { get; set; }
        string SPCDir { get; set; }
        string FreezeFileDir { get; set; }
        string SRAMFileDir { get; set; }
        string PatchDir { get; set; }
        string CheatDir { get; set; }
        string BiosDir { get; set; }
        string SatDir { get; set; }
        bool LockDirectories { get; set; }

        string[] RecentGames { get; set; }
        string[] RecentHostNames { get; set; }

        //turbo switches -- SNES-wide
        ushort TurboMask { get; set; }
        //COLORREF InfoColor{ get; set; }
        bool HideMenu { get; set; }

        // avi writing
        //struct AVIFile* AVIOut{ get; set; }

        long FrameCount { get; set; }
        long LastFrameCount { get; set; }
        int IdleCount { get; set; }

        // rewinding
        bool rewinding { get; set; }
        int rewindBufferSize { get; set; }
        int rewindGranularity { get; set; }
    }
}
