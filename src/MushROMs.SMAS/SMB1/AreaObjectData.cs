using System;
using System.Collections.Generic;
using MushROMs.SMB1;
using AreaHeader = MushROMs.SMB1.AreaHeader;
using NesAreaObjectData = MushROMs.SMB1.AreaObjectData;

namespace MushROMs.SMAS.SMB1
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

        public int AreaObjectCount => AreaObjects.Count;

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
                    result += obj.Size;
                return result;
            }
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
            for (var j = index; j < data.Length;)
            {
                if (data[j] == TerminationCode)
                    return;

                // Determine if this is a three byte object
                if ((data[j] & 0x0F) == 0x0F)
                {
                    // We expected a three byte object but didn't have enough space.
                    if (data.Length < j + 3)
                        throw new ArgumentException();

                    AreaObjects.Add(new AreaObjectCommand(data[j++], data[j++], data[j++]));
                }
                else
                {
                    // We expected a two byte object but didn't have enough space.
                    if (data.Length < j + 2)
                        throw new ArgumentException();

                    AreaObjects.Add(new AreaObjectCommand(data[j++], data[j++]));
                }
            }

            // We've reached the end of the data array without reading the terminating byte command.
            throw new ArgumentException();
        }

        public AreaObjectData(NesAreaObjectData nes, AreaType areaType)
        {
            if (nes == null)
                throw new ArgumentNullException(nameof(nes));

            AreaHeader = nes.AreaHeader;
            var lastTerrain = AreaHeader.TerrainMode;

            AreaObjects = new List<AreaObjectCommand>();

            var flagPole = false;
            var page = 0;
            var x = 0;
            var xPage = 0;
            var CastleLedge = areaType == AreaType.Castle;

            for (var i = 0; i < nes.AreaObjectCount; i++)
            {
                var obj = (AreaObjectCommand)nes[i];
                var type = obj.AreaObjectCode;

                if (CastleLedge)
                {
                    if (type == AreaObjectCode.HorizontalBlocks)
                    {
                        // Turn horizontal bricks into castle-specific stairs
                        obj.Command = 3;
                        obj.ExtendedCommand = 2;
                    }
                    else
                        CastleLedge = false;
                }

                // Empty objects are worthless and can crash the game.
                if (obj.IsEmpty)
                    continue;

                // SNES ignores background changes
                if (type == AreaObjectCode.BackgroundChange)
                    continue;

                // Kepe up with the current page
                if (obj.PageFlag)
                    page++;

                // Check for flag pole object
                if (!flagPole && (type == AreaObjectCode.FlagPole || type == AreaObjectCode.AltFlagPole))
                {
                    flagPole = true;
                    x = obj.X;
                    xPage = (page << 4) | x;
                }

                // For any castle objects (after the flag pole),
                // set as just one of two states, and adjust X-coordinate
                if (flagPole && type == AreaObjectCode.Castle)
                {
                    var x2 = obj.X;
                    var x2Page = (page << 4) | x2;
                    var dif = x2Page - xPage;
                    if (dif < 0x10)
                    {
                        obj.X = x + 4;
                        obj.PageFlag = x + 4 >= 0x10;
                    }

                    flagPole = false;
                }

                if (type == AreaObjectCode.Castle)
                {
                    if (obj.Parameter != 0) // Big castle
                    {
                        obj.Parameter = 6; // Small castle
                    }
                    else if (page != 0)
                    {
                        obj.X -= 2;
                        obj.PageFlag = x + 2 >= 0x10;
                    }
                }

                AreaObjects.Add(obj);

                if (type == AreaObjectCode.BrickAndSceneryChange && areaType == AreaType.Castle)
                {
                    var current = (TerrainMode)(obj.Parameter & 0x0F);
                    InsertCastleTiles(AreaObjects, current, lastTerrain);
                    lastTerrain = current;
                }
            }
            AreaObjects.Add(new AreaObjectCommand(0x7D, 0xC7));
        }

        private void InsertCastleTiles(IList<AreaObjectCommand> objects, TerrainMode terrain, TerrainMode last)
        {
            switch (terrain)
            {
            case TerrainMode.None:
                return;
            case TerrainMode.Ceiling0Floor2:
                return;
            case TerrainMode.Ceiling1Floor2:
                return;
            case TerrainMode.Ceiling3Floor2:
                return;
            case TerrainMode.Ceiling4Floor2:
                return;
            case TerrainMode.Ceiling8Floor2:
                return;
            case TerrainMode.Ceiling1Floor5:
                return;
            case TerrainMode.Ceiling3Floor5:
                return;
            case TerrainMode.Ceiling4Floor5:
                return;
            case TerrainMode.Ceiling1Floor6:
                return;
            case TerrainMode.Ceiling1Floor0:
                return;
            case TerrainMode.Ceiling4Floor6:
                return;
            case TerrainMode.Ceiling1Floor9:
                return;
            case TerrainMode.Ceiling1Middle5Floor2:
                return;
            case TerrainMode.Ceiling1Middle4Floor2:
                return;
            case TerrainMode.Solid:
                return;
            }
        }

        public IEnumerator<AreaObjectCommand> GetEnumerator() =>
            AreaObjects.GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
