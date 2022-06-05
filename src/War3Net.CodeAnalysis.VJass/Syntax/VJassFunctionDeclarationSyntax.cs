// ------------------------------------------------------------------------------
// <copyright file="VJassFunctionDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassFunctionDeclarationSyntax : ITopLevelDeclarationSyntax, IScopedDeclarationSyntax
    {
        public VJassFunctionDeclarationSyntax(
            VJassFunctionDeclaratorSyntax functionDeclarator,
            VJassStatementListSyntax body)
        {
            FunctionDeclarator = functionDeclarator;
            Body = body;
        }

        public VJassFunctionDeclaratorSyntax FunctionDeclarator { get; }

        public VJassStatementListSyntax Body { get; }

        public bool Equals(ITopLevelDeclarationSyntax? other)
        {
            return other is VJassFunctionDeclarationSyntax functionDeclaration
                && FunctionDeclarator.Equals(functionDeclaration.FunctionDeclarator)
                && Body.Equals(functionDeclaration.Body);
        }

        public bool Equals(IScopedDeclarationSyntax? other)
        {
            return other is VJassFunctionDeclarationSyntax functionDeclaration
                && FunctionDeclarator.Equals(functionDeclaration.FunctionDeclarator)
                && Body.Equals(functionDeclaration.Body);
        }

        public override string ToString() => $"{FunctionDeclarator} [{Body.Statements.Length}]";
    }
}