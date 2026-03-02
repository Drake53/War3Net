namespace War3Net.CodeAnalysis.Jass
{
    internal sealed partial class JassSyntaxNormalizer : JassSyntaxRewriter
    {
        private readonly List<JassSyntaxNode> _nodes;
        private readonly bool _addSpacesToOuterInvocation;
        private readonly bool _trimComments;
        private readonly string _indentationString;

        private JassSyntaxToken _previousToken;
        private JassSyntaxToken _currentToken;
        private JassSyntaxNode? _previousNode;
        private JassSyntaxNode? _previousNodeParent;
        private JassSyntaxNode? _previousNodeGrandParent;

        private int _currentLevelOfIndentation;
        private bool _encounteredAnyTextOnCurrentLine;
        private bool _requireNewLineTrivia;

        public JassSyntaxNormalizer(
            bool addSpacesToOuterInvocation = true,
            bool trimComments = false,
            string indentationString = "    ")
        {
            _nodes = new List<JassSyntaxNode>();
            _addSpacesToOuterInvocation = addSpacesToOuterInvocation;
            _trimComments = trimComments;
            _indentationString = indentationString;

            _previousToken = new JassSyntaxToken(JassSyntaxTriviaList.Empty, JassSyntaxKind.None, string.Empty, JassSyntaxTriviaList.Empty);
            _currentToken = _previousToken;
            _previousNode = null;
            _previousNodeParent = null;
            _previousNodeGrandParent = null;

            _currentLevelOfIndentation = 0;
            _encounteredAnyTextOnCurrentLine = false;
            _requireNewLineTrivia = false;
        }
    }
}