// ------------------------------------------------------------------------------
// <copyright file="TextureLoader.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using Veldrid;

using War3Net.Drawing.Blp;
using War3Net.Drawing.Tga;

namespace War3Net.Rendering
{
    public static class TextureLoader
    {
        public static Texture GetDummyTexture(
            GraphicsDevice graphicsDevice,
            ResourceFactory resourceFactory,
            Veldrid.PixelFormat pixelFormat = Veldrid.PixelFormat.B8_G8_R8_A8_UNorm)
        {
            var width = 1U;
            var height = 1U;

            var sampledTexture = resourceFactory.CreateTexture(TextureDescription.Texture2D(width, height, 1, 1, pixelFormat, TextureUsage.Sampled));
            var stagingTexture = resourceFactory.CreateTexture(TextureDescription.Texture2D(width, height, 1, 1, pixelFormat, TextureUsage.Staging));

            var pixelData = new byte[] { 3, 3, 255, 255 };
            graphicsDevice.UpdateTexture(stagingTexture, pixelData, 0, 0, 0, width, height, 1, 0, 0);

            CreateCommandList(graphicsDevice, resourceFactory, stagingTexture, sampledTexture);

            return sampledTexture;
        }

        public static Texture LoadTexture(
            GraphicsDevice graphicsDevice,
            ResourceFactory resourceFactory,
            Stream stream,
            Veldrid.PixelFormat pixelFormat = Veldrid.PixelFormat.B8_G8_R8_A8_UNorm)
        {
            FileFormatVersion blpVersion;
            var oldPosition = stream.Position;
            using (var reader = new BinaryReader(stream, Encoding.ASCII, true))
            {
                blpVersion = (FileFormatVersion)reader.ReadUInt32();
            }

            stream.Position = oldPosition;
            return Enum.IsDefined(typeof(FileFormatVersion), blpVersion)
                ? LoadBlpTexture(graphicsDevice, resourceFactory, stream, pixelFormat)
                : LoadTgaTexture(graphicsDevice, resourceFactory, stream, pixelFormat);
        }

        public static Texture LoadBlpTexture(
            GraphicsDevice graphicsDevice,
            ResourceFactory resourceFactory,
            Stream stream,
            Veldrid.PixelFormat pixelFormat = Veldrid.PixelFormat.B8_G8_R8_A8_UNorm)
        {
            using var blpFile = new BlpFile(stream);

            var width = (uint)blpFile.Width;
            var height = (uint)blpFile.Height;

            var mipMapCount = (uint)blpFile.MipMapCount;

            var sampledTexture = resourceFactory.CreateTexture(TextureDescription.Texture2D(width, height, mipMapCount, 1, pixelFormat, TextureUsage.Sampled));
            var stagingTexture = resourceFactory.CreateTexture(TextureDescription.Texture2D(width, height, mipMapCount, 1, pixelFormat, TextureUsage.Staging));

            var mipMapWidths = new uint[mipMapCount];
            var mipMapHeights = new uint[mipMapCount];
            for (var mipMapLevel = 0; mipMapLevel < mipMapCount; mipMapLevel++)
            {
                var pixelData = blpFile.GetPixels(mipMapLevel, out var w, out var h);
                var mipMapWidth = (uint)w;
                var mipMapHeight = (uint)h;

                mipMapWidths[mipMapLevel] = mipMapWidth;
                mipMapHeights[mipMapLevel] = mipMapHeight;
                graphicsDevice.UpdateTexture(stagingTexture, pixelData, 0, 0, 0, mipMapWidth, mipMapHeight, 1, (uint)mipMapLevel, 0);
            }

            CreateCommandList(graphicsDevice, resourceFactory, stagingTexture, sampledTexture);

            return sampledTexture;
        }

        public static Texture LoadTgaTexture(
            GraphicsDevice graphicsDevice,
            ResourceFactory resourceFactory,
            Stream stream,
            Veldrid.PixelFormat pixelFormat = Veldrid.PixelFormat.B8_G8_R8_A8_UNorm)
        {
            using var bitmap = new TgaImage(stream, true).GetBitmap();

            var width = (uint)bitmap.Width;
            var height = (uint)bitmap.Height;

            var sampledTexture = resourceFactory.CreateTexture(TextureDescription.Texture2D(width, height, 1, 1, pixelFormat, TextureUsage.Sampled));
            var stagingTexture = resourceFactory.CreateTexture(TextureDescription.Texture2D(width, height, 1, 1, pixelFormat, TextureUsage.Staging));

            var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var byteCount = bitmapData.Stride * bitmap.Height;
            var pixelData = new byte[byteCount];
            var pointer = bitmapData.Scan0;
            Marshal.Copy(pointer, pixelData, 0, byteCount);

            graphicsDevice.UpdateTexture(stagingTexture, pixelData, 0, 0, 0, width, height, 1, 0, 0);

            CreateCommandList(graphicsDevice, resourceFactory, stagingTexture, sampledTexture);

            return sampledTexture;
        }

        private static void CreateCommandList(
            GraphicsDevice graphicsDevice,
            ResourceFactory resourceFactory,
            Texture stagingTexture,
            Texture sampledTexture)
        {
            var commandList = resourceFactory.CreateCommandList();
            commandList.Begin();
            commandList.CopyTexture(stagingTexture, sampledTexture);
            commandList.End();
            graphicsDevice.SubmitCommands(commandList);

            graphicsDevice.DisposeWhenIdle(commandList);
            graphicsDevice.DisposeWhenIdle(stagingTexture);
        }
    }
}