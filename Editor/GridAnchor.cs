namespace EasyGrid
{
    /// <summary>
    /// This is just the normal text anchor that you now from Text UI, but with MiddleCenter as the default
    /// </summary>
    public enum GridAnchor
    {
        /// <summary>
        ///   <para>Text is centered both horizontally and vertically.</para>
        /// </summary>
        MiddleCenter = 0,
        /// <summary>
        ///   <para>Text is anchored in upper left corner.</para>
        /// </summary>
        UpperLeft = 1,
        /// <summary>
        ///   <para>Text is anchored in upper side, centered horizontally.</para>
        /// </summary>
        UpperCenter = 2,
        /// <summary>
        ///   <para>Text is anchored in upper right corner.</para>
        /// </summary>
        UpperRight = 3,
        /// <summary>
        ///   <para>Text is anchored in left side, centered vertically.</para>
        /// </summary>
        MiddleLeft = 4,
        /// <summary>
        ///   <para>Text is anchored in right side, centered vertically.</para>
        /// </summary>
        MiddleRight = 5,
        /// <summary>
        ///   <para>Text is anchored in lower left corner.</para>
        /// </summary>
        LowerLeft = 6,
        /// <summary>
        ///   <para>Text is anchored in lower side, centered horizontally.</para>
        /// </summary>
        LowerCenter = 7,
        /// <summary>
        ///   <para>Text is anchored in lower right corner.</para>
        /// </summary>
        LowerRight = 8,
    }
}