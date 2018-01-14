// <copyright file="CompressCommand.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

namespace Snes
{
    public enum CompressCommand
    {
        DirectCopy = 0,
        RepeatedByte = 1,
        RepeatedWord = 2,
        IncrementingByte = 3,
        CopySection = 4,
        LongCommand = 7
    }
}
