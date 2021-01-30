// ------------------------------------------------------------------------------
// <copyright file="StatementListTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public IEnumerable<StatementSyntax> Transpile(JassStatementListSyntax statementList)
        {
            return statementList.Statements.Select(Transpile);
        }
    }
}