namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileActionStatement(
            ImmutableArray<JassStatementSyntax> statements,
            ref int i,
            ref List<TriggerFunction> functions)
        {
            return statements[i] switch
            {
                JassSetStatementSyntax setStatement => TryDecompileSetStatement(setStatement, statements, ref i, ref functions),
                JassCallStatementSyntax callStatement => TryDecompileCallStatement(callStatement, ref functions),
                JassIfStatementSyntax ifStatement => TryDecompileIfStatement(ifStatement, ref functions),
                JassLoopStatementSyntax loopStatement => TryDecompileLoopStatement(loopStatement, ref functions),
                JassReturnStatementSyntax returnStatement => TryDecompileReturnStatement(returnStatement, ref functions),

                _ => false,
            };
        }

        /// <param name="returnValue"><see langword="true"/> for AND conditions, <see langword="false"/> for OR conditions.</param>
        private bool TryDecompileConditionStatement(
            JassStatementSyntax statement,
            bool returnValue,
            [NotNullWhen(true)] out TriggerFunction? function)
        {
            function = null;

            return statement switch
            {
                JassIfStatementSyntax ifStatement => TryDecompileIfStatement(ifStatement, returnValue, out function),
                JassReturnStatementSyntax returnStatement => TryDecompileReturnStatement(returnStatement, out function),

                _ => false,
            };
        }
    }
}