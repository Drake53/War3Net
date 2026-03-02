namespace War3Net.Replay
{
    public sealed class CommandBlock : IEnumerable<ActionBlock>
    {
        private readonly List<ActionBlock> _actionBlocks;

        internal CommandBlock()
        {
            _actionBlocks = new List<ActionBlock>();
        }

        public static CommandBlock Parse(Stream data)
        {
            var block = new CommandBlock();

            var playerId = data.ReadByte();

            var length = data.ReadWord();
            var offset = data.Position + length;
            while (data.Position < offset)
            {
                block._actionBlocks.Add(ActionBlock.Parse(data, offset));
            }

            if (data.Position > offset)
            {
                throw new InvalidDataException();
            }

            return block;
        }

        public IEnumerator<ActionBlock> GetEnumerator()
        {
            return ((IEnumerable<ActionBlock>)_actionBlocks).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<ActionBlock>)_actionBlocks).GetEnumerator();
        }
    }
}