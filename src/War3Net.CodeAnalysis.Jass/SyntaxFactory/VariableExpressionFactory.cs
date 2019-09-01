// ------------------------------------------------------------------------------
// <copyright file="VariableExpressionFactory.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1649 // File name should match first type name

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static NewExpressionSyntax VariableExpression(string variableName)
        {
            return new NewExpressionSyntax(
                new ExpressionSyntax(
                    new TokenNode(new SyntaxToken(SyntaxTokenType.AlphanumericIdentifier, variableName), 0)),
                new EmptyNode(0));
        }
    }
}