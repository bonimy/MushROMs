using System;
using System.Collections.Generic;

namespace MushROMs.SMB1
{
    public class AreaSpriteData : IEnumerable<AreaSpriteCommand>
    {
        public const byte TerminationCode = 0xFF;

        private IList<AreaSpriteCommand> AreaSprites
        {
            get;
            set;
        }

        public int AreaSpriteCount => AreaSprites.Count;

        /// <summary>
        /// Gets the size, in bytes, of this <see cref="AreaSpriteData"/>. This
        /// size includes the termination code.
        /// </summary>
        public int DataSize
        {
            get
            {
                // We add one byte for the termination code.
                var result = 1;
                foreach (var obj in AreaSprites)
                    result += obj.Size;
                return result;
            }
        }

        public AreaSpriteCommand this[int index]
        {
            get => AreaSprites[index];
            set => AreaSprites[index] = value;
        }

        public AreaSpriteData(byte[] data, int index)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            AreaSprites = new List<AreaSpriteCommand>();
            for (var j = index; j < data.Length; )
            {
                if (data[j] == TerminationCode)
                    return;

                // Determine if this is a three byte object
                if ((data[j] & 0x0F) == 0x0E)
                {
                    // We expected a three byte object but didn't have enough space.
                    if (data.Length < j + 3)
                        throw new ArgumentException();

                    AreaSprites.Add(
                        new AreaSpriteCommand(data[j++], data[j++], data[j++]));
                }
                else
                {
                    // We expected a two byte object but didn't have enough space.
                    if (data.Length < j + 2)
                        throw new ArgumentException();

                    AreaSprites.Add(
                        new AreaSpriteCommand(data[j++], data[j++]));
                }
            }

            // If we made it here, then we reached the end of the data without a termination code.
            throw new ArgumentException();
        }

        public AreaSpriteData(AreaSpriteData areaSpriteData)
        {
            AreaSprites = new List<AreaSpriteCommand>(areaSpriteData);
        }

        public IEnumerator<AreaSpriteCommand> GetEnumerator() =>
            AreaSprites.GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
