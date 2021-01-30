// ------------------------------------------------------------------------------
// <copyright file="VariableDeclaratorTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public VariableDeclarationSyntax Transpile(IVariableDeclaratorSyntax declarator)
        {
            return declarator switch
            {
                JassArrayDeclaratorSyntax arrayDeclarator => Transpile(arrayDeclarator),
                JassVariableDeclaratorSyntax variableDeclarator => Transpile(variableDeclarator),
            };
        }

        public VariableDeclarationSyntax Transpile(JassVariableDeclaratorSyntax variableDeclarator)
        {
            var type = Transpile(variableDeclarator.Type);
            if (variableDeclarator.Value is null)
            {
                return SyntaxFactory.VariableDeclaration(
                    type,
                    SyntaxFactory.SingletonSeparatedList(SyntaxFactory.VariableDeclarator(
                        Transpile(variableDeclarator.IdentifierName),
                        null,
                        SyntaxFactory.EqualsValueClause(SyntaxFactory.DefaultExpression(type)))));
            }
            else
            {
                return SyntaxFactory.VariableDeclaration(
                    type,
                    SyntaxFactory.SingletonSeparatedList(SyntaxFactory.VariableDeclarator(
                        Transpile(variableDeclarator.IdentifierName),
                        null,
                        Transpile(variableDeclarator.Value))));
            }
        }
    }
}