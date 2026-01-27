// ------------------------------------------------------------------------------
// <copyright file="ParameterListTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public IEnumerable<LuaIdentifierNameSyntax> Transpile(JassParameterListOrEmptyParameterListSyntax parameterListOrEmptyParameterList)
        {
            return parameterListOrEmptyParameterList switch
            {
                JassParameterListSyntax parameterList => Transpile(parameterList),
                JassEmptyParameterListSyntax => Enumerable.Empty<LuaIdentifierNameSyntax>(),
            };
        }

        public IEnumerable<LuaIdentifierNameSyntax> Transpile(JassParameterListSyntax parameterList)
        {
            return parameterList.Parameters.Items.Select(Transpile);
        }
    }
}