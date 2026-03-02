namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public StatementSyntax Transpile(JassStatementSyntax statement)
        {
            return statement switch
            {
                JassLocalVariableDeclarationStatementSyntax localVariableDeclarationStatement => Transpile(localVariableDeclarationStatement),
                JassSetStatementSyntax setStatement => Transpile(setStatement),
                JassCallStatementSyntax callStatement => Transpile(callStatement),
                JassIfStatementSyntax ifStatement => Transpile(ifStatement),
                JassLoopStatementSyntax loopStatement => Transpile(loopStatement),
                JassExitStatementSyntax exitStatement => Transpile(exitStatement),
                JassReturnStatementSyntax returnStatement => Transpile(returnStatement),
                JassDebugStatementSyntax debugStatement => Transpile(debugStatement),
            };
        }
    }
}