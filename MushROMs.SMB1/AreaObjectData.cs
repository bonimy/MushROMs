using System;
using System.Collections.Generic;

namespace MushROMs.SMB1
{
    public class AreaObjectData : IEnumerable<AreaObjectCommand>
    {
        /// <summary>
        /// The object command to read that defined the end of the area
        /// object data.
        /// </summary>
        public const byte TerminationCode = 0xFD;

        public AreaHeader AreaHeader
        {
            get;
            set;
        }

        private IList<AreaObjectCommand> AreaObjects
        {
            get;
            set;
        }

        public AreaObjectCommand this[int index]
        {
            get => AreaObjects[index];
            set => AreaObjects[index] = value;
        }

        public AreaObjectData(byte[] data, int index)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            AreaHeader = new AreaHeader(data[index++], data[index++]);

            AreaObjects = new List<AreaObjectCommand>();
            for (var j = index; j < data.Length; )
            {
                if (data[j] == TerminationCode)
                    return;

                // We expected a two byte object but didn't have enough space.
                if (data.Length < j + 2)
                    throw new ArgumentException();

                AreaObjects.Add(new AreaObjectCommand(data[j++], data[j++]));
            }

            // We've reached the end of the data array without reading the terminating byte command.
            throw new ArgumentException();
        }

        public IEnumerator<AreaObjectCommand> GetEnumerator() =>
            AreaObjects.GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
