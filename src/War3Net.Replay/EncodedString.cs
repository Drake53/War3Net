// ------------------------------------------------------------------------------
// <copyright file="EncodedString.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

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