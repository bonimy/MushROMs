// <copyright file="AreaSpriteData.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Smb1
{
    using System;
    using System.Collections.Generic;
    using Nes;
    using static Helper.ThrowHelper;

    public class AreaSpriteData : IEnumerable<AreaSpriteCommand>
    {
        public const byte TerminationCode = 0xFF;

        public AreaSpriteData(IReadOnlyList<byte> data, int index)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (index < 0)
            {
                throw ValueNotGreaterThanEqualTo(
                    nameof(index),
                    index);
            }

            AreaSprites = new List<AreaSpriteCommand>();
            for (var j = index; j < data.Count;)
            {
                if (data[j] == TerminationCode)
                {
                    return;
                }

                // Determine if this is a three byte object
                if ((data[j] & 0x0F) == 0x0E)
                {
                    // We expected a three byte object but didn't have enough space.
                    if (j + 3 > data.Count)
                    {
                        throw new ArgumentException();
                    }

                    AreaSprites.Add(
                        new AreaSpriteCommand(data[j++], data[j++], data[j++]));
                }
                else
                {
                    // We expected a two byte object but didn't have enough space.
                    if (j + 2 > data.Count)
                    {
                        throw new ArgumentException();
                    }

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

        public int AreaSpriteCount
        {
            get
            {
                return AreaSprites.Count;
            }
        }

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
                {
                    result += obj.Size;
                }

                return result;
            }
        }

        private IList<AreaSpriteCommand> AreaSprites
        {
            get;
        }

        public AreaSpriteCommand this[int index]
        {
            get
            {
                return AreaSprites[index];
            }

            set
            {
                AreaSprites[index] = value;
            }
        }

        public IEnumerator<AreaSpriteCommand> GetEnumerator()
        {
            return AreaSprites.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
