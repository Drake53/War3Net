// ------------------------------------------------------------------------------
// <copyright file="StreamExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.IO.Mpq
{
    public static class StreamExtensions
    {
        private const int _DefaultCopyBufferSize = 81920;

        public static void CopyTo(this Stream stream, Stream destination, int bytesToCopy, int bufferSize = _DefaultCopyBufferSize)
        {
            var buffer = new byte[bufferSize];
            while (bytesToCopy > 0)
            {
                var toRead = bytesToCopy > bufferSize ? bufferSize : bytesToCopy;
                var read = stream.Read(buffer, 0, toRead);
                if (read != 0)
                {
                    destination.Write(buffer, 0, read);
                    bytesToCopy -= read;
                }
            }
        }
    }
}