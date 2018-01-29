// <copyright file="IGfxSelection.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

namespace Snes
{
    public interface IGfxSelection : IDataSelection
    {
        GraphicsFormat GraphicsFormat
        {
            get;
        }

        int BytesPerTile
        {
            get;
        }

        IGfxSelection Move(int startAddress);
    }
}
