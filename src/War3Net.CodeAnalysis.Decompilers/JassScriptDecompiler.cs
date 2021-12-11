// ------------------------------------------------------------------------------
// <copyright file="JassScriptDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using War3Net.Build;
using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Syntax;

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

                foreach (var statement in mainFunction.FunctionDeclaration.Body.Statements)
                {
                    if (statement is JassCallStatementSyntax callStatement && callStatement.Arguments.Arguments.IsEmpty)
                    {
                        if (string.Equals(callStatement.IdentifierName.Name, expectedFunctionName, StringComparison.Ordinal))
                        {
                            continue;
                        }

                        if (Context.FunctionDeclarations.TryGetValue(callStatement.IdentifierName.Name, out var candidateFunction) &&
                            candidateFunction.IsActionsFunction &&
                            !candidateFunction.Handled)
                        {
                            yield return candidateFunction;
                        }
                    }
                }
            }
        }

        private static IExpressionSyntax DeparenthesizeExpression(IExpressionSyntax expression)
        {
            while (expression is JassParenthesizedExpressionSyntax parenthesizedExpression)
            {
                expression = parenthesizedExpression.Expression;
            }

            return expression;
        }
    }
}