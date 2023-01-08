// ------------------------------------------------------------------------------
// <copyright file="StreamExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

namespace War3Net.Common.Extensions
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

        public static byte[] Copy(this Stream stream, int maxLength)
        {
            _ = stream ?? throw new ArgumentNullException(nameof(stream));

            var output = new byte[maxLength];

            var bytesRead = 0;
            var lengthRemaining = maxLength;
            while (lengthRemaining > 0)
            {
                var read = stream.Read(output, bytesRead, lengthRemaining);
                if (read == 0)
                {
                    break;
                }

                bytesRead += read;
                lengthRemaining -= read;
            }

            if (lengthRemaining > 0)
            {
                Array.Resize(ref output, bytesRead);
            }

            return output;
        }

        public static void CopyTo(this Stream stream, byte[] destination, int offset, int bytesToCopy)
        {
            _ = stream ?? throw new ArgumentNullException(nameof(stream));
            _ = destination ?? throw new ArgumentNullException(nameof(destination));

            if (destination.Length < offset + bytesToCopy)
            {
                throw new ArgumentException("Destination array is too small.", nameof(destination));
            }

            while (bytesToCopy > 0)
            {
                var read = stream.Read(destination, offset, bytesToCopy);
                if (read == 0)
                {
                    throw new ArgumentException("Could not read enough data from the stream.", nameof(stream));
                }

                offset += read;
                bytesToCopy -= read;
            }
        }

        public static ushort ReadWord(this Stream stream)
        {
            var b1 = stream.ReadByte();
            var b2 = stream.ReadByte();

            return (ushort)(b1 | (b2 << 8));
        }

        public static int ReadWordAsInt(this Stream stream)
        {
            var b1 = stream.ReadByte();
            var b2 = stream.ReadByte();

            return b1 | (b2 << 8);
        }
    }
}