// ------------------------------------------------------------------------------
// <copyright file="UnaryExpressionDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        private bool TryDecompileUnaryExpression(
            JassUnaryExpressionSyntax unaryExpression,
            string expectedType,
            [NotNullWhen(true)] out TriggerFunctionParameter? functionParameter)
        {
            switch (unaryExpression.OperatorToken.SyntaxKind)
            {
                case JassSyntaxKind.PlusToken:
                case JassSyntaxKind.MinusToken:
                    if (string.Equals(expectedType, JassKeyword.Integer, StringComparison.Ordinal) ||
                        string.Equals(expectedType, JassKeyword.Real, StringComparison.Ordinal))
                    {
                        if (TryDecompileTriggerFunctionParameter(unaryExpression.Expression, expectedType, out functionParameter))
                        {
                            functionParameter.Value = JassSyntaxFacts.GetText(unaryExpression.OperatorToken.SyntaxKind) + functionParameter.Value;
                            return true;
                        }
                    }

                    break;
            }

            functionParameter = null;
            return false;
        }

        private bool TryDecompileUnaryExpression(
            JassUnaryExpressionSyntax unaryExpression,
            [NotNullWhen(true)] out List<DecompileOption>? decompileOptions)
        {
            switch (unaryExpression.OperatorToken.SyntaxKind)
            {
                case JassSyntaxKind.PlusToken:
                case JassSyntaxKind.MinusToken:
                    var result = new List<DecompileOption>();

                    if (TryDecompileUnaryExpression(unaryExpression, JassKeyword.Integer, out var functionParameterInt))
                    {
                        result.Add(new DecompileOption
                        {
                            Type = JassKeyword.Integer,
                            Parameter = functionParameterInt,
                        });
                    }

                    if (TryDecompileUnaryExpression(unaryExpression, JassKeyword.Real, out var functionParameterReal))
                    {
                        result.Add(new DecompileOption
                        {
                            Type = JassKeyword.Real,
                            Parameter = functionParameterReal,
                        });
                    }

                    if (result.Count > 0)
                    {
                        decompileOptions = result;
                        return true;
                    }

                    break;
            }

            decompileOptions = null;
            return false;
        }
    }
}