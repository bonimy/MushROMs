namespace MushROMs.SMB1
{
    /// <summary>
    /// The terrain and background type of the current area.
    /// </summary>
    public enum AreaType
    {
        /// <summary>
        /// An underwater areas where the player is swimming.
        /// </summary>
        Water,
        /// <summary>
        /// An above gorund area. This includes sky areas too with cloud-ground.
        /// </summary>
        Grassland,
        /// <summary>
        /// An underground area.
        /// </summary>
        Underground,
        /// <summary>
        /// A castle area.
        /// </summary>
        Castle
    }
}
