// ------------------------------------------------------------------------------
// <copyright file="JassFunctionDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassFunctionDeclarationSyntax : ITopLevelDeclarationSyntax
    {
        public JassFunctionDeclarationSyntax(JassFunctionDeclaratorSyntax functionDeclarator, JassStatementListSyntax body)
        {
            FunctionDeclarator = functionDeclarator;
            Body = body;
        }

        public JassFunctionDeclaratorSyntax FunctionDeclarator { get; init; }

        public JassStatementListSyntax Body { get; init; }

        public bool Equals(ITopLevelDeclarationSyntax? other)
        {
            return other is JassFunctionDeclarationSyntax functionDeclaration
                && FunctionDeclarator.Equals(functionDeclaration.FunctionDeclarator)
                && Body.Equals(functionDeclaration.Body);
        }

        public override string ToString() => $"{FunctionDeclarator} [{Body.Statements.Length}]";
    }
}