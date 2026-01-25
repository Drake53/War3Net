// ------------------------------------------------------------------------------
// <copyright file="ElementAccessClauseTranspiler.cs" company="Drake53">
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
        public BracketedArgumentListSyntax Transpile(JassElementAccessClauseSyntax elementAccessClause)
        {
            return SyntaxFactory.BracketedArgumentList(
                Transpile(SyntaxKind.OpenBracketToken, elementAccessClause.OpenBracketToken),
                SyntaxFactory.SingletonSeparatedList(TranspileArgument(elementAccessClause.Expression)),
                Transpile(SyntaxKind.CloseBracketToken, elementAccessClause.CloseBracketToken));
        }
    }
}