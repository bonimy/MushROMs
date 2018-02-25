// <copyright file="AreaObjectData.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Smb1
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Nes;
    using static Helper.ThrowHelper;

    public class AreaObjectData : IEnumerable<AreaObjectCommand>
    {
        /// <summary>
        /// The object command to read that defined the end of the area
        /// object data.
        /// </summary>
        public const byte TerminationCode = 0xFD;

        public AreaObjectData(IReadOnlyList<byte> data, int index)
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

            AreaHeader = new AreaHeader(data[index++], data[index++]);

            AreaObjects = new List<AreaObjectCommand>();
            for (var j = index; j < data.Count;)
            {
                if (data[j] == TerminationCode)
                {
                    return;
                }

                // We expected a two byte object but didn't have enough space.
                if (j + 2 > data.Count)
                {
                    throw new ArgumentException();
                }

                AreaObjects.Add(new AreaObjectCommand(data[j++], data[j++]));
            }

            // We've reached the end of the data array without reading the terminating byte command.
            throw new ArgumentException();
        }

        public int Count
        {
            get
            {
                return AreaObjects.Count;
            }
        }

        public AreaHeader AreaHeader
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the size, in bytes, of this <see cref="AreaObjectData"/>. This
        /// size includes the <see cref="AreaHeader"/> and the termination code.
        /// </summary>
        public int DataSize
        {
            get
            {
                // We add one byte for the termination code.
                var result = 1;

                // We add two bytes for the area header.
                result += AreaHeader.SizeOf;

                foreach (var obj in AreaObjects)
                {
                    result += AreaObjectCommand.Size;
                }

                return result;
            }
        }

        private IList<AreaObjectCommand> AreaObjects
        {
            get;
        }

        public AreaObjectCommand this[int index]
        {
            get
            {
                return AreaObjects[index];
            }

            set
            {
                AreaObjects[index] = value;
            }
        }

        public IEnumerator<AreaObjectCommand> GetEnumerator()
        {
            return AreaObjects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
