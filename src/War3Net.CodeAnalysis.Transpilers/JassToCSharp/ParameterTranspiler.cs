// ------------------------------------------------------------------------------
// <copyright file="ParameterTranspiler.cs" company="Drake53">
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
        public ParameterSyntax Transpile(JassParameterSyntax parameter)
        {
            return SyntaxFactory.Parameter(
                default,
                SyntaxFactory.TokenList(),
                Transpile(parameter.Type),
                Transpile(parameter.IdentifierName.Token),
                null);
        }
    }
}