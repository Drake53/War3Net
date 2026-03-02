namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private readonly Dictionary<string, string> _functionDeclarationRenames;
        private readonly Dictionary<string, string> _globalVariableRenames;
        private readonly HashSet<string> _localVariableNames;

        public JassRenamer(
            Dictionary<string, string> functionDeclarationRenames,
            Dictionary<string, string> globalVariableRenames)
        {
            _functionDeclarationRenames = functionDeclarationRenames;
            _globalVariableRenames = globalVariableRenames;
            _localVariableNames = new(StringComparer.Ordinal);
        }

        public bool RenameExecuteFuncArgument { get; set; }

        private bool TryRenameDummy<TSyntax>(TSyntax? syntax, out TSyntax? renamed)
            where TSyntax : class
        {
            renamed = null;
            return false;
        }
    }
}