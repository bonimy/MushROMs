using System;

namespace Controls
{
    public class EventArgs<T> : EventArgs
    {
        public T Data
        {
            get;
            set;
        }

        public EventArgs()
        {
        }

        public EventArgs(T data)
        {
            Data = data;
        }
    }
}
