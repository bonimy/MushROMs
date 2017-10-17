namespace SnesXM.Emulator
{
    public interface IInternalCpu
    {
        Opcode this[int index] { get; }

        byte[] GetOpcodeLengths();

        void SetOpcodeLengths(byte[] opcodeLengths);

        byte Carry { get; set; }
        byte Zero { get; set; }
        byte Negative { get; set; }
        byte Overflow { get; set; }

        int ShiftedPb { get; set; }
        int ShiftedDb { get; set; }
        int Frame { get; set; }
        int FrameAdvanceCount { get; set; }

        void SetCarry();

        void ClearCarry();

        bool CheckCarry();

        void SetZero();

        void ClearZero();

        bool CheckZero();

        void SetOverflow();

        void ClearOverflow();

        bool CheckOverflow();

        void SetNegative();

        void ClearNegative();

        void CheckNegative();
    }
}
