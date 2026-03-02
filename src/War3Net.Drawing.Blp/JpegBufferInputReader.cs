namespace War3Net.Drawing.Blp
{
    /// <summary>
    /// Input reader for JPEG encoding from a byte buffer.
    /// </summary>
    internal sealed class JpegBufferInputReader : JpegBlockInputReader
    {
        private readonly int _width;
        private readonly int _height;
        private readonly int _componentCount;
        private readonly Memory<byte> _buffer;

        public JpegBufferInputReader(int width, int height, int componentCount, Memory<byte> buffer)
        {
            _width = width;
            _height = height;
            _componentCount = componentCount;
            _buffer = buffer;
        }

        public override int Width => _width;

        public override int Height => _height;

        public override void ReadBlock(ref short blockRef, int componentIndex, int x, int y)
        {
            var width = _width;
            var componentCount = _componentCount;

            ref var sourceRef = ref MemoryMarshal.GetReference(MemoryMarshal.AsBytes(_buffer.Span));

            var blockWidth = Math.Min(width - x, 8);
            var blockHeight = Math.Min(_height - y, 8);

            if (blockWidth != 8 || blockHeight != 8)
            {
                Unsafe.As<short, JpegBlock8x8>(ref blockRef) = default;
            }

            for (var offsetY = 0; offsetY < blockHeight; offsetY++)
            {
                var sourceRowOffset = ((y + offsetY) * width) + x;
                ref var destinationRowRef = ref Unsafe.Add(ref blockRef, offsetY * 8);
                for (var offsetX = 0; offsetX < blockWidth; offsetX++)
                {
                    Unsafe.Add(ref destinationRowRef, offsetX) = Unsafe.Add(ref sourceRef, ((sourceRowOffset + offsetX) * componentCount) + componentIndex);
                }
            }
        }
    }
}