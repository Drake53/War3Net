using System;
using System.IO;

namespace War3Net.Replay
{
    public sealed class EncodedString
    {
        public EncodedString(BinaryReader reader)
        {
            while (true)
            {
                var read = reader.ReadByte();
                if (read == char.MinValue)
                {
                    break;
                }

                // todo
            }
        }

        public void Decode()
        {
            throw new NotImplementedException();
        }
    }
}