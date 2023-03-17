// ------------------------------------------------------------------------------
// <copyright file="InvocationExpressionFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassInvocationExpressionSyntax InvocationExpression(JassIdentifierNameSyntax identifierName, JassArgumentListSyntax argumentList)
        {
            return new JassInvocationExpressionSyntax(
                identifierName,
                argumentList);
        }

        public static JassInvocationExpressionSyntax InvocationExpression(JassIdentifierNameSyntax identifierName, params JassExpressionSyntax[] arguments)
        {
            return new JassInvocationExpressionSyntax(
                identifierName,
                ArgumentList(arguments));
        }

        public static JassInvocationExpressionSyntax InvocationExpression(JassIdentifierNameSyntax identifierName, IEnumerable<JassExpressionSyntax> arguments)
        {
            return new JassInvocationExpressionSyntax(
                identifierName,
                ArgumentList(arguments));
        }

        public static JassInvocationExpressionSyntax InvocationExpression(string name, JassArgumentListSyntax argumentList)
        {
            return new JassInvocationExpressionSyntax(
                ParseIdentifierName(name),
                argumentList);
        }

        public static JassInvocationExpressionSyntax InvocationExpression(string name, params JassExpressionSyntax[] arguments)
        {
            return new JassInvocationExpressionSyntax(
                ParseIdentifierName(name),
                ArgumentList(arguments));
        }

        public static JassInvocationExpressionSyntax InvocationExpression(string name, IEnumerable<JassExpressionSyntax> arguments)
        {
            return new JassInvocationExpressionSyntax(
                ParseIdentifierName(name),
                ArgumentList(arguments));
        }
    }
}