// ------------------------------------------------------------------------------
// <copyright file="ParameterListTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public SeparatedSyntaxList<ParameterSyntax> Transpile(JassParameterListOrEmptyParameterListSyntax parameterListOrEmptyParameterList)
        {
            return SyntaxFactory.SeparatedList(parameterListOrEmptyParameterList.GetParameters().Select(Transpile));
        }
    }
}