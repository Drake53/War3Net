namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        public JassScriptDecompiler(Map map)
            : this(map, null, null)
        {
        }

        public JassScriptDecompiler(Map map, Campaign? campaign)
            : this(map, campaign, null)
        {
        }

        public JassScriptDecompiler(Map map, TriggerData? triggerData)
            : this(map, null, triggerData)
        {
        }

        public JassScriptDecompiler(Map map, Campaign? campaign, TriggerData? triggerData)
        {
            Context = new DecompilationContext(map, campaign, triggerData);
        }

        internal DecompilationContext Context { get; }

        private FunctionDeclarationContext? GetFunction(string functionName)
        {
            if (Context.FunctionDeclarations.TryGetValue(functionName, out var functionDeclaration))
            {
                if (functionDeclaration.Handled)
                {
                    throw new ArgumentException("Function has already been handled.", nameof(functionName));
                }

                return functionDeclaration;
            }

            return null;
        }

        private IEnumerable<FunctionDeclarationContext> GetCandidateFunctions(string? expectedFunctionName = null)
        {
            if (Context.FunctionDeclarations.TryGetValue("main", out var mainFunction))
            {
                if (!string.IsNullOrEmpty(expectedFunctionName) && Context.FunctionDeclarations.TryGetValue(expectedFunctionName, out var expectedFunction))
                {
                    if (expectedFunction.Handled)
                    {
                        throw new ArgumentException("Expected function has already been handled.", nameof(expectedFunctionName));
                    }

                    yield return expectedFunction;
                }

                foreach (var statement in mainFunction.FunctionDeclaration.Statements)
                {
                    if (statement is JassCallStatementSyntax callStatement && callStatement.ArgumentList.Arguments.Items.IsEmpty)
                    {
                        if (string.Equals(callStatement.IdentifierName.Token.Text, expectedFunctionName, StringComparison.Ordinal))
                        {
                            continue;
                        }

                        if (Context.FunctionDeclarations.TryGetValue(callStatement.IdentifierName.Token.Text, out var candidateFunction) &&
                            candidateFunction.IsActionsFunction &&
                            !candidateFunction.Handled)
                        {
                            yield return candidateFunction;
                        }
                    }
                }
            }
        }
    }
}