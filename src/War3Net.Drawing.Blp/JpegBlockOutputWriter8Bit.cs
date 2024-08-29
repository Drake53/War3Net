// ------------------------------------------------------------------------------
// <copyright file="JpegBlockOutputWriter8Bit.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#if !WINDOWS
using System;
using System.Runtime.CompilerServices;

using JpegLibrary;

namespace War3Net.Drawing.Blp
{
    internal sealed class JpegBlockOutputWriter8Bit : JpegBlockOutputWriter
    {
        private readonly int _width;
        private readonly int _height;
        private readonly int _numberOfComponents;
        private readonly Memory<byte> _destination;

        private JpegBlockOutputWriter8Bit(int width, int height, int numberOfComponents, Memory<byte> destination)
        {
            _width = width;
            _height = height;
            _numberOfComponents = numberOfComponents;
            _destination = destination;
        }

        public static JpegBlockOutputWriter8Bit Create(JpegDecoder decoder, out Memory<byte> destination)
        {
            destination = new byte[decoder.Width * decoder.Height * decoder.NumberOfComponents];
            return new JpegBlockOutputWriter8Bit(decoder.Width, decoder.Height, decoder.NumberOfComponents, destination);
        }

        public override void WriteBlock(ref short blockRef, int componentIndex, int x, int y)
        {
            if (x > _width || y > _height)
            {
                throw new InvalidOperationException();
            }

            var writeWidth = Math.Min(_width - x, 8);
            var writeHeight = Math.Min(_height - y, 8);

            var span = _destination.Span;

            var destinationOffsetBase = (y * _width * _numberOfComponents) + (x * _numberOfComponents) + componentIndex;
            for (var destinationY = 0; destinationY < writeHeight; destinationY++)
            {
                var destinationOffset = destinationOffsetBase + (destinationY * _width * _numberOfComponents);
                for (var destinationX = 0; destinationX < writeWidth; destinationX++)
                {
                    span[destinationOffset] = ClampToByte(Unsafe.Add(ref blockRef, destinationX));

                    destinationOffset += _numberOfComponents;
                }

                blockRef = ref Unsafe.Add(ref blockRef, 8);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ClampToByte(short input)
        {
#if NETSTANDARD2_0
            return (byte)Math.Max((short)0, Math.Min((short)255, input));
#else
            return (byte)Math.Clamp(input, (short)0, (short)255);
#endif
        }
    }
}
#endif