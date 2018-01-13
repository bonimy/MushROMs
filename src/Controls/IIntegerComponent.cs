using System;

namespace Controls
{
    public interface IIntegerComponent
    {
        event EventHandler ValueChanged;

        int Value { get; set; }
    }
}
