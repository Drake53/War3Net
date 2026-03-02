namespace War3Net.Drawing.Blp
{
    /// <summary>
    /// Indicates the file format of the image(s) embedded in the <see cref="BlpFile"/>.
    /// </summary>
    internal enum FileContent
    {
        /// <summary>
        /// JPEG Compression (JFIF formatted).
        /// </summary>
        JPG = 0,

        /// <summary>
        /// DirectX Compression or Uncompressed (Palettized).
        /// </summary>
        Direct = 1,
    }
}