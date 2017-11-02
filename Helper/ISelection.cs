using System;
using System.Collections.Generic;
using System.Text;

namespace Helper
{
    public interface ISelection<T> : IReadOnlyList<T>
    {
        bool Contains(T value);

        ISelection<T> Copy();
    }
}
