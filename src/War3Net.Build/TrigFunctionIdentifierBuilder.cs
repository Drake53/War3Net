namespace War3Net.Build
{
    public class TrigFunctionIdentifierBuilder
    {
        private readonly StringBuilder _stringBuilder;
        private readonly Stack<int> _partLengths;

        public TrigFunctionIdentifierBuilder(string identifierBase)
        {
            _stringBuilder = new StringBuilder();
            _partLengths = new Stack<int>();

            _stringBuilder.Append(identifierBase);
        }

        public void Append(int i)
        {
            Append(i.ToString("D3", CultureInfo.InvariantCulture));
        }

        public void Append(string s)
        {
            _stringBuilder.Append(s);
            _partLengths.Push(s.Length);
        }

        public void Remove()
        {
            var lengthToRemove = _partLengths.Pop();
            _stringBuilder.Remove(_stringBuilder.Length - lengthToRemove, lengthToRemove);
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
    }
}