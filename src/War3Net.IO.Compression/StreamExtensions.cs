// ------------------------------------------------------------------------------
// <copyright file="StreamExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

namespace War3Net.IO.Compression
{
    /// <summary>
    /// Provides extension methods for the <see cref="Stream"/> class.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// The default buffer size.
        /// </summary>
        public const int DefaultBufferSize = 81920;

        /// <summary>
        /// Reads the bytes from the current stream and writes them to another stream.
        /// </summary>
        /// <param name="stream">The stream from which to copy.</param>
        /// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
        /// <param name="bytesToCopy">The amount of bytes to copy.</param>
        /// <param name="bufferSize">The size of the buffer. This value must be greater than zero.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> and/or the <paramref name="destination"/> are <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="bufferSize"/> is less than zero.</exception>
        public static void CopyTo(this Stream stream, Stream destination, int bytesToCopy, int bufferSize)
        {
            _ = stream ?? throw new ArgumentNullException(nameof(stream));
            _ = destination ?? throw new ArgumentNullException(nameof(destination));

            var buffer = new byte[bufferSize];
            while (bytesToCopy > 0)
            {
                var toRead = bytesToCopy > bufferSize ? bufferSize : bytesToCopy;
                var read = stream.Read(buffer, 0, toRead);
                if (read == 0)
                {
                    break;
                }

                destination.Write(buffer, 0, read);
                bytesToCopy -= read;
            }
        }

        /// <summary>
        /// Reads the bytes from the current stream and writes them to another stream.
        /// </summary>
        /// <param name="stream">The stream from which to copy.</param>
        /// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
        /// <param name="copyOffset">The offset in the <paramref name="stream"/> from where to start copying.</param>
        /// <param name="bytesToCopy">The amount of bytes to copy.</param>
        /// <param name="bufferSize">The size of the buffer. This value must be greater than zero.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> and/or the <paramref name="destination"/> are <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="bufferSize"/> is less than zero.</exception>
        public static void CopyTo(this Stream stream, Stream destination, long copyOffset, int bytesToCopy, int bufferSize)
        {
            _ = stream ?? throw new ArgumentNullException(nameof(stream));

            stream.Position = copyOffset;
            stream.CopyTo(destination, bytesToCopy, bufferSize);
        }
    }
}