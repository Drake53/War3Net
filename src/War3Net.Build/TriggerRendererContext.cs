namespace War3Net.Build
{
    public class TriggerRendererContext
    {
        private readonly IndentedTextWriter _writer;
        private readonly TrigFunctionIdentifierBuilder _builder;

        public TriggerRendererContext(
            IndentedTextWriter writer,
            TrigFunctionIdentifierBuilder builder)
        {
            _writer = writer;
            _builder = builder;
        }

        public IndentedTextWriter Writer => _writer;

        public TrigFunctionIdentifierBuilder TrigFunctionIdentifierBuilder => _builder;
    }
}