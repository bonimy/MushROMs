using System;
using System.Collections.Generic;
using System.Text;

namespace MushROMs.SMAS.SMB1
{
    public interface ILevelReader
    {
        byte[] GetLevelObjectData();
    }
}
