namespace MushROMs.SMB1
{
    /// <summary>
    /// The header data for the current area.
    /// </summary>
    /// <remarks>
    /// An area header defines how the current area should look when first
    /// entering it. It defines certain properties like start time, position,
    /// scenery, and terrain. Certain objects in the area can modify these properties
    /// too, but every area object pointer starts with two bytes that deterime the
    /// header.
    /// </remarks>
    public struct AreaHeader
    {
        /// <summary>
        /// Gets or sets the first byte of the area data.
        /// </summary>
        public byte Value1
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the second byte of the area data.
        /// </summary>
        public byte Value2
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the players start time when entering the area.
        /// </summary>
        public StartTime StartTime
        {
            get => (StartTime)(Value1 >> 6);
            set
            {
                Value1 &= unchecked((byte)~(3 << 6));
                Value1 |= (byte)(((int)value & 3) << 6);
            }
        }

        /// <summary>
        /// Gets or sets the players start Y-position when entering the area.
        /// </summary>
        public StartYPosition StartYPosition
        {
            get => (StartYPosition)((Value1 >> 3) & 7);
            set
            {
                Value1 &= unchecked((byte)~(7 << 3));
                Value1 |= (byte)(((int)value & 7) << 3);
            }
        }

        /// <summary>
        /// Gets or sets the area background type to use at the start of the area.
        /// </summary>
        public BackgroundType BackgroundType
        {
            get => (BackgroundType)(Value1 & 7);
            set
            {
                Value1 &= unchecked((byte)~7);
                Value1 |= (byte)((int)value & 7);
            }
        }

        /// <summary>
        /// Gets or sets the miscellaneous platform type to use in the area.
        /// </summary>
        public MiscPlatformType ObjectMode
        {
            get => (MiscPlatformType)(Value2 >> 5);
            set
            {
                Value2 &= unchecked((byte)~(7 << 5));
                Value2 |= (byte)(((int)value & 7) << 5);
            }
        }

        /// <summary>
        /// Gets or sets the layer 1 scenery to draw at the start of the area.
        /// </summary>
        public SceneryType SceneryType
        {
            get => (SceneryType)((Value2 >> 4) & 3);
            set
            {
                Value2 &= unchecked((byte)~(3 << 4));
                Value2 |= (byte)(((int)value & 3) << 4);
            }
        }

        /// <summary>
        /// Gets or sets the terrain layout to use when starting the area.
        /// </summary>
        public TerrainMode TerrainMode
        {
            get => (TerrainMode)(Value2 & 0x0F);
            set
            {
                Value2 &= unchecked((byte)~0x0F);
                Value2 |= (byte)((int)value & 0x0F);
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="AreaHeader"/> struct.
        /// </summary>
        /// <param name="val1">
        /// The first byte of the area header.
        /// </param>
        /// <param name="val2">
        /// The second byte of the area header.
        /// </param>
        public AreaHeader(byte val1, byte val2)
        {
            Value1 = val1;
            Value2 = val2;
        }

        public AreaHeader(StartTime startTime, StartYPosition startYPosition, BackgroundType backgroundType, MiscPlatformType objectMode, SceneryType sceneryType, TerrainMode terrainMode) : this()
        {
            StartTime = startTime;
            StartYPosition = startYPosition;
            BackgroundType = backgroundType;
            ObjectMode = objectMode;
            SceneryType = sceneryType;
            TerrainMode = terrainMode;
        }

        public static bool operator ==(AreaHeader left, AreaHeader right)
        {
            return left.Value1 == right.Value1 &&
                left.Value2 == right.Value2;
        }
        public static bool operator !=(AreaHeader left, AreaHeader right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is AreaHeader))
                return false;
            return (AreaHeader)obj == this;
        }

        public override int GetHashCode()
        {
            return (Value1) | (Value2 << 8);
        }

        public override string ToString()
        {
            return System.String.Format("{0} {1}", Value1.ToString("X2"), Value2.ToString("X2"));
        }
    }
}
