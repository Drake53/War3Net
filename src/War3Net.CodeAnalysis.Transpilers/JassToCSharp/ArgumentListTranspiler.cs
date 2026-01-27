// ------------------------------------------------------------------------------
// <copyright file="ArgumentListTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Linq;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public ArgumentListSyntax Transpile(JassArgumentListSyntax argumentList)
        {
            return SyntaxFactory.ArgumentList(
                Transpile(SyntaxKind.OpenParenToken, argumentList.OpenParenToken),
                SyntaxFactory.SeparatedList(
                    argumentList.Arguments.Items.Select(TranspileArgument),
                    argumentList.Arguments.Separators.Select(Transpile)),
                Transpile(SyntaxKind.CloseParenToken, argumentList.CloseParenToken));
        }
    }
}