// ------------------------------------------------------------------------------
// <copyright file="VJassMethodDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassMethodDeclarationSyntax : IMemberDeclarationSyntax
    {
        public VJassMethodDeclarationSyntax(
            IMethodDeclaratorSyntax methodDeclarator,
            VJassStatementListSyntax body)
        {
            MethodDeclarator = methodDeclarator;
            Body = body;
        }

        public IMethodDeclaratorSyntax MethodDeclarator { get; }

        public VJassStatementListSyntax Body { get; }

        public bool Equals(IMemberDeclarationSyntax? other)
        {
            return other is VJassMethodDeclarationSyntax functionDeclaration
                && MethodDeclarator.Equals(functionDeclaration.MethodDeclarator)
                && Body.Equals(functionDeclaration.Body);
        }

        public override string ToString() => $"{MethodDeclarator} [{Body.Statements.Length}]";
    }
}