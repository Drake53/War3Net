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
            return SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(argumentList.Arguments.Select(argument => SyntaxFactory.Argument(Transpile(argument)))));
        }
    }
}