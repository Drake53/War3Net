// ------------------------------------------------------------------------------
// <copyright file="LocalVariableDeclarationStatementFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassLocalVariableDeclarationStatementSyntax LocalVariableDeclarationStatement(JassTypeSyntax type, string name)
        {
            return new JassLocalVariableDeclarationStatementSyntax(
                new JassVariableDeclaratorSyntax(
                    type,
                    ParseIdentifierName(name),
                    null));
        }

        public static JassLocalVariableDeclarationStatementSyntax LocalVariableDeclarationStatement(JassTypeSyntax type, string name, IExpressionSyntax value)
        {
            return new JassLocalVariableDeclarationStatementSyntax(
                new JassVariableDeclaratorSyntax(
                    type,
                    ParseIdentifierName(name),
                    new JassEqualsValueClauseSyntax(value)));
        }

        public static JassLocalVariableDeclarationStatementSyntax LocalArrayDeclarationStatement(JassTypeSyntax type, string name)
        {
            return new JassLocalVariableDeclarationStatementSyntax(
                new JassArrayDeclaratorSyntax(
                    type, ParseIdentifierName(name)));
        }
    }
}