// ------------------------------------------------------------------------------
// <copyright file="TimeslotBlock.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.IO;

using War3Net.Common.Extensions;

namespace War3Net.Replay
{
    public sealed class TimeslotBlock : ReplayBlock, IEnumerable<CommandBlock>
    {
        private readonly List<CommandBlock> _commandBlocks;

        internal TimeslotBlock(Stream data)
        {
            var length = data.ReadWord();
            var offset = data.Position + length;

            var timeIncrement = data.ReadWord();

            _commandBlocks = new List<CommandBlock>();
            while (data.Position < offset)
            {
                _commandBlocks.Add(CommandBlock.Parse(data));
            }

            if (data.Position > offset)
            {
                throw new InvalidDataException();
            }
        }

        public IEnumerator<CommandBlock> GetEnumerator()
        {
            return ((IEnumerable<CommandBlock>)_commandBlocks).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<CommandBlock>)_commandBlocks).GetEnumerator();
        }
    }
}