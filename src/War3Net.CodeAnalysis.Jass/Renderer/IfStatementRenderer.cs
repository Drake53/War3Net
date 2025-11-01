// ------------------------------------------------------------------------------
// <copyright file="IfStatementRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        public void Render(JassIfStatementSyntax ifStatement)
        {
            Render(ifStatement.IfClause);

            foreach (var elseIfClause in ifStatement.ElseIfClauses)
            {
                Render(elseIfClause);
            }

            if (ifStatement.ElseClause is not null)
            {
                Render(ifStatement.ElseClause);
            }

            Render(ifStatement.EndIfToken);
        }
    }
}